namespace failure_api.Models
{
    public class UserLoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}