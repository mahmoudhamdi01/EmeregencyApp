using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Repositor.Data;
using System.Text;

namespace Project.API.Services
{
    public class EmergencyService : IEmergencyService
    {
        private readonly EmergencyContext _dbContext;

        public EmergencyService(EmergencyContext DbContext)
        {
            _dbContext = DbContext;
        }

		public async Task<List<EmergencyRequestDTO>> GetRequestsForService(string serviceName)
		{
			var service = await _dbContext.emergencyServices.FirstOrDefaultAsync(s => s.ServiceName.ToLower() == serviceName.ToLower());

			if (service == null)
				return new List<EmergencyRequestDTO>();

			// Load preloaded videos
			var preloadedVideos = await _dbContext.videos
				.Include(v => v.User)
				.Where(v => v.EmergencyServiceId == service.ServiceId && v.User != null)
				.Select(video => new EmergencyRequestDTO
				{
					UserId = video.User.UserId,
					UserName = video.User.USerName,
					VideoDescription = video.Description,
					Latitude = 0, // Use default or provided location
					Longitude = 0, // Use default or provided location
				})
				.ToListAsync();

			// Load user-uploaded videos
			var userUploadedVideos = await _dbContext.uploadVideos
				.Include(uv => uv.User)
				.Where(uv => uv.EmergencyServiceId == service.ServiceId && uv.User != null)
				.Select(video => new EmergencyRequestDTO
				{
					UserId = video.User.UserId,
					UserName = video.User.USerName,
					VideoDescription = video.Description,
					Latitude = 0, // Use default or provided location
					Longitude = 0, // Use default or provided location
				})
				.ToListAsync();

			// Combine both lists
			var requests = preloadedVideos.Concat(userUploadedVideos).ToList();
			return requests;
		}

		//public async Task AddEmergencyRequestAsync(string serviceName, EmergencyRequestDTO request)
		//{
		//	var service = await _dbContext.emergencyServices.FirstOrDefaultAsync(s => s.ServiceName.ToLower() == serviceName.ToLower());
		//	if (service == null)
		//		return;

		//	Console.WriteLine($"Adding emergency request for service {serviceName}:");
		//	Console.WriteLine($" - User: {request.UserName}");
		//	Console.WriteLine($" - Description: {request.VideoDescription}");
		//	Console.WriteLine($" - Location: ({request.Latitude}, {request.Longitude})");
		//	Console.WriteLine($" - Timestamp: {request.TimeStamp}");
		//}

		#region Error
		//public async Task AddEmergencyRequestAsync(string serviceName, EmergencyRequestDTO request)
		//{
		//	try
		//	{
		//		var service = await _dbContext.emergencyServices.FirstOrDefaultAsync(s => s.ServiceName.ToLower() == serviceName.ToLower());
		//		if (service == null)
		//		{
		//			Console.WriteLine($"Service '{serviceName}' not found.");
		//			return;
		//		}

		//		// Log the request details
		//		Console.WriteLine($"Adding emergency request for service {serviceName}:");
		//		Console.WriteLine($" - User: {request.UserName}");
		//		Console.WriteLine($" - Description: {request.VideoDescription}");
		//		Console.WriteLine($" - Location: ({request.Latitude}, {request.Longitude})");
		//		Console.WriteLine($" - Timestamp: {request.TimeStamp}");

		//		// Send the request to the relevant service
		//		bool isSent = await SendRequestToExternalService(service.ServiceName, request);

		//		if (!isSent)
		//		{
		//			Console.WriteLine($"Failed to send emergency request to service '{serviceName}'.");
		//		}
		//		else
		//		{
		//			Console.WriteLine($"Request successfully sent to service '{serviceName}'.");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error adding emergency request: {ex.Message}");
		//	}
		//}

		//private async Task<bool> SendRequestToExternalService(string serviceName, EmergencyRequestDTO request)
		//{
		//	try
		//	{
		//		// Example: Send the request to an external API endpoint
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

		public async Task AddEmergencyRequestAsync(string serviceName, EmergencyRequestDTO request)
		{
			var service = await _dbContext.emergencyServices.FirstOrDefaultAsync(s => s.ServiceName.ToLower() == serviceName.ToLower());
			if (service == null)
				return;

			Console.WriteLine($"Adding emergency request for service {serviceName}:");
			Console.WriteLine($" - User: {request.UserName}");
			Console.WriteLine($" - Description: {request.VideoDescription}");
			Console.WriteLine($" - Location: ({request.Latitude}, {request.Longitude})");
			Console.WriteLine($" - Timestamp: {request.TimeStamp}");
		}
	}
}
