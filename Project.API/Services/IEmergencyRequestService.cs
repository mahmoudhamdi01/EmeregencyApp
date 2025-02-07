namespace Project.API.Services
{
    public interface IEmergencyRequestService
    {
        Task<bool> SendEmergencyRequest(int userId, int videoId, double latitude, double longitude);
    }
}
