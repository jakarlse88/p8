using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Services
{
    public interface IGenderService
    {
        Task<IEnumerable<GenderDTO>> GetAllAsync();
    }
}