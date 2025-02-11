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
            IEmergencyService emergencyService, EmergencyContext context)
        {
            _emergencyRequest = emergencyRequest;
            _emergencyService = emergencyService;
            _context = context;
        }

        [HttpGet("select-video/{userId}/{videoId}")]
        public async Task<IActionResult> SelectVideo(int userId, int videoId, [FromQuery] double latitude, [FromQuery] double longitude)
        {
            bool isSent = await _emergencyRequest.SendEmergencyRequest(userId, videoId, latitude, longitude);

            if (!isSent)
                return StatusCode(500, "Failed to send emergency request.");

            // Get user data
            var user = await _emergencyRequest.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound($"User with ID {userId} not found.");

            // Get video data
            var video = await _emergencyRequest.GetVideoByIdAsync(videoId);
            if (video == null)
            {
                var userUploadedVideo = await _emergencyRequest.GetUserUploadedVideoByIdAsync(videoId);
                if (userUploadedVideo == null || userUploadedVideo.User == null)
                    return NotFound($"Video with ID {videoId} not found.");

                // Use the description from UserUploadedVideo
                video = new Video
                {
                    Description = userUploadedVideo.Description,
                    EmergencyServiceId = userUploadedVideo.EmergencyServiceId
                };
            }

            if (video == null)
                return NotFound($"Video with ID {videoId} not found.");

            // Get the service name
            var service = await _context.emergencyServices.FindAsync(video.EmergencyServiceId);
            if (service == null)
                return NotFound($"Emergency service not found for video ID {videoId}.");

            // Create the emergency request DTO
            var emergencyRequest = new EmergencyRequestDTO
            {
                UserId = user.UserId,
                UserName = user.USerName,
                VideoDescription = video.Description,
                Latitude = latitude,
                Longitude = longitude
            };

            // Notify the emergency service by adding the request to their list
            await _emergencyService.AddEmergencyRequestAsync(service.ServiceName, emergencyRequest);

            return Ok(new { Message = "Emergency request sent successfully." });
        }
    }
}
