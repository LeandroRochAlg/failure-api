using System.ComponentModel.DataAnnotations;

namespace failure_api.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = "";

        [Required]
        public string AppliedOn { get; set; } = "";

        [Required]
        public string Role { get; set; } = "";

        [Required]
        public string Company { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";
        
        public DateTime ApplyDate { get; set; } = DateTime.UtcNow;

        public int? FirstStepId { get; set; }

        public bool GotIt { get; set; } = false;

        public bool Active { get; set; } = true;
    }
}