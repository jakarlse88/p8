using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.PatientService.Repositories
{
    public class ReligionRepository : RepositoryBase<Religion>, IReligionRepository
    {
        public ReligionRepository(PatientContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Religion>> GetAllAsync()
        {
            var result = await
                base.GetByCondition(_ => true)
                    .ToListAsync();

            return result;
        }
    }
}