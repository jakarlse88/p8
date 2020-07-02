using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalHealth.CalendarService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;

        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        /// <summary>
        /// Gets all <see cref="TimeSlot"/> entities.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Request OK, return results.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> Get()
        {
            var result = await _timeSlotService.GetAllAsDTOAsync();

            return Ok(result);
        }
        
    }
}