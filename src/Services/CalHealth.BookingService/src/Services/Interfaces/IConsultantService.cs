using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Services
{
    public interface IConsultantService
    {
        Task<IEnumerable<ConsultantDTO>> GetAllAsDTOAsync();
    }
}