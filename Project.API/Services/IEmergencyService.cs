using Project.API.DTOs;
using Project.Core.Entities;

namespace Project.API.Services
{
    public interface IEmergencyService
    {
        Task<List<EmergencyRequestDTO>> GetRequestsForService(string serviceName);

        // after-update
        Task AddEmergencyRequestAsync(string serviceName, EmergencyRequestDTO request);
        Task<Project.Core.Entities.EmergencyServices> GetServiceByNameAsync(string serviceName);
    }
}
