using Project.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.API.DTOs
{
    public class VideoDTO
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int EmergencyServiceId { get; set; }
        public string EmergencyService { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
    }
}
