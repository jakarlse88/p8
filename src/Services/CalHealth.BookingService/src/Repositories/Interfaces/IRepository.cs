using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntityBase
    {
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
    }
}