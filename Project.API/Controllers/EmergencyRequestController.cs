using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Services;

namespace Project.API.Controllers
{
   
    public class EmergencyRequestController : APIBaseController
    {
        private readonly IEmergencyRequestService _emergencyRequest;

        public EmergencyRequestController(IEmergencyRequestService emergencyRequest)
        {
            _emergencyRequest = emergencyRequest;
        }

        [HttpGet("select-video/{userId}/{videoId}")]
        public async Task<IActionResult> SelectVideo(int userId, int videoId, [FromQuery] double latitude, [FromQuery] double longitude)
        {
            bool isSent = await _emergencyRequest.SendEmergencyRequest(userId, videoId, latitude, longitude);

            if (isSent)
                return Ok(new { Message = "تم إرسال طلب الطوارئ بنجاح." });
            else
                return StatusCode(500, "فشل إرسال طلب الطوارئ.");
        }
    }
}
