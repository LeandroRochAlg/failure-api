using System.ComponentModel.DataAnnotations;
using failure_api.Validators;

namespace failure_api.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = "";

        [Required]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        [NoLinks(ErrorMessage = "The {0} value cannot contain links.")]
        public string AppliedOn { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        [NoLinks(ErrorMessage = "The {0} value cannot contain links.")]
        public string Role { get; set; } = "";

        [Required]
        [StringLength(200, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Company { get; set; } = "";

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Description { get; set; } = "";
        
        public DateTime ApplyDate { get; set; } = DateTime.UtcNow;

        public int? FirstStepId { get; set; }

        public bool GotIt { get; set; }

        public bool Active { get; set; } = true;
    }
}