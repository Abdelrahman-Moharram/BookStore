using BookStore.Constants;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(IRoleService roleService)
        {
            await roleService.AddRole(Roles.SuperAdmin.ToString());
            await roleService.AddRole(Roles.Admin.ToString());
            await roleService.AddRole(Roles.Basic.ToString());
        }
    }
}
