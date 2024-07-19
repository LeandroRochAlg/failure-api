using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace failure_api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string IdGoogle { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public int Badge { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public bool Private { get; set; } = false;
        public bool Active { get; set; } = true;

        [StringLength(300, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Description { get; set; } = "";
        public string Link1 { get; set; } = "";
        public string Link2 { get; set; } = "";
        public string Link3 { get; set; } = "";
    }
}