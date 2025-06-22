using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities
{
	public class EmergencyRequest
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string VideoDescription { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public int ServiceId { get; set; }
		public string ServiceName { get; set; }
		public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
	}
}
