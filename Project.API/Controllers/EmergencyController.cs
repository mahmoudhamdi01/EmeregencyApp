using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.DTOs;
using Project.API.Services;

namespace Project.API.Controllers
{
    
    public class EmergencyController : APIBaseController
    {
        private readonly IEmergencyService _emergencyService;

        public EmergencyController(IEmergencyService emergencyService)
        {
            _emergencyService = emergencyService;
        }

        [HttpGet("{serviceName}")]
        public async Task<ActionResult<IEnumerable<EmergencyRequestDTO>>> GetEmergencyRequestsForService(string serviceName)
        {
            var requests = await _emergencyService.GetRequestsForService(serviceName);

            if (!requests.Any())
                return NotFound($"No requests found for service '{serviceName}'.");

            return Ok(requests);
        }
    }
}
