using BookStore.Constants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.Services
{
    public static class RoleClaimsService
    {
        // هنا انا عايز اخلي رول السوبر ادمن تقدر تعمل اي حاجه في السيستم 
        // فهضيف الكليمز بتاعت كل موديول عندي في السيستم للرول بتاعت السوبر ادمن 
        public static async Task AddModuleCliamsForRole(this RoleManager<IdentityRole> _roleManager, string roleName, string module, string[] cruds)
        {
            var Role = await _roleManager.FindByNameAsync(roleName);
            if (Role == null)
            {
                throw new ArgumentException($"{roleName} is not found");
            }
            await _roleManager.AddPermissionClaims(Role, module, cruds);
        }


        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> _rolesManager, IdentityRole role, string module, string[] cruds)
        {
            var AllClaims = await _rolesManager.GetClaimsAsync(role);

            var allPermissions = Permissions.GeneratePermissionsList(module, cruds);

            foreach (var permission in allPermissions)
            {
                if (!AllClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                {
                    await _rolesManager.AddClaimAsync(role,new Claim("Permission", permission));
                }
            }
        }

    }
}
