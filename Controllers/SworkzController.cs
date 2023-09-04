using Microsoft.AspNetCore.Mvc;
using IntegrationsApi.Models;
using IntegrationsApi.Services;

namespace IntegrationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SworkzController : ControllerBase
    {
        private readonly SworkzService _sworkzService;

        public SworkzController(SworkzService sworkzService)
        {
            _sworkzService = sworkzService;
        }

        /// <summary>
        /// Get all careers from Sworkz website
        /// </summary>
        [HttpGet("Careers/")]
        public async Task<IActionResult> GetSworkzCareers()
        {
            try
            {
                var userData = await _sworkzService.GetSworkzCareers();
                return Ok(userData);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        
    }
}
