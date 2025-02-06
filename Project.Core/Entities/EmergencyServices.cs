using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities
{
    public class EmergencyServices
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}

