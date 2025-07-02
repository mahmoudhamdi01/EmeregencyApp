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
