using Microsoft.EntityFrameworkCore;
using Project.Core.Entities;
using Project.Core.Repositories;
using Project.Repositor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositor
{
    public class Services : IServices
    {
        private readonly EmergencyContext _context;

        public Services(EmergencyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EmergencyServices>> GetAllServicesAsync()
       => await _context.emergencyServices.ToListAsync();


        public async Task<EmergencyServices> GetServicesByIdAsync(int id)
        => await _context.emergencyServices.FindAsync(id);
    }
}
