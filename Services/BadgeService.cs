using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using failure_api.Models;
using failure_api.Data;
using System.IO;
using Newtonsoft.Json;

namespace failure_api.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BadgeService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task UpdateBadgeFollowAsync(ApplicationUser follower, ApplicationUser followed)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            follower.Badge += badgeValues.FollowValue;
            followed.Badge += badgeValues.FollowedValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBadgeUnfollowAsync(ApplicationUser follower, ApplicationUser followed)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            follower.Badge -= badgeValues.FollowValue;
            followed.Badge -= badgeValues.FollowedValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBadgeReactionAsync(ApplicationUser reacter, ApplicationUser reacted)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            reacter.Badge += badgeValues.ReactionValue;
            reacted.Badge += badgeValues.ReactionReceivedValue;

            await _context.SaveChangesAsync();
        }

        public async Task updateBadgeReactionDeletedAsync(ApplicationUser reacter, ApplicationUser reacted)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            reacter.Badge -= badgeValues.ReactionValue;
            reacted.Badge -= badgeValues.ReactionReceivedValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpJobApplicationAsync(ApplicationUser user)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            user.Experience += badgeValues.JobApplicationValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpJobApplicationDeletedAsync(ApplicationUser user)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            user.Experience -= badgeValues.JobApplicationValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpApplicationStepAsync(ApplicationUser user)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            user.Experience += badgeValues.ApplicationStepValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpApplicationStepDeletedAsync(ApplicationUser user)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            user.Experience -= badgeValues.ApplicationStepValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpApplicationStepProgressedAsync(ApplicationUser user)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            user.Experience += badgeValues.ApplicationStepProgressedValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpGotJobAsync(ApplicationUser user, int numSteps, bool GotIt)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            if (GotIt)
            {
                user.Experience += (int)(badgeValues.GotJobMultiplier * numSteps) + badgeValues.GotJobValue;
            }
            else
            {
                user.Experience += (int)(badgeValues.DidNotGetJobMultiplier * numSteps) + badgeValues.DidNotGetJobValue;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpGotJobDeletedAsync(ApplicationUser user, int numSteps, bool GotIt)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json) ?? throw new FileNotFoundException("badgeValues.json not found.");
            
            if (GotIt)
            {
                user.Experience -= (int)(badgeValues.GotJobMultiplier * numSteps) + badgeValues.GotJobValue;
            }
            else
            {
                user.Experience -= (int)(badgeValues.DidNotGetJobMultiplier * numSteps) + badgeValues.DidNotGetJobValue;
            }

            await _context.SaveChangesAsync();
        }
    }
}