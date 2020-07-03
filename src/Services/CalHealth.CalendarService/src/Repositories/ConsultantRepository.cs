using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Models.DTOs;
using CalHealth.CalendarService.Repositories.Interfaces;

namespace CalHealth.CalendarService.Repositories
{
    public class ConsultantRepository : RepositoryBase<Consultant>, IConsultantRepository
    {
        public ConsultantRepository(CalendarContext context) : base(context)
        {
        }
    }
}