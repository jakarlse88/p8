using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.BookingService.Repositories
{
    public class ConsultantRepository : RepositoryBase<Consultant>, IConsultantRepository
    {
        public ConsultantRepository(BookingContext context) : base(context)
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