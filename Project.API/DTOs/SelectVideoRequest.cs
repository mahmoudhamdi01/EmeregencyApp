namespace Project.API.DTOs
{
    public class SelectVideoRequest
    {
		public int VideoId { get; set; }
		public int UserId { get; set; } 
		public double Latitude { get; set; } 
		public double Longitude { get; set; }
	}
}
