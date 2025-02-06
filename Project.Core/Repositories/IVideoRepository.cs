using Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories
{
    public interface IVideoRepository
    {
        Task<IEnumerable<Video>> GetAllVideosAsync();
        Task<Video> GetVideoByIdAsync(int id);
    }
}
