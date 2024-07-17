using failure_api.Models;

namespace failure_api.Services
{
    public interface IBadgeService
    {
        Task UpdateBadgeFollowAsync(ApplicationUser follower, ApplicationUser followed);
        Task UpdateBadgeUnfollowAsync(ApplicationUser follower, ApplicationUser followed);
        Task UpdateXpJobApplicationAsync(ApplicationUser user);
    }
}