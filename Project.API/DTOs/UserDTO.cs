namespace Project.API.DTOs
{
    public class UserDTO
    {
		public int UserId { get; set; }
		public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
