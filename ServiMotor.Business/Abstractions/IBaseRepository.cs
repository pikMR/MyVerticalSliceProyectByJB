using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Business.Shared
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity obj);
        Task<TEntity> GetAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void DeleteAll();
        Task<TEntity> GetFirstAsync();
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> UpdateAsync(TEntity obj);
        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id);
    }
}
