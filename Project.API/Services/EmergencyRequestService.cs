
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
	}
}
