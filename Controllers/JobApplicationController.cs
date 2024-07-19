using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using failure_api.Models;
using failure_api.Data;
using failure_api.Services;
using failure_api.Filters;

namespace failure_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IBadgeService badgeService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
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

            jobApplication.ApplyDate = jobApplication.ApplyDate.ToUniversalTime();

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpJobApplicationAsync(user);

            return Ok("Job application registered.");
        }

        [HttpGet("list/{username}")]
        [ServiceFilter(typeof(PrivateProfileFilter))]
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

            // Returns the job applications of the user and a list of steps of each application
            var jobApplications = _context.JobApplications
                .Where(j => j.UserId == userToGet.Id && j.Active)
                .Select(j => new
                {
                    JobApplication = j,
                    Steps = _context.ApplicationSteps
                        .Where(s => s.JobApplicationId == j.Id)
                        .OrderBy(s => s.StepDate)
                        .ToList()
                })
                .OrderByDescending(j => j.JobApplication.ApplyDate)
                .ToList();

            return Ok(jobApplications);
        }

        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> UnactiveJobApplication(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == id && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            // Delete all steps of the application
            var steps = _context.ApplicationSteps.Where(s => s.JobApplicationId == jobApplication.Id && s.Active).ToList();

            foreach (var step in steps)
            {
                step.Active = false;

                await _badgeService.UpdateXpApplicationStepDeletedAsync(user);
            }

            if (jobApplication.GotIt)
            {
                await _badgeService.UpdateXpGotJobDeletedAsync(user, steps.Count, true);
            } else if (jobApplication.GotIt == false)
            {
                await _badgeService.UpdateXpGotJobDeletedAsync(user, steps.Count, false);
            }

            jobApplication.Active = false;

            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpJobApplicationDeletedAsync(user);

            return Ok("Job application deleted.");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateJobApplication(int id, JobApplication jobApplication)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplicationToUpdate = _context.JobApplications.FirstOrDefault(j => j.Id == id && j.UserId == user.Id);

            if (jobApplicationToUpdate == null)
            {
                return NotFound("Job application not found.");
            }
            
            jobApplicationToUpdate.Company = jobApplication.Company;
            jobApplicationToUpdate.ApplyDate = jobApplication.ApplyDate.ToUniversalTime();
            jobApplicationToUpdate.Description = jobApplication.Description;
            jobApplicationToUpdate.Active = jobApplication.Active;

            var firstStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == jobApplicationToUpdate.FirstStepId);
            
            if (firstStep != null)
            {
                if (jobApplication.ApplyDate < firstStep.StepDate)
                {
                    return BadRequest("Application date cannot be before the first step date.");
                }
            }


            await _context.SaveChangesAsync();

            return Ok("Job application updated.");
        }

        [HttpPut("success/{id}")]
        public async Task<IActionResult> SuccessJobApplication(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == id && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            // Set the last step as successful
            var step = _context.ApplicationSteps.FirstOrDefault(s => s.JobApplicationId == jobApplication.Id && s.NextStepId == null);

            if (step == null)
            {
                return NotFound("Last step not found.");
            }

            step.Final = true;

            jobApplication.GotIt = true;

            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpGotJobAsync(user, _context.ApplicationSteps.Count(s => s.JobApplicationId == jobApplication.Id), true);

            return Ok("Job application set as successful.");
        }

        [HttpPost("applicationStep")]
        public async Task<IActionResult> ApplicationStep(ApplicationStep jobApplicationStep)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == jobApplicationStep.JobApplicationId && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            jobApplicationStep.StepDate = jobApplicationStep.StepDate.ToUniversalTime();

            if (jobApplicationStep.StepDate < jobApplication.ApplyDate)
            {
                return BadRequest("Step date cannot be before the application date.");
            }

            _context.ApplicationSteps.Add(jobApplicationStep);
            await _context.SaveChangesAsync();

            // Find the last step of the application
            if (jobApplication.FirstStepId == null)
            {
                jobApplication.FirstStepId = jobApplicationStep.Id;
            }
            else
            {
                var step = _context.ApplicationSteps.FirstOrDefault(s => s.Id == jobApplication.FirstStepId);

                while (step!.NextStepId != null)
                {
                    step = _context.ApplicationSteps.FirstOrDefault(s => s.Id == step.NextStepId);
                }

                if (!step.Progressed)
                {
                    step.ResultDate = jobApplicationStep.StepDate;
                    step.Progressed = true;
                }

                step.NextStepId = jobApplicationStep.Id;
            }

            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpApplicationStepAsync(user);

            return Ok("Application step registered.");
        }

        [HttpPut("applicationStep/progressed/{id}/{date}")]
        public async Task<IActionResult> ProgressApplicationStep(int id, string date)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplicationStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == id);

            if (jobApplicationStep == null)
            {
                return NotFound("Application step not found.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == jobApplicationStep.JobApplicationId && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            DateTime dateP = DateTime.Parse(date);

            if (dateP < jobApplication.ApplyDate)
            {
                return BadRequest("Step date cannot be before the application date.");
            }

            if (jobApplicationStep.StepDate > dateP)
            {
                return BadRequest("Step date cannot be before the current step date.");
            }

            jobApplicationStep.ResultDate = dateP.ToUniversalTime();

            jobApplicationStep.Progressed = true;

            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpApplicationStepProgressedAsync(user);

            if (jobApplicationStep.Final)
            {
                jobApplication.GotIt = true;
                await _context.SaveChangesAsync();

                await _badgeService.UpdateXpGotJobAsync(user, _context.ApplicationSteps.Count(s => s.JobApplicationId == jobApplication.Id), true);

                return Ok("CONGRATULATIONS! You got the job!");
            }

            return Ok("Application step progressed.");
        }

        [HttpPut("applicationStep/failed/{id}/{date}")]
        public async Task<IActionResult> FailApplicationStep(int id, string date)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplicationStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == id);

            if (jobApplicationStep == null)
            {
                return NotFound("Application step not found.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == jobApplicationStep.JobApplicationId && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            DateTime dateP = DateTime.Parse(date).ToUniversalTime();

            if (dateP < jobApplication.ApplyDate)
            {
                return BadRequest("Step date cannot be before the application date.");
            }

            if (jobApplicationStep.StepDate > dateP)
            {
                return BadRequest("Step date cannot be before the current step date.");
            }

            jobApplicationStep.ResultDate = dateP.ToUniversalTime();

            jobApplicationStep.Progressed = true;

            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpApplicationStepProgressedAsync(user);

            if (jobApplicationStep.Final)
            {
                jobApplication.GotIt = false;
                await _context.SaveChangesAsync();

                await _badgeService.UpdateXpGotJobAsync(user, _context.ApplicationSteps.Count(s => s.JobApplicationId == jobApplication.Id), false);

                return Ok("You did not get the job.");
            }

            return Ok("Application step failed.");
        }

        [HttpPatch("applicationStep/delete/{id}")]
        public async Task<IActionResult> UnactiveApplicationStep(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplicationStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == id);

            if (jobApplicationStep == null)
            {
                return NotFound("Application step not found.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == jobApplicationStep.JobApplicationId && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            // Find the last step of the application
            if (jobApplication.FirstStepId == jobApplicationStep.Id)
            {
                jobApplication.FirstStepId = jobApplicationStep.NextStepId;
            }
            else
            {
                var step = _context.ApplicationSteps.FirstOrDefault(s => s.NextStepId == jobApplicationStep.Id);

                if (step != null)
                {
                    step.NextStepId = jobApplicationStep.NextStepId;
                }
            }

            jobApplicationStep.Active = false;

            await _context.SaveChangesAsync();

            await _badgeService.UpdateXpApplicationStepDeletedAsync(user);

            return Ok("Application step deleted.");
        }

        [HttpPatch("applicationStep/final/{id}")]
        public async Task<IActionResult> SetFinalApplicationStep(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var jobApplicationStep = _context.ApplicationSteps.FirstOrDefault(s => s.Id == id);

            if (jobApplicationStep == null)
            {
                return NotFound("Application step not found.");
            }

            var jobApplication = _context.JobApplications.FirstOrDefault(j => j.Id == jobApplicationStep.JobApplicationId && j.UserId == user.Id);

            if (jobApplication == null)
            {
                return NotFound("Job application not found.");
            }

            jobApplicationStep.Final = true;

            await _context.SaveChangesAsync();

            return Ok("Application step set as final.");
        }
    }
}