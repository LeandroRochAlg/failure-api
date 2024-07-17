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
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json);

            if (badgeValues == null)
            {
                throw new FileNotFoundException("badgeValues.json not found.");
            }

            follower.Badge += badgeValues.FollowValue;
            followed.Badge += badgeValues.FollowedValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBadgeUnfollowAsync(ApplicationUser follower, ApplicationUser followed)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json);

            if (badgeValues == null)
            {
                throw new FileNotFoundException("badgeValues.json not found.");
            }

            follower.Badge -= badgeValues.FollowValue;
            followed.Badge -= badgeValues.FollowedValue;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateXpJobApplicationAsync(ApplicationUser user)
        {
            var json = File.ReadAllText("Resources/badgeValues.json");
            var badgeValues = JsonConvert.DeserializeObject<BadgeValuesModel>(json);

            if (badgeValues == null)
            {
                throw new FileNotFoundException("badgeValues.json not found.");
            }

            user.Experience += badgeValues.JobApplicationValue;

            await _context.SaveChangesAsync();
        }
    }
}