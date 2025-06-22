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

		//[HttpGet("select-video/{userId}/{videoId}")]
		//public async Task<ActionResult<EmergencyRequestDTO>> SelectVideo(int userId, int videoId, [FromQuery] double latitude, [FromQuery] double longitude)
		//{
		//	#region Before
		//	//bool isSent = await _emergencyRequest.SendEmergencyRequest(userId, videoId, latitude, longitude);

		//	//if (!isSent)
		//	//	return StatusCode(500, "Failed to send emergency request.");

		//	//// Get user data
		//	//var user = await _emergencyRequest.GetUserByIdAsync(userId);
		//	//if (user == null)
		//	//	return NotFound($"User with ID {userId} not found.");

		//	//// Initialize variables outside the if block
		//	//Video video = null;
		//	//UserUploadVideo userUploadedVideo = null;

		//	//// Get video data
		//	//video = await _emergencyRequest.GetVideoByIdAsync(videoId);

		//	//if (video == null)
		//	//{
		//	//	userUploadedVideo = await _emergencyRequest.GetUserUploadedVideoByIdAsync(videoId);
		//	//	if (userUploadedVideo == null || userUploadedVideo.User == null)
		//	//		return NotFound($"Video with ID {videoId} not found.");

		//	//	// Use the description from UserUploadedVideo
		//	//	video = new Video
		//	//	{
		//	//		Description = userUploadedVideo.Description,
		//	//		EmergencyServiceId = userUploadedVideo.EmergencyServiceId
		//	//	};
		//	//}

		//	//if (video == null)
		//	//	return NotFound($"Video with ID {videoId} not found.");

		//	//// Translate the video to text using the AI model
		//	//string translatedDescription = video.Description; // Default description

		//	//if (userUploadedVideo != null && !string.IsNullOrEmpty(userUploadedVideo.VideoUrl))
		//	//{
		//	//	translatedDescription = await _aIModelServices.TranslateVideoToTextAsync(userUploadedVideo.VideoUrl);
		//	//}

		//	//// Ensure the translated description is not empty
		//	//if (string.IsNullOrEmpty(translatedDescription))
		//	//	translatedDescription = video.Description; // Fallback to stored description

		//	//// Get the service name
		//	//var service = await _context.emergencyServices.FindAsync(video.EmergencyServiceId);
		//	//if (service == null)
		//	//	return NotFound($"Emergency service not found for video ID {videoId}.");

		//	//// Create the emergency request DTO
		//	//var emergencyRequestDTO = new EmergencyRequestDTO
		//	//{
		//	//	UserId = user.UserId,
		//	//	UserName = user.USerName,
		//	//	VideoDescription = translatedDescription, // Use the translated description
		//	//	Latitude = latitude,
		//	//	Longitude = longitude,
		//	//	TimeStamp = DateTime.Now
		//	//};

		//	//// Notify the emergency service by adding the request to their list
		//	//await _emergencyService.AddEmergencyRequestAsync(service.ServiceName, emergencyRequestDTO);

		//	//return Ok(new { Message = "Emergency request sent successfully." }); 
		//	#endregion

		//	#region Before Edit
		//	//try
		//	//{
		//	//	// Get user data
		//	//	var user = await _emergencyRequest.GetUserByIdAsync(userId);
		//	//	if (user == null)
		//	//		return NotFound($"User with ID {userId} not found.");

		//	//	// Initialize variables outside the if block
		//	//	Video video = null;
		//	//	UserUploadVideo userUploadedVideo = null;

		//	//	// Get video data
		//	//	video = await _emergencyRequest.GetVideoByIdAsync(videoId);

		//	//	if (video == null)
		//	//	{
		//	//		userUploadedVideo = await _emergencyRequest.GetUserUploadedVideoByIdAsync(videoId);
		//	//		if (userUploadedVideo == null || userUploadedVideo.User == null)
		//	//			return NotFound($"Video with ID {videoId} not found.");

		//	//		// Use the description from UserUploadedVideo
		//	//		video = new Video
		//	//		{
		//	//			Description = userUploadedVideo.Description,
		//	//			EmergencyServiceId = userUploadedVideo.EmergencyServiceId
		//	//		};
		//	//	}

		//	//	if (video == null)
		//	//		return NotFound($"Video with ID {videoId} not found.");

		//	//	// Create the emergency request DTO
		//	//	var emergencyRequest = new EmergencyRequestDTO
		//	//	{
		//	//		UserId = user.UserId,
		//	//		UserName = user.USerName,
		//	//		VideoDescription = video.Description, // Use the video's description
		//	//		Latitude = latitude,
		//	//		Longitude = longitude,
		//	//		TimeStamp = DateTime.Now
		//	//	};

		//	//	// Notify the emergency service by sending the request
		//	//	bool isSent = await _emergencyRequest.SendEmergencyRequest(userId, videoId, latitude, longitude);
		//	//	if (!isSent)
		//	//		return StatusCode(500, "Failed to send emergency request.");

		//	//	return Ok(new { Message = "Emergency request sent successfully.", RequestDetails = emergencyRequest });
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	return StatusCode(500, $"Internal server error: {ex.Message}");
		//	//} 
		//	#endregion

		//	try
		//	{
		//		// Get user data
		//		var user = await _emergencyRequest.GetUserByIdAsync(userId);
		//		if (user == null)
		//			return NotFound(new { Message = $"User with ID {userId} not found." });

		//		// Initialize variables outside the if block
		//		Video video = null;
		//		UserUploadVideo userUploadedVideo = null;

		//		// Get video data
		//		video = await _emergencyRequest.GetVideoByIdAsync(videoId);

		//		if (video == null)
		//		{
		//			userUploadedVideo = await _emergencyRequest.GetUserUploadedVideoByIdAsync(videoId);
		//			if (userUploadedVideo == null || userUploadedVideo.User == null)
		//				return NotFound(new { Message = $"Video with ID {videoId} not found." });

		//			// Use the description from UserUploadedVideo
		//			video = new Video
		//			{
		//				Description = userUploadedVideo.Description,
		//				EmergencyServiceId = userUploadedVideo.EmergencyServiceId,
		//				UploadTime = userUploadedVideo.UploadTime
		//			};
		//		}

		//		if (video == null)
		//			return NotFound(new { Message = $"Video with ID {videoId} not found." });

		//		// Create the emergency request DTO
		//		var emergencyRequest = new EmergencyRequestDTO
		//		{
		//			UserId = user.UserId,
		//			UserName = user.USerName,
		//			VideoDescription = video.Description, // Use the video's description
		//			Latitude = latitude,
		//			Longitude = longitude,
		//			TimeStamp = video.UploadTime
		//		};

		//		// Notify the emergency service by sending the request
		//		bool isSent = await _emergencyRequest.SendEmergencyRequest(userId, videoId, latitude, longitude);
		//		if (!isSent)
		//			return StatusCode(500, new { Message = "Failed to send emergency request." });

		//		// Return a successful response with details
		//		return Ok(emergencyRequest);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
		//	}
		//}


		#region Correct
		//[HttpPost("select-video")]
		//public async Task<ActionResult<EmergencyRequestDTO>> SelectVideoForEmergencyRequest(
		//[FromBody] SelectVideoRequest model)
		//{
		//	try
		//	{
		//		// Validate input
		//		if (model == null || model.VideoId <= 0 || model.UserId <= 0)
		//			return BadRequest("Invalid input data.");

		//		// Get user data
		//		var user = await _emergencyRequest.GetUserByIdAsync(model.UserId);
		//		if (user == null)
		//			return NotFound(new { Message = $"User with ID {model.UserId} not found." });

		//		// Get video data
		//		var video = await _emergencyRequest.GetVideoByIdAsync(model.VideoId);

		//		if (video == null)
		//		{
		//			var userUploadedVideo = await _emergencyRequest.GetUserUploadedVideoByIdAsync(model.VideoId);
		//			if (userUploadedVideo == null || userUploadedVideo.User == null)
		//				return NotFound(new { Message = $"Video with ID {model.VideoId} not found." });

		//			// Use the description and upload time from UserUploadedVideo
		//			video = new Video
		//			{
		//				Description = userUploadedVideo.Description,
		//				EmergencyServiceId = userUploadedVideo.EmergencyServiceId,
		//				UploadTime = userUploadedVideo.UploadTime // Use the stored upload time
		//			};
		//		}

		//		if (video == null)
		//			return NotFound(new { Message = $"Video with ID {model.VideoId} not found." });

		//		// Create the emergency request DTO
		//		var emergencyRequest = new EmergencyRequestDTO
		//		{
		//			UserId = user.UserId,
		//			UserName = user.USerName,
		//			VideoDescription = video.Description, // Use the video's description
		//			Latitude = model.Latitude,
		//			Longitude = model.Longitude,
		//			TimeStamp = video.UploadTime // Use the stored upload time
		//		};

		//		// Notify the emergency service by sending the request
		//		bool isSent = await _emergencyRequest.SendEmergencyRequest(
		//			model.UserId,
		//			model.VideoId,
		//			model.Latitude,
		//			model.Longitude);

		//		if (!isSent)
		//			return StatusCode(500, new { Message = "Failed to send emergency request." });

		//		// Return a successful response with details
		//		return Ok(emergencyRequest);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
		//	}
		//}
		#endregion

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



		#region MyRegion
		//[HttpPost("select-video")]
		//public async Task<ActionResult<EmergencyRequestDTO>> SelectVideoForEmergencyRequest([FromBody] SelectVideoRequest model)
		//{
		//	try
		//	{
		//		// Validate input
		//		if (model == null || model.VideoId <= 0 || model.UserId <= 0)
		//			return BadRequest("Invalid input data.");

		//		// Get user data
		//		var user = await _emergencyRequest.GetUserByIdAsync(model.UserId);
		//		if (user == null)
		//			return NotFound(new { Message = $"User with ID {model.UserId} not found." });

		//		// Get video data
		//		var video = await _emergencyRequest.GetVideoByIdAsync(model.VideoId);

		//		if (video == null)
		//		{
		//			var userUploadedVideo = await _emergencyRequest.GetUserUploadedVideoByIdAsync(model.VideoId);
		//			if (userUploadedVideo == null || userUploadedVideo.User == null)
		//				return NotFound(new { Message = $"Video with ID {model.VideoId} not found." });

		//			// Use the description and upload time from UserUploadVideo
		//			video = new Video
		//			{
		//				Description = userUploadedVideo.Description,
		//				EmergencyServiceId = userUploadedVideo.EmergencyServiceId,
		//				UploadTime = userUploadedVideo.UploadTime
		//			};
		//		}

		//		// Create the emergency request DTO
		//		var emergencyRequest = new EmergencyRequestDTO
		//		{
		//			UserId = user.UserId,
		//			UserName = user.USerName,
		//			VideoDescription = video.Description,
		//			Latitude = model.Latitude,
		//			Longitude = model.Longitude,
		//			TimeStamp = video.UploadTime,
		//			ServiceId = video.EmergencyServiceId
		//		};

		//		// ✅ ابعته كـ DTO جاهز
		//		bool isSent = await _emergencyRequest.SendEmergencyRequest(emergencyRequest);

		//		if (!isSent)
		//			return StatusCode(500, new { Message = "Failed to send emergency request." });

		//		return Ok(emergencyRequest);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
		//	}
		//} 
		#endregion

	}
} 
