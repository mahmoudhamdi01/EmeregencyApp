using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Core.Repositories;

namespace Project.API.Controllers
{
    public class VideoController : APIBaseController
    {
        private readonly IVideoRepository _videoRepo;
        private readonly IMapper _mapper;

        public VideoController(IVideoRepository VideoRepo, IMapper mapper)
        {
            _videoRepo = VideoRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetAllVideos()
        {
            var Videos = await _videoRepo.GetAllVideosAsync();
            var MappedVideos = _mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(Videos);
            return Ok(MappedVideos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> getVideoById(int id)
        {
            var video = await _videoRepo.GetVideoByIdAsync(id);
            var MappedVideos = _mapper.Map<Video, VideoDTO>(video);
            return Ok(MappedVideos);
        }
    }
}
