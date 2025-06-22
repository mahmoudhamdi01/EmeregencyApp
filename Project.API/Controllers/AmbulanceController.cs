using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Repositor.Data;

namespace Project.API.Controllers
{
	public class AmbulanceController : APIBaseController
	{
		private readonly EmergencyContext _context;

		public AmbulanceController(EmergencyContext context)
        {
			_context = context;
		}

		[HttpGet("emergency-requests")]
		public async Task<ActionResult<IEnumerable<EmergencyRequest>>> GetAmbulanceRequests()
		{
			var requests = await _context.emergencyRequests
										 .Where(r => r.ServiceId == 2)
										 .OrderByDescending(r => r.TimeStamp)
										 .ToListAsync();
			return Ok(requests);
		}
	}
}
