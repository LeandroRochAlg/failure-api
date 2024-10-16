using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using failure_api.Models;

namespace failure_api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Follow> Follows { get; init; }

        public DbSet<JobApplication> JobApplications { get; init; }

        public DbSet<ApplicationStep> ApplicationSteps { get; init; }

        public DbSet<Reaction> Reactions { get; init; }

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

                // Setting up the relationship between ApplicationUser and JobApplication
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Setting up the relationship between JobApplication and ApplicationStep
                entity.HasOne<ApplicationStep>()
                      .WithMany()
                      .HasForeignKey(e => e.FirstStepId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApplicationStep>(entity =>
            {
                entity.ToTable("ApplicationSteps");
                entity.HasIndex(e => e.Id).IsUnique();
                
                // Setting up the relationship between JobApplication and ApplicationStep
                entity.HasOne<JobApplication>()
                      .WithMany()
                      .HasForeignKey(e => e.JobApplicationId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Setting up the relationship between ApplicationStep and ApplicationStep
                entity.HasOne<ApplicationStep>()
                      .WithMany()
                      .HasForeignKey(e => e.NextStepId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Reaction>(entity =>
            {
                entity.ToTable("Reactions");
                entity.HasIndex(e => new { e.ReactionName, e.ReactionType, e.JobApplicationId, e.ApplicationStepId, e.UserId }).IsUnique();

                // Setting up the relationship between ApplicationUser and Reaction
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Setting up the relationship between JobApplication and Reaction
                entity.HasOne<JobApplication>()
                      .WithMany()
                      .HasForeignKey(e => e.JobApplicationId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Setting up the relationship between ApplicationStep and Reaction
                entity.HasOne<ApplicationStep>()
                      .WithMany()
                      .HasForeignKey(e => e.ApplicationStepId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}