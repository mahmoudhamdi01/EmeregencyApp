using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.DTOs;
using Project.Core.Entities;
using Project.Core.Repositories;

namespace Project.API.Controllers
{
    
    public class UploadVideoController : APIBaseController
    {
        private readonly IVideoUploadRepository _uploadRepo;
        private readonly IMapper _mapper;

        public UploadVideoController(IVideoUploadRepository UploadRepo, IMapper mapper)
        {
            _uploadRepo = UploadRepo;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserUploadVideo>>> GetUserUploadedVideos(int userId)
        {
            var videos = await _uploadRepo.GetUserUploadedVideosAsync(userId);
            var MappedVideos = _mapper.Map<IEnumerable<UserUploadVideo>, IEnumerable<UploadVideoDTO>>(videos);
            return Ok(MappedVideos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserUploadVideo>> GetUserUploadedVideoById(int id)
        {
            var video = await _uploadRepo.GetUploadVideoAsync(id);
            var MappedVideos = _mapper.Map<UserUploadVideo, UploadVideoDTO>(video);
            return Ok(MappedVideos);
        }


        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteVideo(int id)
        {
            //var result = await _uploadRepo.DeleteVideoAsync(id);
            //if (!result)
            //    return NotFound();
            //return Ok();
            return await _uploadRepo.DeleteVideoAsync(id);
        }

    }
}
