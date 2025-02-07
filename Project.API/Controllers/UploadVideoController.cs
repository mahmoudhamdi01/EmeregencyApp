using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.API.Services;
using Project.Core.Entities;
using Project.Core.Repositories;
using Project.Repositor.Data;

namespace Project.API.Controllers
{
    
    public class UploadVideoController : APIBaseController
    {
        private readonly IVideoUploadRepository _uploadRepo;
        private readonly IMapper _mapper;
        private readonly EmergencyContext _context;
        private readonly IEmergencyRequestService _emergencyRequestService;

        public UploadVideoController(IVideoUploadRepository UploadRepo,
            IMapper mapper, EmergencyContext context,
            IEmergencyRequestService emergencyRequestService)
        {
            _uploadRepo = UploadRepo;
            _mapper = mapper;
            _context = context;
            _emergencyRequestService = emergencyRequestService;
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

        //[HttpPost]
        //public async Task<IActionResult> AddUserUploadedVideo([FromBody] UserUploadVideo newVideo)
        //{
        //    if (newVideo == null || string.IsNullOrEmpty(newVideo.VideoUrl))
        //        return BadRequest("VideoUrl is required.");

        //    var addedVideo = await _uploadRepo.AddUserUploadedVideo(newVideo);

        //    if (addedVideo == null)
        //        return StatusCode(500, "Failed to add the video.");

        //    return CreatedAtAction(nameof(GetUserUploadedVideoById), new { id = addedVideo.UploadVideoId }, addedVideo);
        //}

        //[HttpPost]
        //public async Task<ActionResult<UserUploadVideo>> UploadVideo([FromForm] IFormFile videoFile, [FromForm] int userId, [FromForm] string title, [FromForm] string description)
        //{
        //    if (videoFile == null || videoFile.Length == 0)
        //        return BadRequest("No video file provided.");

        //    // Check if the file is a valid video format
        //    if (!IsVideoFile(videoFile))
        //        return BadRequest("Invalid video file format. Supported formats: mp4, avi, mov.");

        //    // Save the video file to a storage location (e.g., local server or cloud storage)
        //    var filePath = Path.Combine("wwwroot", "videos", videoFile.FileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await videoFile.CopyToAsync(stream);
        //    }

        //    // Generate a public URL for the video
        //    var videoUrl = $"https://your-server-url/videos/{videoFile.FileName}";

        //    // Return the video URL or additional data
        //    return Ok(new { VideoUrl = videoUrl, UserId = userId, Title = title, Description = description });
        //}

        //private bool IsVideoFile(IFormFile file)
        //{
        //    var allowedExtensions = new[] { ".mp4", ".avi", ".mov" };
        //    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    return allowedExtensions.Contains(ext);
        //}

        #region Add-Method
        //[HttpPost]
        //public async Task<IActionResult> UploadAndAddVideo([FromForm] IFormFile videoFile, [FromForm] int userId, [FromForm] string title, [FromForm] string description, [FromForm] int? emergencyServiceId = null)
        //{
        //    if (videoFile == null || videoFile.Length == 0)
        //        return BadRequest("No video file provided.");

        //    // Check if the file is a valid video format
        //    if (!IsVideoFile(videoFile))
        //        return BadRequest("Invalid video file format. Supported formats: mp4, avi, mov.");

        //    // Ensure the directory exists
        //    var directoryPath = Path.Combine("wwwroot", "videos");
        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }

        //    // Save the video file to the storage location
        //    var filePath = Path.Combine(directoryPath, videoFile.FileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await videoFile.CopyToAsync(stream);
        //    }

        //    // Generate a public URL for the video
        //    var videoUrl = $"https://localhost:7040/videos/{videoFile.FileName}"; // Update with your actual server URL

        //    // Validate the UserId
        //    var user = await _context.users.FindAsync(userId);
        //    if (user == null)
        //        return NotFound($"User with ID {userId} not found.");

        //    // Determine the EmergencyServiceId (use default if not provided)
        //    var serviceId = emergencyServiceId ?? 1; // Default to Ambulance (ID = 1)

        //    // Validate the EmergencyServiceId
        //    var service = await _context.emergencyServices.FindAsync(serviceId);
        //    if (service == null)
        //        return NotFound($"Emergency service with ID {serviceId} not found.");

        //    // Create a new UserUploadedVideo entity
        //    var userUploadedVideo = new UserUploadVideo
        //    {
        //        Title = title,
        //        Description = description,
        //        VideoUrl = videoUrl,
        //        UserId = userId,
        //        EmergencyServiceId = serviceId
        //    };

        //    // Add the video to the database using Repository
        //    var addedVideo = await _uploadRepo.AddUserUploadedVideo(userUploadedVideo);

        //    if (addedVideo == null)
        //        return StatusCode(500, "Failed to add the video to the database.");

        //    return CreatedAtAction(nameof(GetUserUploadedVideoById), new { id = addedVideo.UploadVideoId }, addedVideo);
        //} 
        #endregion

        [HttpPost]
        public async Task<IActionResult> UploadAndAddVideo([FromForm] IFormFile videoFile, [FromForm] int userId, [FromForm] string title, [FromForm] string description, [FromForm] int? emergencyServiceId = null, [FromForm] double latitude = 0, [FromForm] double longitude = 0)
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("No video file provided.");

            // Check if the file is a valid video format
            if (!IsVideoFile(videoFile))
                return BadRequest("Invalid video file format. Supported formats: mp4, avi, mov.");

            // Ensure the directory exists
            var directoryPath = Path.Combine("wwwroot", "videos");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the video file to the storage location
            var filePath = Path.Combine(directoryPath, videoFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            // Generate a public URL for the video
            //var videoUrl = $"https://localhost:7040/videos/{videoFile.FileName}";
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/videos"; // Dynamic base URL
            var videoUrl = Path.Combine(baseUrl, videoFile.FileName).Replace("\\", "/");

            // Validate the UserId
            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return NotFound($"User with ID {userId} not found.");

            // Determine the EmergencyServiceId (use default if not provided)
            var serviceId = emergencyServiceId ?? 1; // Default to Ambulance (ID = 1)

            // Validate the EmergencyServiceId
            var service = await _context.emergencyServices.FindAsync(serviceId);
            if (service == null)
                return NotFound($"Emergency service with ID {serviceId} not found.");

            // Create a new UserUploadedVideo entity
            var userUploadedVideo = new UserUploadVideo
            {
                Title = title,
                Description = description,
                VideoUrl = videoUrl,
                UserId = userId,
                EmergencyServiceId = serviceId
            };

            // Add the video to the database using Repository
            var addedVideo = await _uploadRepo.AddUserUploadedVideo(userUploadedVideo);

            if (addedVideo == null)
                return StatusCode(500, "Failed to add the video to the database.");

            // Automatically send an emergency request with the provided location
            bool isSent = await _emergencyRequestService.SendEmergencyRequest(userId, addedVideo.UploadVideoId, latitude, longitude);

            if (isSent)
                return CreatedAtAction(nameof(GetUserUploadedVideoById), new { id = addedVideo.UploadVideoId }, addedVideo);

            return StatusCode(500, "Failed to send emergency request.");
        }

        private bool IsVideoFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".mp4", ".avi", ".mov" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(ext);
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
