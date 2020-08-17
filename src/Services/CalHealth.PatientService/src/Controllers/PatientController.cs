using System;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalHealth.PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Gets all <see cref="Patient"/> entities.
        /// </summary>
        /// <returns>A list of all existing entities as <see cref="PatientDTO"/> data transfer objects.</returns>
        /// <response code="200">Query OK.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDTO>> Get()
        {
            var results = await _patientService.GetAllAsync();

            return Ok(results);
        }

        /// <summary>
        /// Verify whether a <see cref="Patient"/> entity exists based on his/her personal information.
        /// </summary>
        /// <param name="firstName">First name of patient.</param>
        /// <param name="lastName">Last name of patient.</param>
        /// <param name="dateOfBirth">Date of birth of patient.</param>
        /// <returns>A boolean value indicating whether or not the patient exists in the DB.</returns>
        [HttpGet]
        [Route("Exists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> PatientExists(string firstName, string lastName, string dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return BadRequest();
            }
            
            var results = await _patientService.GetAllAsync();

            var exists = results.Any((p => p.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                && p.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase) 
                && p.DateOfBirth.ToShortDateString().Equals(dateOfBirth)));

            return Ok(exists);
        }
    }
}