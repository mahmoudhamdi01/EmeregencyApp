using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Repositor.Data;

namespace Project.API.Services
{
    public class EmergencyService : IEmergencyService
    {
        private readonly EmergencyContext _dbContext;

        public EmergencyService(EmergencyContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task AddEmergencyRequestAsync(string serviceName, EmergencyRequestDTO request)
        {
            var service = await GetServiceByNameAsync(serviceName);
            if (service == null)
                return;

            var existingRequests = await GetRequestsForService(serviceName);

            // Check if the request already exists
            if (!existingRequests.Any(r => r.UserId == request.UserId && r.VideoDescription == request.VideoDescription))
            {
                existingRequests.Add(request); // Add the new request

                // Save changes to the database (if needed)
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<EmergencyRequestDTO>> GetRequestsForService(string serviceName)
        {
            var service = await GetServiceByNameAsync(serviceName);
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
                    TimeStamp = DateTime.Now
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
                    TimeStamp = DateTime.Now
                })
                .ToListAsync();

            // Combine both lists
            var requests = preloadedVideos.Concat(userUploadedVideos).ToList();
            return requests;
        }

        public async Task<EmergencyServices> GetServiceByNameAsync(string serviceName)
        {
            return await _dbContext.emergencyServices.FirstOrDefaultAsync(s => s.ServiceName.ToLower() == serviceName.ToLower());
        }
    }
}
