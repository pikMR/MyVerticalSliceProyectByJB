using MongoDB.Bson;
using MongoDB.Driver;
using ServiMotor.Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Infraestructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoBookDBContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;

        public BaseRepository(IMongoBookDBContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task CreateAsync(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            await _dbCollection.InsertOneAsync(obj);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        }

        public void DeleteAll()
        {
            _dbCollection.DeleteManyAsync(Builders<TEntity>.Filter.Empty);
        }

        public async Task<TEntity> GetAsync(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();

        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        {
            return await _dbCollection.Find(predicate).ToListAsync(cancellationToken: cancellationToken)!;
        }

        public async Task<TEntity> GetFirstAsync()
        {
            return await _dbCollection.Find(FilterDefinition<TEntity>.Empty).FirstOrDefaultAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {
            var id = obj.GetType().GetProperty("_id").GetValue(obj, null);
            await _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", id), obj);
            return obj;
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
