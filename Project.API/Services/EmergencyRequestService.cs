
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Project.API.DTOs;
using Project.API.Helpers;
using Project.Core.Entities;
using Project.Repositor.Data;
using System.Text;

namespace Project.API.Services
{
    public class EmergencyRequestService : IEmergencyRequestService
    {
        private readonly EmergencyContext _dbContext;
		

		public EmergencyRequestService(EmergencyContext DbContext)
        {
            _dbContext = DbContext;
			
		}

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserUploadVideo> GetUserUploadedVideoByIdAsync(int videoId)
        {
            return await _dbContext.uploadVideos
                .Include(uv => uv.User)
                .Include(uv => uv.EmergencyService) // Directly include EmergencyService instead of Video
                .FirstOrDefaultAsync(uv => uv.UploadVideoId == videoId);
        }

        public async Task<Video> GetVideoByIdAsync(int videoId)
        {
            return await _dbContext.videos
                .Include(v => v.EmergencyService)
                .FirstOrDefaultAsync(v => v.VideoId == videoId);
        }

		#region Old-Code
		//      public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
		//      {
		//	#region MyRegion
		//	//var user = await _dbContext.users.FindAsync(userId);

		//	//// Check if the video is a preloaded video
		//	//var video = await _dbContext.videos.FindAsync(videoId);

		//	//if (video == null)
		//	//{
		//	//    // If not found in Videos, check UserUploadedVideos
		//	//    var userUploadedVideo = await _dbContext.uploadVideos
		//	//        .Include(uv => uv.User) // Load the related User
		//	//        .FirstOrDefaultAsync(uv => uv.UploadVideoId == videoId);

		//	//    if (userUploadedVideo == null || userUploadedVideo.User == null)
		//	//        return false;

		//	//    // Use UserUploadedVideo data instead of Video
		//	//    var emergencyRequests = new EmergencyRequestDTO
		//	//    {
		//	//        UserId = userUploadedVideo.User.UserId,
		//	//        UserName = userUploadedVideo.User.USerName,
		//	//        VideoDescription = userUploadedVideo.Description, // Use UserUploadedVideo.Description
		//	//        Latitude = latitude,
		//	//        Longitude = longitude
		//	//    };

		//	//    // Simulate sending data to authorities
		//	//    Console.WriteLine($"Sending emergency request for user {emergencyRequests.UserName} at location ({emergencyRequests.Latitude}, {emergencyRequests.Longitude}) with description: {emergencyRequests.VideoDescription}");

		//	//    return true;
		//	//}

		//	//// If the video is preloaded, use its data
		//	//if (user == null || video == null)
		//	//    return false;

		//	//var emergencyRequest = new EmergencyRequestDTO
		//	//{
		//	//    UserId = user.UserId,
		//	//    UserName = user.USerName,
		//	//    VideoDescription = video.Description, // Use Video.Description
		//	//    Latitude = latitude,
		//	//    Longitude = longitude
		//	//};

		//	//// Simulate sending data to authorities
		//	//Console.WriteLine($"Sending emergency request for user {emergencyRequest.UserName} at location ({emergencyRequest.Latitude}, {emergencyRequest.Longitude}) with description: {emergencyRequest.VideoDescription}");

		//	//return true; 
		//	#endregion

		//	#region End 
		//	//Console.WriteLine($"Sending emergency request for user {userId} at location ({latitude}, {longitude}) for video ID {videoId}");
		//	//return true;
		//	#endregion

		//	//var user = await GetUserByIdAsync(userId);

		//	//if (user == null)
		//	//{
		//	//	Console.WriteLine($"User with ID {userId} not found.");
		//	//	return false;
		//	//}

		//	//// Get video data
		//	//var video = await GetVideoByIdAsync(videoId);

		//	//if (video == null)
		//	//{
		//	//	var userUploadedVideo = await GetUserUploadedVideoByIdAsync(videoId);

		//	//	if (userUploadedVideo == null || userUploadedVideo.User == null)
		//	//	{
		//	//		Console.WriteLine($"Video with ID {videoId} not found.");
		//	//		return false;
		//	//	}

		//	//	video = new Video
		//	//	{
		//	//		Description = userUploadedVideo.Description,
		//	//		EmergencyServiceId = userUploadedVideo.EmergencyServiceId
		//	//	};
		//	//}

		//	//if (video == null)
		//	//{
		//	//	Console.WriteLine("Failed to retrieve video data.");
		//	//	return false;
		//	//}

