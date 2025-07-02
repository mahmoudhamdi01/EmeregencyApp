using Project.API.DTOs;
using Project.Core.Entities;

namespace Project.API.Services
{
    public interface IEmergencyRequestService
    {
		Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude);
		Task<User> GetUserByIdAsync(int userId);
        Task<Video> GetVideoByIdAsync(int videoId);
        Task<UserUploadVideo> GetUserUploadedVideoByIdAsync(int videoId);
    }
}
