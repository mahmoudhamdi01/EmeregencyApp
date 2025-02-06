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
    public class VideoRepository : IVideoRepository
    {
        private readonly EmergencyContext _dbContext;

        public VideoRepository(EmergencyContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            return await _dbContext.videos.Include(v => v.EmergencyService).Include(v=>v.User).ToListAsync();
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            return await _dbContext.videos.Include(v => v.EmergencyService).Include(v=>v.User).Where(v => v.VideoId == id).FirstOrDefaultAsync();
        }
    }
}
