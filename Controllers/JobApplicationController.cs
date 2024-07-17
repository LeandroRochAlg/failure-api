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
    public class JobApplicationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IBadgeService badgeService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ApplicationDbContext _context = context;
        private readonly IBadgeService _badgeService = badgeService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterJobApplication(JobApplication jobApplication)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            jobApplication.UserId = user.Id;

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpJobApplicationAsync(user);

            return Ok("Job application registered.");
        }

        [HttpGet("list/{username}")]
        public async Task<IActionResult> ListJobApplications(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var userToGet = await _userManager.FindByNameAsync(username);

            if (userToGet == null || !userToGet.Active)
            {
                return NotFound("User not found or inactive.");
            }

            if (user.UserName != username)
            {
                if (userToGet.Private)
                {
                    if (!_context.Follows.Where(f => f.IdFollowing == user.Id && f.IdFollowed == userToGet.Id && f.Active && f.Allowed).Any())
                    {
                        return Unauthorized("User is private.");
                    }
                }
            }

            var jobApplications = _context.JobApplications.Where(j => j.UserId == userToGet.Id && j.Active).ToList();

            return Ok(jobApplications);
        }
    }
}