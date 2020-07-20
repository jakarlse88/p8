using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalHealth.PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;

        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }
        
        /// <summary>
        /// Gets all <see cref="Gender"/> entities as <seealso cref="GenderDTO"/> data transfer objects.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all results.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenderDTO>>> Get()
        {
            var result =
                await _genderService.GetAllAsync();

            return Ok(result);
        }
    }
}