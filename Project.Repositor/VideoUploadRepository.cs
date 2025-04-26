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
        //public async Task<UserUploadVideo> AddUserUploadedVideo(UserUploadVideo newVideo)
        //{
        //    if (newVideo == null || string.IsNullOrEmpty(newVideo.VideoUrl))
        //        return null; // Return null if validation fails

        //    _dbContext.uploadVideos.Add(newVideo);
        //    await _dbContext.SaveChangesAsync();

        //    // Refresh the entity to include generated values (e.g., UploadedVideoId)
        //    _dbContext.Entry(newVideo).State = EntityState.Detached;
        //    return await _dbContext.uploadVideos.FindAsync(newVideo.UploadVideoId);
        //}

        public async Task<UserUploadVideo> AddUserUploadedVideo(UserUploadVideo video)
        {
            if (video == null || string.IsNullOrEmpty(video.VideoUrl) || video.EmergencyServiceId == 0)
                return null;

            _dbContext.uploadVideos.Add(video);
            await _dbContext.SaveChangesAsync();

            // Refresh the entity to include generated values (e.g., UploadedVideoId)
            _dbContext.Entry(video).State = EntityState.Detached;
            return await _dbContext.uploadVideos.FindAsync(video.UploadVideoId);
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