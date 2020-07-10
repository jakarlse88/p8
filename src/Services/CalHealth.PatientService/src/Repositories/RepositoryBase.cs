using System;
using System.Linq;
using System.Linq.Expressions;
using CalHealth.PatientService.Data;

namespace CalHealth.PatientService.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {

        private readonly PatientContext _context;

        public RepositoryBase(PatientContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            var result = _context
                .Set<TEntity>()
                .Where(predicate);

            return result;
        }
    }
}