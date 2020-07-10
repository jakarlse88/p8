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
    public class ReligionController : ControllerBase
    {
        private readonly IReligionService _religionService;

        public ReligionController(IReligionService religionService)
        {
            _religionService = religionService;
        }
        
        /// <summary>
        /// Gets all <see cref="Religion"/> entities as <seealso cref="ReligionDTO"/> data transfer objects.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all results.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReligionDTO>>> Get()
        {
            var result = await _religionService.GetAllAsync();

            return Ok(result);
        }
    }
}