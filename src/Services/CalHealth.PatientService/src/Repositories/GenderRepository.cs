using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.PatientService.Repositories
{
    public class GenderRepository : RepositoryBase<Gender>, IGenderRepository
    {
        public GenderRepository(PatientContext context) : base(context)
        { 
        }

        public async Task<IEnumerable<Gender>> GetAllAsync()
        {
            var result = await base
                .GetByCondition(_ => true)
                .ToListAsync();

            return result;
        }
    }
}