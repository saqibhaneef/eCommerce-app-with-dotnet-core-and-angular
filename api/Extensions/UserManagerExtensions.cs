using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Extensions
{
    public static class UserManagerExtensions
    {

        public static async Task<AppUser> FindUserByClaimPrincipleWithAddress(this UserManager<AppUser> userManager,ClaimsPrincipal user)
        {
            var email=user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users.Include(x=>x.Address).SingleOrDefaultAsync(x=>x.Email==email);
        }
        public static async Task<AppUser> FindByEmailFromClaimPrinciples(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users.SingleOrDefaultAsync(x=>x.Email==user.FindFirstValue(ClaimTypes.Email));
        }
    }
}
