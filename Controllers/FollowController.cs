using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using failure_api.Models;
using System.Threading.Tasks;
using System.Linq;
using failure_api.Data;

namespace failure_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ApplicationDbContext _context = context;

        [Authorize]
        [HttpPost("follow/{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var followed = await _userManager.FindByNameAsync(username);

            if (followed == null || !followed.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var follow = new Follow
            {
                IdFollowing = user.Id,
                IdFollowed = followed.Id,
                FollowDate = DateTime.UtcNow,
                Allowed = !followed.Private,
                AllowDate = !followed.Private ? DateTime.UtcNow : null,
                Active = true
            };

            // Check if the user is already following the target user
            var existingFollow = _context.Follows.Where(f => f.IdFollowing == user.Id && f.IdFollowed == followed.Id).FirstOrDefault();

            // If the user is already following the target user, update the follow record
            if (existingFollow != null)
            {
                if (existingFollow.Active)
                {
                    return BadRequest("User is already following the target user.");
                }

                existingFollow.Active = true;
                existingFollow.Allowed = !followed.Private;
                existingFollow.AllowDate = !followed.Private ? DateTime.UtcNow : null;

                _context.Follows.Update(existingFollow);
                await _context.SaveChangesAsync();
            } else {
                _context.Follows.Add(follow);
                await _context.SaveChangesAsync();
            }

            if(followed.Private)
            {
                return Ok("Follow request sent.");
            }

            return Ok("User followed successfully.");
        }

        [Authorize]
        [HttpPatch("unfollow/{username}")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var followed = await _userManager.FindByNameAsync(username);

            if (followed == null || !followed.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var follow = _context.Follows.Where(f => f.IdFollowing == user.Id && f.IdFollowed == followed.Id).FirstOrDefault();

            if (follow == null)
            {
                return NotFound("User is not following the target user.");
            }

            follow.Active = false;

            _context.Follows.Update(follow);
            await _context.SaveChangesAsync();

            return Ok("User unfollowed successfully.");
        }

        [Authorize]
        [HttpPatch("allow/{username}")]
        public async Task<IActionResult> Allow(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var following = await _userManager.FindByNameAsync(username);

            if (following == null || !following.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var follow = _context.Follows.Where(f => f.IdFollowing == following.Id && f.IdFollowed == user.Id).FirstOrDefault();

            if (follow == null)
            {
                return NotFound("User is not following the target user.");
            }

            if (follow.Allowed)
            {
                return BadRequest("User is already allowed.");
            }

            follow.Allowed = true;
            follow.AllowDate = DateTime.UtcNow;

            _context.Follows.Update(follow);
            await _context.SaveChangesAsync();

            return Ok("User allowed successfully.");
        }

        [Authorize]
        [HttpPatch("disallow/{username}")]
        public async Task<IActionResult> Disallow(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var following = await _userManager.FindByNameAsync(username);

            if (following == null || !following.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var follow = _context.Follows.Where(f => f.IdFollowing == following.Id && f.IdFollowed == user.Id).FirstOrDefault();

            if (follow == null)
            {
                return NotFound("User is not following the target user.");
            }

            if (!follow.Allowed)
            {
                return BadRequest("User is already disallowed.");
            }

            follow.Allowed = false;
            follow.AllowDate = null;

            _context.Follows.Update(follow);
            await _context.SaveChangesAsync();

            return Ok("User disallowed successfully.");
        }

        [Authorize]
        [HttpPatch("deny/{username}")]
        public async Task<IActionResult> Deny(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var following = await _userManager.FindByNameAsync(username);

            if (following == null || !following.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var follow = _context.Follows.Where(f => f.IdFollowing == following.Id && f.IdFollowed == user.Id).FirstOrDefault();

            if (follow == null)
            {
                return NotFound("User is not following the target user.");
            }

            if (follow.Allowed)
            {
                return BadRequest("User is already allowed.");
            }

            follow.Active = false;

            _context.Follows.Update(follow);
            await _context.SaveChangesAsync();

            return Ok("User denied successfully.");
        }

        [Authorize]
        [HttpGet("followers/{username}")]
        public async Task<IActionResult> GetFollowers(string username)
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

            if (userToGet.Private && !_context.Follows.Where(f => f.IdFollowing == user.Id && f.IdFollowed == userToGet.Id && f.Active && f.Allowed).Any() && user.Id != userToGet.Id)
            {
                return BadRequest("User is private.");
            }

            var followers = _context.Follows.Where(f => f.IdFollowed == userToGet.Id && f.Active && f.Allowed).ToList();

            // Get the usernames of the followers using the Ids
            var followersUsernames = followers.Select(f => _context.Users.Where(u => u.Id == f.IdFollowing).OrderBy(u => f.AllowDate).FirstOrDefault()?.UserName).ToList();

            return Ok(followersUsernames);
        }

        [Authorize]
        [HttpGet("following/{username}")]
        public async Task<IActionResult> GetFollowing(string username)
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

            if (userToGet.Private && !_context.Follows.Where(f => f.IdFollowing == user.Id && f.IdFollowed == userToGet.Id && f.Active && f.Allowed).Any() && user.Id != userToGet.Id)
            {
                return BadRequest("User is private.");
            }

            var following = _context.Follows.Where(f => f.IdFollowing == userToGet.Id && f.Active && f.Allowed).ToList();

            // Get the usernames of the following using the Ids
            var followingUsernames = following.Select(f => _context.Users.Where(u => u.Id == f.IdFollowed).OrderBy(u => f.AllowDate).FirstOrDefault()?.UserName).ToList();

            return Ok(followingUsernames);
        }

        [Authorize]
        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.Active)
            {
                return NotFound("User not found or inactive.");
            }

            var requests = _context.Follows.Where(f => f.IdFollowed == user.Id && f.Active && !f.Allowed).ToList();

            // Get the usernames of the followers using the Ids
            var requestsUsernames = requests.Select(f => _context.Users.Where(u => u.Id == f.IdFollowing).FirstOrDefault()?.UserName).ToList();

            return Ok(requestsUsernames);
        }
    }
}