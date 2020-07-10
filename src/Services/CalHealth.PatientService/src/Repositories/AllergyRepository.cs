using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.PatientService.Repositories
{
    public class AllergyRepository : RepositoryBase<Allergy>, IAllergyRepository
    {
        public AllergyRepository(PatientContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Allergy>> GetAllAsync()
        {
            var result = await
                base.GetByCondition(_ => true)
                    .ToListAsync();

            return result;
        }
    }
}