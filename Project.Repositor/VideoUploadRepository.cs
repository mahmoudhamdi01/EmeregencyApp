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
    public class VideoUploadRepository : IVideoUploadRepository
    {
        private readonly EmergencyContext _dbContext;

        public VideoUploadRepository(EmergencyContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<UserUploadVideo> AddUserUploadedVideo(UserUploadVideo newVideo)
        {
            await _dbContext.uploadVideos.AddAsync(newVideo);
            await _dbContext.SaveChangesAsync();
            return newVideo;
        }

        public async Task<bool> DeleteVideoAsync(int VideoId)
        {
            var video = await _dbContext.uploadVideos.FirstOrDefaultAsync(v => v.UploadVideoId == VideoId);
            if (video == null)
                return false;

            _dbContext.uploadVideos.Remove(video);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserUploadVideo> GetUploadVideoAsync(int VideoId)
        {
            return await _dbContext.uploadVideos.Include(v=>v.EmergencyService).Include(v => v.User).Where(v => v.UploadVideoId == VideoId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserUploadVideo>> GetUserUploadedVideosAsync(int userId)
        {
            return await _dbContext.uploadVideos.Include(v => v.EmergencyService).Include(v => v.User).Where(v => v.UserId == userId).ToListAsync();
        }
    }
}
