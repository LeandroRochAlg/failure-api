using System.ComponentModel.DataAnnotations;

namespace failure_api.Models
{
    public class ApplicationStep
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int JobApplicationId { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        [RegularExpression("^(Interview|Technical Interview|Group Dynamic|CV|Challenge|Phone Screen|Coding Test|Homework Assignment|On-Site Interview|Panel Interview|Behavioral Interview|HR Interview|Background Check|Reference Check|Offer Extended|Offer Accepted|Offer Declined|Negotiation|Contract Signing|Onboarding|Technical Presentation|Case Study|Assessment Center|Final Interview|Follow-up Interview|Job Shadowing|Work Sample Test|Personality Test|Culture Fit Interview|Video Interview|Portfolio Review|Other)$", ErrorMessage = "Invalid Type. Please provide a valid step type.")]
        public string Type { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Final { get; set; } = false;

        public DateTime StepDate { get; set; } = DateTime.UtcNow;

        public bool Progressed { get; set; } = false;

        public DateTime ResultDate { get; set; }

        public int? NextStepId { get; set; }

        public bool Active { get; set; } = true;
    }
}