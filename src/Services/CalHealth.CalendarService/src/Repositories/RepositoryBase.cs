using System;
using System.Linq;
using System.Linq.Expressions;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Repositories.Interfaces;

namespace CalHealth.CalendarService.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> 
        where TEntity : class
    {
        private readonly CalendarContext _context;

        public RepositoryBase(CalendarContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets every occurence of a given entity that satisfy
        /// a supplied predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = _context
                .Set<TEntity>()
                .Where(predicate);

            return result;
        }
    }
}