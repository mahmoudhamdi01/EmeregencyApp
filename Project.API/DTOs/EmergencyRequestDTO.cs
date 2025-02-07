namespace Project.API.DTOs
{
    public class EmergencyRequestDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public string VideoDescription { get; set; }
        public double Latitude { get; set; } 
        public double Longitude { get; set; }
    }
}
