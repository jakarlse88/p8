using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.CalendarService.Models.DTOs;

namespace CalHealth.CalendarService.Services.Interfaces
{
    public interface IConsultantService
    {
        Task<IEnumerable<ConsultantDTO>> GetAllAsDTOAsync();
    }
}