		//	//// Create the emergency request DTO
		//	//var emergencyRequest = new EmergencyRequestDTO
		//	//{
		//	//	UserId = user.UserId,
		//	//	UserName = user.USerName,
		//	//	VideoDescription = video.Description,
		//	//	Latitude = latitude,
		//	//	Longitude = longitude
		//	//};

		//	//Console.WriteLine($"Sending emergency request for user {emergencyRequest.UserName} at location ({emergencyRequest.Latitude}, {emergencyRequest.Longitude}) with description: {emergencyRequest.VideoDescription}");

		//	//return true;

		//	#region After-Update
		//	var user = await _dbContext.users.FindAsync(userId);

		//	// Check if the video is a preloaded video
		//	var video = await _dbContext.videos.FindAsync(videoId);

		//	if (video == null)
		//	{
		//		// If not found in Videos, check UserUploadedVideos
		//		var userUploadedVideo = await _dbContext.uploadVideos
		//			.Include(uv => uv.User) // Ensure User is loaded
		//			.FirstOrDefaultAsync(uv => uv.UploadVideoId == videoId);

		//		if (userUploadedVideo == null || userUploadedVideo.User == null)
		//			return false;

		//		// Use UserUploadedVideo data
		//		var emergencyRequests = new EmergencyRequestDTO
		//		{
		//			UserId = userUploadedVideo.User.UserId,
		//			UserName = userUploadedVideo.User.USerName,
		//			VideoDescription = userUploadedVideo.Description, // Use UserUploadedVideo's description
		//			Latitude = latitude, // Use provided location
		//			Longitude = longitude // Use provided location
		//		};

		//		// Simulate sending data to authorities
		//		Console.WriteLine($"Sending emergency request for user {emergencyRequests.UserName} at location ({emergencyRequests.Latitude}, {emergencyRequests.Longitude}) with description: {emergencyRequests.VideoDescription}");

		//		return true;
		//	}

		//	// If the video is preloaded, use its data
		//	if (user == null || video == null)
		//		return false;

		//	var emergencyRequest = new EmergencyRequestDTO
		//	{
		//		UserId = user.UserId,
		//		UserName = user.USerName,
		//		VideoDescription = video.Description, // Use Video's description
		//		Latitude = latitude, // Use provided location
		//		Longitude = longitude // Use provided location
		//	};

		//	// Simulate sending data to authorities
		//	Console.WriteLine($"Sending emergency request for user {emergencyRequest.UserName} at location ({emergencyRequest.Latitude}, {emergencyRequest.Longitude}) with description: {emergencyRequest.VideoDescription}");

		//	return true;
		//	#endregion
		//} 
		#endregion


		#region MyRegion
		//public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
		//{
		//	try
		//	{
		//		// Retrieve user
		//		var user = await _dbContext.users.FindAsync(userId);
		//		if (user == null)
		//		{
		//			Console.WriteLine($"User with ID {userId} not found.");
		//			return false;
		//		}

		//		// Try to get from UserUploadVideos first
		//		var uploadVideo = await _dbContext.uploadVideos.FindAsync(videoId);
		//		int? serviceId = null;
		//		string description = null;
		//		DateTime? uploadTime = null;

		//		if (uploadVideo != null)
		//		{
		//			serviceId = uploadVideo.EmergencyServiceId;
		//			description = uploadVideo.Description;
		//			uploadTime = uploadVideo.UploadTime;
		//		}
		//		else
		//		{
		//			// If not found in uploadVideos, try in static videos
		//			var staticVideo = await _dbContext.videos.FindAsync(videoId);
		//			if (staticVideo == null)
		//			{
		//				Console.WriteLine($"Video with ID {videoId} not found in both tables.");
		//				return false;
		//			}
		//			serviceId = staticVideo.EmergencyServiceId;
		//			description = staticVideo.Description;
		//			uploadTime = staticVideo.UploadTime;
		//		}

		//		if (serviceId == null)
		//		{
		//			Console.WriteLine($"Service ID not found for video {videoId}.");
		//			return false;
		//		}

		//		// Determine the service name
		//		var service = await _dbContext.emergencyServices.FindAsync(serviceId);
		//		if (service == null)
		//		{
		//			Console.WriteLine($"Emergency service with ID {serviceId} not found.");
		//			return false;
		//		}

