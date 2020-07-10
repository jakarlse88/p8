using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalHealth.BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<AppointmentDTO>> Get(int appointmentId)
        {
            if (appointmentId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(appointmentId));
            }

            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("consultant/{consultantId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetByConsultant(int consultantId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <returns></returns>
        /// <response code="201">The entity was successfully created.</response>
        /// <response code="400">Malformed request (arg null).</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AppointmentDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var entity = await _appointmentService.CreateAsync(dto);

            return CreatedAtAction("Get", new { entity.Id }, entity);
        }
        
        
    }
}