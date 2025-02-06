using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities
{
    public class UserUploadVideo
    {
        [Key]
        public int UploadVideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        [ForeignKey("EmergencyServiceId")]
        public int EmergencyServiceId { get; set; }
        public EmergencyServices EmergencyService { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
