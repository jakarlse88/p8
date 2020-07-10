using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Repositories
{
    public interface IAllergyRepository
    {
        Task<IEnumerable<Allergy>> GetAllAsync();
    }
}