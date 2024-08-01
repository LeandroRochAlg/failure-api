using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using failure_api.Models;
using failure_api.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Google.Apis.Auth;
using DotNetEnv;

namespace failure_api.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            string hashedGoogleId = new HashService().HashGoogleId(model.IdGoogle);

            var user = new ApplicationUser
            {
                UserName = model.Username,
                IdGoogle = hashedGoogleId
            };

            try {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var userCreated = await _userManager.FindByNameAsync(model.Username);

                    if (userCreated == null)
                    {
                        return NotFound("Internal server error. User not found after creation.");
                    }

                    userCreated.Email = model.Email;
                    userCreated.EmailConfirmed = true;

                    if (model.TokenGoogle != null)
                    {
                        // Validate the Google token
                        // var payload = await GoogleJsonWebSignature.ValidateAsync(model.TokenGoogle);

                        // if (payload == null)
                        // {
                        //     return BadRequest("Invalid Google token.");
                        // }

                        var info = new UserLoginInfo("Google", userCreated.IdGoogle, "Google");

                        var resultGoogle = await _userManager.AddLoginAsync(userCreated, info);

                        if (!resultGoogle.Succeeded)
                        {
                            return BadRequest("Error linking Google account to user.");
                        }
                    }

                    return Ok();
                }

                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")          // If a user with the same username already exists
                        {
                            return Conflict("User with the same username already exists.");
                        } else if (error.Description == "DuplicateIdGoogle")   // If a user with the same Google ID already exists
                        {
                            return Conflict("This Google account is already linked to another user.");
                        } else
                        {
                            return BadRequest("Internal Server Error");
                        }
                    }
                }
            } catch (DbUpdateException ex) {
                // Check if the inner exception is a PostgresException
                if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    if (pgEx.ConstraintName == "IX_Users_IdGoogle")
                    {
                        return Conflict("This Google account is already linked to another user.");
                    }
                }

                // Re-throw the exception if it's not handled
                throw;
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok();
            }

            if (result.IsLockedOut)
            {
                return StatusCode(423, "User is locked out.");
            }

            if (result.IsNotAllowed)
            {
                return StatusCode(403, "User is not allowed to sign in.");
            }

            return Unauthorized("Invalid login attempt.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginModel model)
        {
            try
            {
                // var payload = await GoogleJsonWebSignature.ValidateAsync(model.TokenGoogle);

                // if (payload == null)
                // {
                //     return BadRequest("Invalid Google token.");
                // }

                string hashedGoogleId = new HashService().HashGoogleId(model.IdGoogle);

                var user = await _userManager.FindByLoginAsync("Google", hashedGoogleId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                await _signInManager.SignInAsync(user, true);

                return Ok();
            }
            catch (InvalidJwtException)
            {
                return BadRequest("Invalid Google token.");
            }
        }
    }
}