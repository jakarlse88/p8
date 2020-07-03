using System;
using System.Linq;
using System.Linq.Expressions;

namespace CalHealth.CalendarService.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity>
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
    }
}