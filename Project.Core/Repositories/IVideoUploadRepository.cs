using Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories
{
    public interface IVideoUploadRepository
    {
        Task<IEnumerable<UserUploadVideo>> GetUserUploadedVideosAsync(int userId);
        Task<UserUploadVideo> GetUploadVideoAsync(int VideoId);
        Task<UserUploadVideo> AddUserUploadedVideo(UserUploadVideo newVideo);
        //Task<UserUploadVideo> UpdateVideoAsync(UserUploadVideo video);
        Task<bool> DeleteVideoAsync(int VideoId);
    }
}
