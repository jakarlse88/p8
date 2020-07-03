using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.CalendarService.Repositories
{
    public class ConsultantRepository : RepositoryBase<Consultant>, IConsultantRepository
    {
        public ConsultantRepository(CalendarContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Consultant>> GetAllAsync()
        {
            var result = 
                await base
                    .Get(_ => true)
                    .Include(c => c.Specialty)
                    .ToListAsync();

            return result;
        }
    }
}