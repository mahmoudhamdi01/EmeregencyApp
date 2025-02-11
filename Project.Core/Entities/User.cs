using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string USerName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } 

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
