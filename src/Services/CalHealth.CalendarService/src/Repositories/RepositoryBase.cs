using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var result = await _context.Set<TEntity>().ToListAsync();

            return result;
        }
    }
}