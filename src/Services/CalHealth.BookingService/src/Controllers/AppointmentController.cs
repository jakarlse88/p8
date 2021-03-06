﻿using System.Collections.Generic;
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
        private readonly IExternalPatientApiService _externalPatientApiService;

        public AppointmentController(IAppointmentService appointmentService, IExternalPatientApiService externalPatientApiService)
        {
            _appointmentService = appointmentService;
            _externalPatientApiService = externalPatientApiService;
        }

        /// <summary>
        /// Gets an <see cref="Appointment"/> entity by ID.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns>The found entity as a <seealso cref="AppointmentDTO"/> data transfer object.</returns>
        /// <response code="200">Entity found.</response>
        /// <response code="400">Invalid <paramref name="appointmentId"/>.</response>
        /// <response code="404">Entity not found.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppointmentDTO>> Get(int appointmentId)
        {
            if (appointmentId < 1)
            {
                return BadRequest(nameof(appointmentId));
            }

            var result = await _appointmentService.GetByIdAsync(appointmentId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all <see cref="Appointment"/> entities associated with a given <see cref="Consultant"/> entity.
        /// </summary>
        /// <param name="consultantId">ID by which to query.</param>
        /// <returns>A list of all <see cref="Appointment"/> entities associated with the given <see cref="Consultant"/>.</returns>
        /// <response code="200">Query OK.</response>
        /// <response code="400">No <see cref="Consultant"/> entities exist with the given ID.</response>
        [HttpGet]
        [Route("consultant/{consultantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetByConsultant(int consultantId)
        {
            if (consultantId < 1)
            {
                return BadRequest(nameof(consultantId));
            }

            var results = 
                await _appointmentService.GetByConsultantAsync(consultantId);

            return Ok(results);
        }
        
        /// <summary>
        /// Creates a new <see cref="Appointment"/> entity.
        /// </summary>
        /// <returns><see cref="CreatedAtActionResult"/> with the created entity's ID.</returns>
        /// <response code="201">The entity was successfully created.</response>
        /// <response code="400">Bad request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AppointmentDTO dto)
        {
            if (dto == null)
            {
                return BadRequest($"The {nameof(dto)} parameter cannot be null.");
            }

            if (dto.Patient == null)
            {
                return BadRequest($"The {nameof(dto.Patient)} property of the {typeof(AppointmentDTO)} parameter cannot be null.");
            }

            if (! await _externalPatientApiService.PatientExists(dto.Patient))
            {
                return BadRequest("No patient entity matching the specified personal details was found.");
            }
            
            var model = await _appointmentService.CreateAsync(dto);
            
            if (model != null)
            {
                return CreatedAtAction("Get", new { model.Id }, model);    
            }

            return BadRequest("An error occurred creating the appointment.");
        }
    }
}