using CalHealth.BookingService.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CalHealth.BookingService.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> 
        where TEntity : class
    {
        private readonly BookingContext _context;

        public RepositoryBase(BookingContext context)
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

        /// <summary>
        /// Begins tracking a given entity in the 'Added' state.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Add(entity);

            return entity;
        }
    }
}