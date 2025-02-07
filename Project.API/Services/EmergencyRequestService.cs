
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.Repositor.Data;

namespace Project.API.Services
{
    public class EmergencyRequestService : IEmergencyRequestService
    {
        private readonly EmergencyContext _dbContext;

        public EmergencyRequestService(EmergencyContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude)
        {
            #region MyRegion
            //var user = await _dbContext.users.FindAsync(userId);

            //// Check if the video is a preloaded video
            //var video = await _dbContext.videos.FindAsync(videoId);

            //if (video == null)
            //{
            //    // If not found in Videos, check UserUploadedVideos
            //    var userUploadedVideo = await _dbContext.uploadVideos
            //        .Include(uv => uv.User) // Load the related User
            //        .FirstOrDefaultAsync(uv => uv.UploadVideoId == videoId);

            //    if (userUploadedVideo == null || userUploadedVideo.User == null)
            //        return false;

            //    // Use UserUploadedVideo data instead of Video
            //    var emergencyRequests = new EmergencyRequestDTO
            //    {
            //        UserId = userUploadedVideo.User.UserId,
            //        UserName = userUploadedVideo.User.USerName,
            //        VideoDescription = userUploadedVideo.Description, // Use UserUploadedVideo.Description
            //        Latitude = latitude,
            //        Longitude = longitude
            //    };

            //    // Simulate sending data to authorities
            //    Console.WriteLine($"Sending emergency request for user {emergencyRequests.UserName} at location ({emergencyRequests.Latitude}, {emergencyRequests.Longitude}) with description: {emergencyRequests.VideoDescription}");

            //    return true;
            //}

            //// If the video is preloaded, use its data
            //if (user == null || video == null)
            //    return false;

            //var emergencyRequest = new EmergencyRequestDTO
            //{
            //    UserId = user.UserId,
            //    UserName = user.USerName,
            //    VideoDescription = video.Description, // Use Video.Description
            //    Latitude = latitude,
            //    Longitude = longitude
            //};

            //// Simulate sending data to authorities
            //Console.WriteLine($"Sending emergency request for user {emergencyRequest.UserName} at location ({emergencyRequest.Latitude}, {emergencyRequest.Longitude}) with description: {emergencyRequest.VideoDescription}");

            //return true; 
            #endregion


            var user = await _dbContext.users.FindAsync(userId);

            // Check if the video is a preloaded video
            var video = await _dbContext.videos.FindAsync(videoId);

            if (video == null)
            {
                // If not found in Videos, check UserUploadedVideos
                var userUploadedVideo = await _dbContext.uploadVideos
                    .Include(uv => uv.User) // Ensure User is loaded
                    .FirstOrDefaultAsync(uv => uv.UploadVideoId == videoId);

                if (userUploadedVideo == null || userUploadedVideo.User == null)
                    return false;

                // Use UserUploadedVideo data
                var emergencyRequests = new EmergencyRequestDTO
                {
                    UserId = userUploadedVideo.User.UserId,
                    UserName = userUploadedVideo.User.USerName,
                    VideoDescription = userUploadedVideo.Description, // Use UserUploadedVideo's description
                    Latitude = latitude, // Use provided location
                    Longitude = longitude // Use provided location
                };

                // Simulate sending data to authorities
                Console.WriteLine($"Sending emergency request for user {emergencyRequests.UserName} at location ({emergencyRequests.Latitude}, {emergencyRequests.Longitude}) with description: {emergencyRequests.VideoDescription}");

                return true;
            }

            // If the video is preloaded, use its data
            if (user == null || video == null)
                return false;

            var emergencyRequest = new EmergencyRequestDTO
            {
                UserId = user.UserId,
                UserName = user.USerName,
                VideoDescription = video.Description, // Use Video's description
                Latitude = latitude, // Use provided location
                Longitude = longitude // Use provided location
            };

            // Simulate sending data to authorities
            Console.WriteLine($"Sending emergency request for user {emergencyRequest.UserName} at location ({emergencyRequest.Latitude}, {emergencyRequest.Longitude}) with description: {emergencyRequest.VideoDescription}");

            return true;
        }
    }
}
