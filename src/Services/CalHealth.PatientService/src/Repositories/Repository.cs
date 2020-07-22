using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.PatientService.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntityBase
    {
        private readonly PatientContext _context;

        public Repository(PatientContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the subset of <typeparamref name="TEntity"/> entities corresponding to the predicate <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">Filter which entities are returned.</param>
        /// <param name="eager">Indicates whether or not to eagerly load navigation properties.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, bool eager = false)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            var query = _context.Set<TEntity>().AsQueryable();
            query = query.Where(predicate);

            if (eager)
            {
                var navigations = _context.Model.FindEntityType(typeof(TEntity))
                    .GetDerivedTypesInclusive()
                    .SelectMany(type => type.GetNavigations())
                    .Distinct();

                foreach (var property in navigations)
                {
                    query = query.Include(property.Name);
                }
            }

            var results = await query.ToListAsync();

            return results ?? new List<TEntity>();
        }

        /// <summary>
        /// Get all <typeparamref name="TEntity"/> entities.
        /// </summary>
        /// <param name="eager">Indicates whether or not to eagerly load navigation properties.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool eager = false)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (eager)
            {
                var navigations = _context.Model.FindEntityType(typeof(TEntity))
                    .GetDerivedTypesInclusive()
                    .SelectMany(type => type.GetNavigations())
                    .Distinct();

                foreach (var property in navigations)
                {
                    query = query.Include(property.Name);
                }
            }

            var result = await query.ToListAsync();

            return result ?? new List<TEntity>();
        }

        /// <summary>
        /// Begins tracking a <typeparamref name="TEntity"/> entity in the "Added" state.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Begins tracking a <typeparamref name="TEntity"/> entity in the "Modified" state.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Update(entity);
        }
    }
}