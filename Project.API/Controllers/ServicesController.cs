using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Core.Repositories;

namespace Project.API.Controllers
{
    public class ServicesController : APIBaseController
    {
        private readonly IServices _services;

        public ServicesController(IServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmergencyServices>>> GetAllServices()
        {
            var Videos = await _services.GetAllServicesAsync();
            return Ok(Videos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> getServiceById(int id)
        {
            var video = await _services.GetServicesByIdAsync(id);
            return Ok(video);
        }
    }
}
