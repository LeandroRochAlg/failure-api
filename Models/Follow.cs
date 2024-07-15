using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace failure_api.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdFollowed { get; set; } = "";

        [Required]
        public string IdFollowing { get; set; } = "";

        public DateTime FollowDate { get; set; } = DateTime.UtcNow;

        public bool Allowed { get; set; } = false;

        public DateTime? AllowDate { get; set; }

        public bool Active { get; set; } = true;

        [ForeignKey("IdFollowed")]
        public required ApplicationUser Followed { get; set; }

        [ForeignKey("IdFollowing")]
        public required ApplicationUser Following { get; set; }
    }
}