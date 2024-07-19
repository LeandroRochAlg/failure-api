using failure_api.Models;

namespace failure_api.Services
{
    public interface IBadgeService
    {
        Task UpdateBadgeFollowAsync(ApplicationUser follower, ApplicationUser followed);
        Task UpdateBadgeUnfollowAsync(ApplicationUser follower, ApplicationUser followed);
        Task UpdateXpJobApplicationAsync(ApplicationUser user);
        Task UpdateXpJobApplicationDeletedAsync(ApplicationUser user);
        Task UpdateXpApplicationStepAsync(ApplicationUser user);
        Task UpdateXpApplicationStepDeletedAsync(ApplicationUser user);
        Task UpdateXpApplicationStepProgressedAsync(ApplicationUser user);
        Task UpdateXpGotJobAsync(ApplicationUser user, int numSteps, bool GotIt);
        Task UpdateXpGotJobDeletedAsync(ApplicationUser user, int numSteps, bool GotIt);
    }
}