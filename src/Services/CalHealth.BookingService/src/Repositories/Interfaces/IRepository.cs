using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntityBase
    {
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, bool eager = false);
        Task<IEnumerable<TEntity>> GetAllAsync(bool eager = false);
        Task<TEntity> GetByIdAsync(int id, bool eager = false);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
    }
}