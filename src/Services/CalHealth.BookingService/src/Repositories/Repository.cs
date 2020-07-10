﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.BookingService.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntityBase
    {
        private readonly BookingContext _context;

        public Repository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var results = 
                await _context
                    .Set<TEntity>()
                    .Where(predicate)
                    .ToListAsync();

            return results;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var result = await _context.Set<TEntity>().ToListAsync();

            return result;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var result = await 
                _context
                    .Set<TEntity>()
                    .Where(e => e.Id == id)
                    .FirstOrDefaultAsync();

            return result;
        }

        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            
            await _context.Set<TEntity>().AddAsync(entity);
        }
    }
}