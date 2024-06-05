namespace failure_api.Models{
    public class UserRegistrationModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string IdGoogle { get; set; }
        public string Description { get; set; } = "";
        public string Link1 { get; set; } = "";
        public string Link2 { get; set; } = "";
        public string Link3 { get; set; } = "";
    }
}