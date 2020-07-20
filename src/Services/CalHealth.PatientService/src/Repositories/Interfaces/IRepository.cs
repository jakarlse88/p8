using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntityBase
    {
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, bool eager = false);
        Task<IEnumerable<TEntity>> GetAllAsync(bool eager = false);
        void Insert(TEntity entity);
        void Update(TEntity entity);
    }
}