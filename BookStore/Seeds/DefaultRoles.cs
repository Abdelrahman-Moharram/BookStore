using BookStore.Constants;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(IAuthService authService)
        {
            await authService.AddRole(Roles.SuperAdmin.ToString());
            await authService.AddRole(Roles.Admin.ToString());
            await authService.AddRole(Roles.Basic.ToString());
        }
    }
}
