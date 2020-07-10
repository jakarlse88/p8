using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Repositories
{
    public interface IGenderRepository
    {
        Task<IEnumerable<Gender>> GetAllAsync();
    }
}
