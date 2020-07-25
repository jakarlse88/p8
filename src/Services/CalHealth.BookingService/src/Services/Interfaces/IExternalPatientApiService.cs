using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Services
{
    public interface IExternalPatientApiService
    {
        Task<bool> PatientExists(PatientDTO patient);
    }
}