using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using failure_api.Models;

namespace failure_api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Follow> Follows { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configurations
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.IdGoogle).IsUnique();
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<Follow>(entity =>
            {
                entity.ToTable("Follows");
                entity.HasIndex(e => new { e.IdFollowed, e.IdFollowing }).IsUnique();

                entity.HasOne(e => e.Followed)
                    .WithMany()
                    .HasForeignKey(e => e.IdFollowed)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Following)
                    .WithMany()
                    .HasForeignKey(e => e.IdFollowing)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<JobApplication>(entity =>
            {
                entity.ToTable("JobApplications");
            });
        }
    }
}