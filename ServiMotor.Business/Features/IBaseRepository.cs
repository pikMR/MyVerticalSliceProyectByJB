using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiMotor.Features.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(string id);
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> Get();
        void DeleteAll();
        Task<TEntity> GetFirstAsync();
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter);
    }
}
