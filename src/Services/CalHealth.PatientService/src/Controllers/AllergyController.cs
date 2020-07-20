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
    public class AllergyController : ControllerBase
    {
        private readonly IAllergyService _religionService;

        public AllergyController(IAllergyService religionService)
        {
            _religionService = religionService;
        }
        
        /// <summary>
        /// Gets all <see cref="Allergy"/> entities as <seealso cref="AllergyDTO"/> data transfer objects.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all results.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AllergyDTO>>> Get()
        {
            var result = await _religionService.GetAllAsync();

            return Ok(result);
        }
    }
}