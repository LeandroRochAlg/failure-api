using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using failure_api.Models;
using System.ComponentModel.DataAnnotations;

namespace failure_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            return Ok(new
            {
                user.UserName,
                user.CreationDate,
                user.Badge,
                user.Experience,
                user.Private,
                user.Description,
                user.Link1,
                user.Link2,
                user.Link3
            });
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            user.Description = model.Description;
            user.Link1 = model.Link1;
            user.Link2 = model.Link2;
            user.Link3 = model.Link3;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPatch("me/private")]
        public async Task<IActionResult> TogglePrivate()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            user.Private = !user.Private;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPatch("me/active")]
        public async Task<IActionResult> ToggleActive()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Active = !user.Active;

            var result = await _userManager.UpdateAsync(user);

            // If the user is inactive, sign them out
            if (!user.Active)
            {
                await _signInManager.SignOutAsync();
                return Ok("Logged out.");
            }

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPut("me/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                if (model.Logout)
                {
                    await _signInManager.SignOutAsync();
                    return Ok("Logged out.");
                }

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPut("me/username")]
        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var newUsername = model.NewUsername;

            var result = await _userManager.SetUserNameAsync(user, newUsername);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            return Ok(new
            {
                user.UserName,
                user.CreationDate,
                user.Badge,
                user.Experience,
                user.Private,
                user.Description,
                user.Link1,
                user.Link2,
                user.Link3
            });
        }
    }

    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; } = "";

        [Required]
        public string NewPassword { get; set; } = "";

        [Required]
        public bool Logout { get; set; } = false;
    }

    public class ChangeUsernameModel
    {
        [Required]
        public string NewUsername { get; set; } = "";
    }
}