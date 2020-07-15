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
        /// Gets all <see cref="Patient"/> entities as <seealso cref="PatientDTO"/> data transfer objects.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all results.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDTO>> Get()
        {
            var results = await _patientService.GetAllAsync();

            return Ok(results);
        }
    }
}