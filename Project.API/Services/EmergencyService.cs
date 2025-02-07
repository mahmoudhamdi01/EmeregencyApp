using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
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
                    Latitude = 0, // Use default value or provided location
                    Longitude = 0 // Use default value or provided location
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
                    Latitude = 0, // Use default value or provided location
                    Longitude = 0 // Use default value or provided location
                })
                .ToListAsync();

            // Combine both lists
            var requests = preloadedVideos.Concat(userUploadedVideos).ToList();

            return requests;
        }
    }
}
