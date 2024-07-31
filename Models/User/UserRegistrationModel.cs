namespace failure_api.Models{
    public class UserRegistrationModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string IdGoogle { get; set; }
        public required string Email { get; set; }
        public required string TokenGoogle { get; set; }
    }
}