		//		// Prepare the request DTO
		//		var emergencyRequest = new EmergencyRequestDTO
		//		{
		//			UserId = user.UserId,
		//			UserName = user.USerName,
		//			VideoDescription = description,
		//			Latitude = latitude,
		//			Longitude = longitude,
		//			TimeStamp = uploadTime ?? DateTime.UtcNow,
		//			ServiceId = service.ServiceId
		//		};

		//		// Send to external service
		//		bool isSent = await SendRequestToExternalService(service.ServiceName, emergencyRequest);

		//		return isSent;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error sending emergency request: {ex.Message}");
		//		return false;
		//	}
		//}



		//private async Task<bool> SendRequestToExternalService(string serviceName, EmergencyRequestDTO request)
		//{
		//	try
		//	{
		//		using (var client = new HttpClient())
		//		{
		//			var json = JsonConvert.SerializeObject(request);
		//			var content = new StringContent(json, Encoding.UTF8, "application/json");

		//			var response = await client.PostAsync($"https://api.example.com/emergency/{serviceName}", content);

		//			if (response.IsSuccessStatusCode)
		//			{
		//				Console.WriteLine($"Request successfully sent to {serviceName}.");
		//				return true;
		//			}
		//			else
		//			{
		//				Console.WriteLine($"Failed to send request to {serviceName}. Status code: {response.StatusCode}");
		//				return false;
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error sending request to service: {ex.Message}");
		//		return false;
		//	}
		//} 
		#endregion

		//#region قبل اخر تعديل
		//public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
		//{
		//	//try
		//	//{
		//	//	// Retrieve user
		//	//	var user = await _dbContext.users.FindAsync(userId);
		//	//	if (user == null)
		//	//	{
		//	//		Console.WriteLine($"User with ID {userId} not found.");
		//	//		return false;
		//	//	}

		//	//	// Try to get from UserUploadVideos first
		//	//	var uploadVideo = await _dbContext.uploadVideos.FindAsync(videoId);
		//	//	int? serviceId = null;
		//	//	string description = null;
		//	//	DateTime? uploadTime = null;

		//	//	if (uploadVideo != null)
		//	//	{
		//	//		serviceId = uploadVideo.EmergencyServiceId;
		//	//		description = uploadVideo.Description;
		//	//		uploadTime = uploadVideo.UploadTime;
		//	//	}
		//	//	else
		//	//	{
		//	//		var staticVideo = await _dbContext.videos.FindAsync(videoId);
		//	//		if (staticVideo == null)
		//	//		{
		//	//			Console.WriteLine($"Video with ID {videoId} not found in both tables.");
		//	//			return false;
		//	//		}
		//	//		serviceId = staticVideo.EmergencyServiceId;
		//	//		description = staticVideo.Description;
		//	//		uploadTime = staticVideo.UploadTime;
		//	//	}

		//	//	if (serviceId == null)
		//	//	{
		//	//		Console.WriteLine($"Service ID not found for video {videoId}.");
		//	//		return false;
		//	//	}

		//	//	var service = await _dbContext.emergencyServices.FindAsync(serviceId);
		//	//	if (service == null)
		//	//	{
		//	//		Console.WriteLine($"Emergency service with ID {serviceId} not found.");
		//	//		return false;
		//	//	}

		//	//	// Prepare DTO then call main version
		//	//	var request = new EmergencyRequestDTO
		//	//	{
		//	//		UserId = user.UserId,
		//	//		UserName = user.USerName,
		//	//		VideoDescription = description,
		//	//		Latitude = latitude,
		//	//		Longitude = longitude,
		//	//		TimeStamp = uploadTime ?? DateTime.UtcNow,
		//	//		ServiceId = service.ServiceId
		//	//	};

		//	//	return await SendEmergencyRequest(request);
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	Console.WriteLine($"Error sending emergency request: {ex.Message}");
		//	//	return false;
		//	//}

		//	var user = await _dbContext.users.FindAsync(userId);

		//	// Check if the video is a preloaded video
		//	var video = await _dbContext.videos.FindAsync(videoId);

		//	if (video == null)
		//	{
		//		// If not found in Videos, check UserUploadedVideos
		//		var userUploadedVideo = await _dbContext.uploadVideos
		//			.Include(uv => uv.User) // Ensure User is loaded
		//			.FirstOrDefaultAsync(uv => uv.UploadVideoId == videoId);

		//		if (userUploadedVideo == null || userUploadedVideo.User == null)
		//			return false;

