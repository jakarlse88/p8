using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CalHealth.BookingService.Services;

namespace CalHealth.BookingService.Controllers
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
        public async Task<ActionResult<IEnumerable<TimeSlot>>> Get()
        {
            var result = await _timeSlotService.GetAllAsDTOAsync();

            return Ok(result);
        }
        
    }
}