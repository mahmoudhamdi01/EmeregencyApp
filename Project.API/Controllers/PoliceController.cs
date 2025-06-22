using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Repositor.Data;

namespace Project.API.Controllers
{
	public class PoliceController : APIBaseController
	{

		private readonly EmergencyContext _context;
		public PoliceController(EmergencyContext context)
		{
			_context = context;
		}

		[HttpGet("emergency-requests")]
		public async Task<ActionResult<IEnumerable<EmergencyRequest>>> GetPoliceRequests()
		{
			var requests = await _context.emergencyRequests
										 .Where(r => r.ServiceId == 3)
										 .OrderByDescending(r => r.TimeStamp)
										 .ToListAsync();
			return Ok(requests);
		}
	}
}