		//		// Use UserUploadedVideo data
		//		var emergencyRequests = new EmergencyRequestDTO
		//		{
		//			UserId = userUploadedVideo.User.UserId,
		//			UserName = userUploadedVideo.User.USerName,
		//			VideoDescription = userUploadedVideo.Description, // Use UserUploadedVideo's description
		//			Latitude = latitude, // Use provided location
		//			Longitude = longitude // Use provided location
		//		};

		//		// Simulate sending data to authorities
		//		Console.WriteLine($"Sending emergency request for user {emergencyRequests.UserName} at location ({emergencyRequests.Latitude}, {emergencyRequests.Longitude}) with description: {emergencyRequests.VideoDescription}");

		//		return true;
		//	}

		//	// If the video is preloaded, use its data
		//	if (user == null || video == null)
		//		return false;

		//	var emergencyRequest = new EmergencyRequestDTO
		//	{
		//		UserId = user.UserId,
		//		UserName = user.USerName,
		//		VideoDescription = video.Description, // Use Video's description
		//		Latitude = latitude, // Use provided location
		//		Longitude = longitude // Use provided location
		//	};

		//	// Simulate sending data to authorities
		//	Console.WriteLine($"Sending emergency request for user {emergencyRequest.UserName} at location ({emergencyRequest.Latitude}, {emergencyRequest.Longitude}) with description: {emergencyRequest.VideoDescription}");

		//	return true;

		//}
		//#endregion


		//public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
		//{
		//	try
		//	{
		//		var user = await _dbContext.users.FindAsync(userId);
		//		if (user == null)
		//		{
		//			Console.WriteLine($"User with ID {userId} not found.");
		//			return false;
		//		}

		//		// Try UserUploadedVideo first
		//		var userUploadedVideo = await _dbContext.uploadVideos.FindAsync(videoId);
		//		int? serviceId = null;
		//		string description = null;
		//		DateTime? uploadTime = null;

		//		if (userUploadedVideo != null)
		//		{
		//			serviceId = userUploadedVideo.EmergencyServiceId;
		//			description = userUploadedVideo.Description;
		//			uploadTime = userUploadedVideo.UploadTime;
		//		}
		//		else
		//		{
		//			var video = await _dbContext.videos.FindAsync(videoId);
		//			if (video == null)
		//			{
		//				Console.WriteLine($"Video with ID {videoId} not found in both tables.");
		//				return false;
		//			}
		//			serviceId = video.EmergencyServiceId;
		//			description = video.Description;
		//			uploadTime = video.UploadTime;
		//		}

		//		if (serviceId == null)
		//		{
		//			Console.WriteLine($"Service ID not found for video {videoId}.");
		//			return false;
		//		}

		//		// Get the actual service name
		//		var service = await _dbContext.emergencyServices.FindAsync(serviceId);
		//		if (service == null)
		//		{
		//			Console.WriteLine($"Emergency service with ID {serviceId} not found.");
		//			return false;
		//		}

		//		// Build request DTO
		//		var emergencyRequest = new EmergencyRequestDTO
		//		{
		//			UserId = user.UserId,
		//			UserName = user.USerName,
		//			VideoDescription = description,
		//			Latitude = latitude,
		//			Longitude = longitude,
		//			ServiceId = service.ServiceId,
		//			TimeStamp = uploadTime ?? DateTime.UtcNow
		//		};

		//		// ✅ Here: use the actual HTTP POST
		//		return await SendRequestToExternalService(service.ServiceName, emergencyRequest);
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error sending emergency request: {ex.Message}");
		//		return false;
		//	}
		//}


		#region Correct
		public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
		{
			try
			{
				var user = await _dbContext.users.FindAsync(userId);
				if (user == null) return false;

				var userUploadedVideo = await _dbContext.uploadVideos.FindAsync(videoId);
				int? serviceId = null;
				string description = null;

				if (userUploadedVideo != null)
				{
					serviceId = userUploadedVideo.EmergencyServiceId;
					description = userUploadedVideo.Description;
				}
				else
				{
					var video = await _dbContext.videos.FindAsync(videoId);
					if (video == null) return false;

					serviceId = video.EmergencyServiceId;
					description = video.Description;
				}

				var service = await _dbContext.emergencyServices.FindAsync(serviceId);
				if (service == null) return false;

				var newRequest = new EmergencyRequest
				{
					UserId = user.UserId,
					UserName = user.USerName,
					VideoDescription = description,
					Latitude = latitude,
					Longitude = longitude,
					ServiceId = service.ServiceId,
					ServiceName = service.ServiceName,
					TimeStamp = DateTime.UtcNow
				};

				_dbContext.emergencyRequests.Add(newRequest);
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return false;
			}
		}
		#endregion




