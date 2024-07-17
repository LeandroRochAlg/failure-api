using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using failure_api.Data;
using failure_api.Models;

namespace failure_api.Filters
{
    public class PrivateProfileFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public PrivateProfileFilter(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(context.HttpContext.User);

            if (user == null || !user.Active)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.HttpContext.Response.WriteAsync("User not found or inactive.");
                return;
            }

            var routeData = context.HttpContext.GetRouteData();
            var username = routeData.Values["username"]?.ToString();
            var userToGet = await _userManager.FindByNameAsync(username!);

            if (userToGet == null || !userToGet.Active)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.HttpContext.Response.WriteAsync("User not found or inactive.");
                return;
            }

            if (user.UserName != username)
            {
                if (userToGet.Private)
                {
                    var isFollowing = await _dbContext.Follows
                        .Where(f => f.IdFollowing == user.Id && f.IdFollowed == userToGet.Id && f.Active && f.Allowed)
                        .AnyAsync();

                    if (!isFollowing)
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.HttpContext.Response.WriteAsync("User is private.");
                        return;
                    }
                }
            }

            await next();
        }
    }
}