using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CalHealth.PatientService.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate);
    }
}