		//public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
		//{
		//	try
		//	{
		//		// 1️⃣ تأكد من وجود المستخدم
		//		var user = await _dbContext.users.FindAsync(userId);
		//		if (user == null)
		//		{
		//			Console.WriteLine($"❌ User with ID {userId} not found.");
		//			return false;
		//		}

		//		// 2️⃣ تأكد من وجود الفيديو واحصل على الوصف حسب نوعه
		//		string description = null;
		//		int? serviceId = null;
		//		DateTime? uploadTime = null;

		//		// تحقق أولاً في جدول UserUploadedVideos (يعني فيديو مستخدم)
		//		var userUploadedVideo = await _dbContext.uploadVideos.FindAsync(videoId);
		//		if (userUploadedVideo != null)
		//		{
		//			description = userUploadedVideo.Description; // وصف AI
		//			serviceId = userUploadedVideo.EmergencyServiceId;
		//			uploadTime = userUploadedVideo.UploadTime;
		//		}
		//		else
		//		{
		//			// لو مش موجود في الجدول الأول → ابحث في الجدول الجاهز
		//			var video = await _dbContext.videos.FindAsync(videoId);
		//			if (video != null)
		//			{
		//				description = video.Description; // وصف جاهز مكتوب
		//				serviceId = video.EmergencyServiceId;
		//				uploadTime = video.UploadTime;
		//			}
		//			else
		//			{
		//				Console.WriteLine($"❌ Video with ID {videoId} not found in both tables.");
		//				return false;
		//			}
		//		}

		//		// 3️⃣ تحقق من الـ Service
		//		if (serviceId == null)
		//		{
		//			Console.WriteLine($"❌ Service ID not found for video {videoId}.");
		//			return false;
		//		}

		//		var service = await _dbContext.emergencyServices.FindAsync(serviceId);
		//		if (service == null)
		//		{
		//			Console.WriteLine($"❌ Emergency service with ID {serviceId} not found.");
		//			return false;
		//		}

		//		// 4️⃣ أنشئ EmergencyRequest جديد وخزنه في الداتابيز
		//		var emergencyRequest = new EmergencyRequest
		//		{
		//			UserId = user.UserId,
		//			UserName = user.USerName,
		//			VideoDescription = description,
		//			Latitude = latitude,
		//			Longitude = longitude,
		//			ServiceId = service.ServiceId,
		//			ServiceName = service.ServiceName,
		//			TimeStamp = uploadTime ?? DateTime.UtcNow
		//		};

		//		_dbContext.emergencyRequests.Add(emergencyRequest);
		//		await _dbContext.SaveChangesAsync();

		//		Console.WriteLine($"✅ Emergency request saved for service: {service.ServiceName}.");
		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"❌ Error sending emergency request: {ex.Message}");
		//		return false;
		//	}
		//}




		#region Error
		//public async Task<bool> SendEmergencyRequest(EmergencyRequestDTO request)
		//{
		//	try
		//	{
		//		// Use the ServiceId from DTO
		//		var service = await _dbContext.emergencyServices.FindAsync(request.ServiceId);
		//		if (service == null)
		//		{
		//			Console.WriteLine($"Emergency service with ID {request.ServiceId} not found.");
		//			return false;
		//		}

		//		// Send to external service
		//		bool isSent = await SendRequestToExternalService(service.ServiceName, request);
		//		return isSent;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error sending emergency request: {ex.Message}");
		//		return false;
		//	}
		//}

		//private async Task<bool> SendRequestToExternalService(string serviceName, EmergencyRequestDTO request)
		//{
		//	try
		//	{
		//		using (var client = new HttpClient())
		//		{
		//			var json = JsonConvert.SerializeObject(request);
		//			var content = new StringContent(json, Encoding.UTF8, "application/json");

		//			var response = await client.PostAsync($"https://api.example.com/emergency/{serviceName}", content);

		//			if (response.IsSuccessStatusCode)
		//			{
		//				Console.WriteLine($"Request successfully sent to {serviceName}.");
		//				return true;
		//			}
		//			else
		//			{
		//				Console.WriteLine($"Failed to send request to {serviceName}. Status code: {response.StatusCode}");
		//				return false;
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error sending request to service: {ex.Message}");
		//		return false;
		//	}
		//} 
		#endregion
	}
}
