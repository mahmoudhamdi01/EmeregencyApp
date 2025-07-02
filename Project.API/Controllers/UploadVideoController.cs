using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.API.Services;
using Project.Core.Entities;
using Project.Core.Repositories;
using Project.Repositor.Data;

namespace Project.API.Controllers
{
    
    public class UploadVideoController : APIBaseController
    {
        private readonly IVideoUploadRepository _uploadRepo;
        private readonly IMapper _mapper;
        private readonly EmergencyContext _context;
        private readonly IEmergencyRequestService _emergencyRequestService;
        private readonly IConfiguration _configuration;
		private readonly IAIModelServices _aIModelServices;

		public UploadVideoController(IVideoUploadRepository UploadRepo,
            IMapper mapper, EmergencyContext context,
            IEmergencyRequestService emergencyRequestService, 
            IConfiguration configuration, IAIModelServices aIModelServices)
        {
            _uploadRepo = UploadRepo;
            _mapper = mapper;
            _context = context;
            _emergencyRequestService = emergencyRequestService;
            _configuration = configuration;
			_aIModelServices = aIModelServices;
		}

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserUploadVideo>>> GetUserUploadedVideos(int userId)
        {
            var videos = await _uploadRepo.GetUserUploadedVideosAsync(userId);
            var MappedVideos = _mapper.Map<IEnumerable<UserUploadVideo>, IEnumerable<UploadVideoDTO>>(videos);
            return Ok(MappedVideos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserUploadVideo>> GetUserUploadedVideoById(int id)
        {
            var video = await _uploadRepo.GetUploadVideoAsync(id);
            var MappedVideos = _mapper.Map<UserUploadVideo, UploadVideoDTO>(video);
            return Ok(MappedVideos);
        }

		[HttpPost]
		public async Task<ActionResult<UserUploadVideo>> UploadAndAddVideo(
	[FromForm] IFormFile videoFile,
	[FromForm] int userId,
	[FromForm] int emergencyServiceId,
	[FromForm] double latitude,
	[FromForm] double longitude)
		{
			if (videoFile == null || videoFile.Length == 0)
				return BadRequest("No video file provided.");

			if (!IsVideoFile(videoFile))
				return BadRequest("Invalid video file format.");

			var directoryPath = Path.Combine("wwwroot", "videos");
			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);

			var filePath = Path.Combine(directoryPath, videoFile.FileName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await videoFile.CopyToAsync(stream);
			}

			var videoUrl = $"{Request.Scheme}://{Request.Host}/videos/{videoFile.FileName}";

			var description = await _aIModelServices.TranslateVideoToTextAsync(filePath);
			if (string.IsNullOrEmpty(description))
				return StatusCode(500, "Failed to translate video.");

			var user = await _context.users.FindAsync(userId);
			if (user == null)
				return NotFound($"User {userId} not found.");

			var service = await _context.emergencyServices.FindAsync(emergencyServiceId);
			if (service == null)
				return NotFound($"Service {emergencyServiceId} not found.");

			// Save uploaded video info
			var uploadedVideo = new UserUploadVideo
			{
				Title = videoFile.FileName,
				Description = description,
				VideoUrl = videoUrl,
				UserId = userId,
				EmergencyServiceId = emergencyServiceId
			};
			_context.uploadVideos.Add(uploadedVideo);
			await _context.SaveChangesAsync();

			// Create emergency request using AI description
			var emergencyRequest = new EmergencyRequest
			{
				UserId = userId,
				UserName = user.USerName,
				VideoDescription = description,
				Latitude = latitude,
				Longitude = longitude,
				ServiceId = emergencyServiceId,
				ServiceName = service.ServiceName,
				TimeStamp = DateTime.UtcNow
			};
			_context.emergencyRequests.Add(emergencyRequest);
			await _context.SaveChangesAsync();

			return Ok(new { uploadedVideo, emergencyRequest });
		}

		private bool IsVideoFile(IFormFile file)
		{
			var allowed = new[] { ".mp4", ".avi", ".mov" };
			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
			return allowed.Contains(ext);
		}


		[HttpDelete]
        public async Task<ActionResult<bool>> DeleteVideo(int id)
        {
            return await _uploadRepo.DeleteVideoAsync(id);
        }

    }
}
