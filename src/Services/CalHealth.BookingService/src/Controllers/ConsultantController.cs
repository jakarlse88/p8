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
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _consultantService;

        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        /// <summary>
        /// Gets all <see cref="Consultant"/> entities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ConsultantDTO>>> Get()
        {
            var result = await _consultantService.GetAllAsDTOAsync();

            return Ok(result);
        }
    }
}