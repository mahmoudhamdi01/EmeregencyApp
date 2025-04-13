using Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories
{
    public interface IServices
    {
        Task<IEnumerable<EmergencyServices>> GetAllServicesAsync();
        Task<EmergencyServices> GetServicesByIdAsync(int id);
    }
}
