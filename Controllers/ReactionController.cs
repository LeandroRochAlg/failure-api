using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using failure_api.Models;
using failure_api.Data;
using failure_api.Services;

namespace failure_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IBadgeService badgeService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly IBadgeService _badgeService = badgeService;

        [HttpPost("react")]
        public async Task<IActionResult> React([FromBody] Reaction reaction)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return Unauthorized("User not found or inactive.");
            }

            var jobApplication = new JobApplication();

            if (reaction.ReactionType == "Job")
            {
                jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == reaction.JobApplicationId);
            }
            else
            {
                var applicationStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == reaction.ApplicationStepId);

                if (applicationStep == null)
                {
                    return NotFound("Application Step not found.");
                }

                jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == applicationStep.JobApplicationId);
            }

            if (jobApplication == null || !jobApplication.Active)
            {
                return NotFound("Job Application not found.");
            }

            var existingReaction = _context.Reactions.FirstOrDefault(r => r.UserId == user.Id && r.JobApplicationId == reaction.JobApplicationId && r.ApplicationStepId == reaction.ApplicationStepId && r.Active);

            if (existingReaction != null)
            {
                existingReaction.ReactionName = reaction.ReactionName;
                existingReaction.ReactionDate = reaction.ReactionDate;

                _context.Reactions.Update(existingReaction);
                await _context.SaveChangesAsync();
                
                return Ok("Reaction updated.");
            }

            reaction.UserId = user.Id;
            _context.Reactions.Add(reaction);
            await _context.SaveChangesAsync();

            var reacter = user;
            var reacted = _context.Users.FirstOrDefault(u => u.Id == jobApplication.UserId);

            if (reacted == null || !reacted.Active)
            {
                return NotFound("User not found or inactive.");
            }

            await _badgeService.UpdateBadgeReactionAsync(reacter, reacted);

            return Ok("Reaction added.");
        }

        [HttpPatch("delete")]
        public async Task<IActionResult> DeleteReact([FromBody] Reaction reaction)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return Unauthorized("User not found or inactive.");
            }

            var jobApplication = new JobApplication();

            if (reaction.ReactionType == "Job")
            {
                jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == reaction.JobApplicationId);
            }
            else
            {
                var applicationStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == reaction.ApplicationStepId);

                if (applicationStep == null)
                {
                    return NotFound("Application Step not found.");
                }

                jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == applicationStep.JobApplicationId);
            }

            if (jobApplication == null || !jobApplication.Active)
            {
                return NotFound("Job Application not found.");
            }

            var existingReaction = _context.Reactions.FirstOrDefault(r => r.UserId == user.Id && r.JobApplicationId == reaction.JobApplicationId && r.ApplicationStepId == reaction.ApplicationStepId && r.Active);

            if (existingReaction == null)
            {
                return NotFound("Reaction not found.");
            }

            existingReaction.Active = false;
            _context.Reactions.Update(existingReaction);
            await _context.SaveChangesAsync();

            var reacter = user;
            var reacted = _context.Users.FirstOrDefault(u => u.Id == jobApplication.UserId);

            if (reacted == null || !reacted.Active)
            {
                return NotFound("User not found or inactive.");
            }

            await _badgeService.updateBadgeReactionDeletedAsync(reacter, reacted);

            return Ok("Reaction deleted.");
        }
    }
}