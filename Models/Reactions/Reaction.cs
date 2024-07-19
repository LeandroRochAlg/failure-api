using System.ComponentModel.DataAnnotations;

namespace failure_api.Models
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression("^(Like|Congrats|Love|Hate|Neutral|Haha|Good Luck)$", ErrorMessage = "Invalid Type. Please provide a valid reaction type.")]
        public string ReactionName { get; set; } = "";

        [Required]
        [RegularExpression("^(Job|Step)$", ErrorMessage = "Invalid Type. Please provide a valid reaction type.")]
        public string ReactionType { get; set; } = "";

        public int? JobApplicationId { get; set; }

        public int? ApplicationStepId { get; set; }

        public string UserId { get; set; } = "";

        public DateTime ReactionDate { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;
    }
}