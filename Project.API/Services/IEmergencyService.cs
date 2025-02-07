using Project.API.DTOs;

namespace Project.API.Services
{
    public interface IEmergencyService
    {
        Task<List<EmergencyRequestDTO>> GetRequestsForService(string serviceName);
    }
}
