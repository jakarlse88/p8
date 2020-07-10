using System;
using System.Linq;
using System.Linq.Expressions;

namespace CalHealth.BookingService.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity Create(TEntity entity);
    }
}