using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.API.Services;
using Project.Core.Entities;
using Project.Repositor.Data;

namespace Project.API.Controllers
{
   
    public class EmergencyRequestController : APIBaseController
    {
        private readonly IEmergencyRequestService _emergencyRequest;
        private readonly IEmergencyService _emergencyService;
        private readonly EmergencyContext _context;

		public EmergencyRequestController(IEmergencyRequestService emergencyRequest,
            IEmergencyService emergencyService,
            EmergencyContext context)
        {
            _emergencyRequest = emergencyRequest;
            _emergencyService = emergencyService;
            _context = context;
		}


		[HttpPost("select-video")]
		public async Task<ActionResult> SelectVideo([FromBody] SelectVideoRequest dto)
		{
			if (dto == null)
				return BadRequest("Invalid request body.");

			// 1️⃣ تحقق من وجود المستخدم
			var user = await _context.users.FindAsync(dto.UserId);
			if (user == null)
				return NotFound($"User with ID {dto.UserId} not found.");

			// 2️⃣ تحقق من وجود الفيديو في جدول Videos (فقط الفيديوهات الجاهزة)
			var video = await _context.videos.FindAsync(dto.VideoId);
			if (video == null)
				return NotFound($"Video with ID {dto.VideoId} not found.");

			// 3️⃣ تحقق من الخدمة
			var service = await _context.emergencyServices.FindAsync(video.EmergencyServiceId);
			if (service == null)
				return NotFound($"Emergency service with ID {video.EmergencyServiceId} not found.");

			// 4️⃣ جهز الـ EmergencyRequest
			var emergencyRequest = new EmergencyRequest
			{
				UserId = user.UserId,
				UserName = user.USerName,
				VideoDescription = video.Description, // ✅ from ready video
				Latitude = dto.Latitude,
				Longitude = dto.Longitude,
				ServiceId = video.EmergencyServiceId,
				ServiceName = service.ServiceName,
				TimeStamp = DateTime.UtcNow
			};

			_context.emergencyRequests.Add(emergencyRequest);
			await _context.SaveChangesAsync();

			return Ok(new { message = "Emergency request sent successfully." });
		}
	}
} 
