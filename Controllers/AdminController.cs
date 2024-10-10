using failure_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace failure_api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost("addUser/{username}")]
        public async Task<IActionResult> AddUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }
            
            await userManager.AddToRoleAsync(user, "Admin");

            return Ok(username + " is now an administrator");
        }

        [HttpPost("removeUser/{username}")]
        public async Task<IActionResult> RemoveUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }
            
            await userManager.RemoveFromRoleAsync(user, "Admin");
            
            return Ok(username + " is no longer an administrator");
        }
    }
}