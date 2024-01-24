using BookStore.Controllers;
using BookStore.DTOs.Account;
using BookStore.DTOs.Response;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BookStore.Services
{
    public class RoleService:IRoleService
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountsController> _logger;
        private readonly JWTSettings _jwt;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(
            UserManager<ApplicationUser> userManager,
            IOptions<JWTSettings> jwt,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountsController> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _jwt = jwt.Value;


        }

        public async Task<BaseResponse> AddUserToRole(ApplicationUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new BaseResponse { Message = "Add To Role Successfully !" };
            }
            _logger.LogError("Something went wrong Add User To Role");
            return new BaseResponse { Message = "Something went wrong !" };
        }

        public async Task<BaseResponse> AddRole(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) != null)
                return new BaseResponse { Message = $"Role {roleName} Already Exists" };

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
                return new BaseResponse { Message = "Role Added Successfully !", isAuthenticated = true };
            _logger.LogError("Something went wrong Add Role");
            return new BaseResponse { Message = "something went wrong" };
        }

        public async Task<BaseResponse> RemoveRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return new BaseResponse { Message = $"Role {roleName} not found" };

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return new BaseResponse { Message = $"Role {roleName} Removed Successfully !", isAuthenticated = true };
            _logger.LogError($"Something went wrong while Adding {roleName} Role");
            return new BaseResponse { Message = "something went wrong" };
        }


        public async Task<BaseResponse> AddToRoleAsync(AddToRoleDTO addRole)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(addRole.userId);

            IdentityRole role = await _roleManager.FindByNameAsync(addRole.roleName);

            if (user != null && role != null)
            {
                if (await _userManager.IsInRoleAsync(user, addRole.roleName))
                    return new BaseResponse { Message = "User already assigned to this role" };

                return await AddUserToRole(user, addRole.roleName);
            }
            return new BaseResponse { Message = "Invalid user or Role" };
        }
        public async Task<BaseResponse> RemoveFromRoleAsync(AddToRoleDTO addRole)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(addRole.userId);

            IdentityRole role = await _roleManager.FindByNameAsync(addRole.roleName);

            if (user != null && role != null)
            {
                if (!await _userManager.IsInRoleAsync(user, addRole.roleName))
                    return new BaseResponse { Message = $"{user.UserName} is not  assigned to {addRole.roleName} role" };

                var result = await _userManager.RemoveFromRoleAsync(user, addRole.roleName);
                if (result.Succeeded)
                {
                    return new BaseResponse { Message = $"{user.UserName} removed from {addRole.roleName} role Successfully !", isAuthenticated = true };
                }
            }
            return new BaseResponse { Message = "Invalid user or Role" };
        }

        public async Task<List<string>> GetRoleClaimsPermissions(string roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role == null)
                return null;
            return _roleManager.GetClaimsAsync(role).Result.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
        }
        public async Task<List<string>> EditRoleClaimsPermissions(RolePermissionsDTO permissionsDTO)
        {
            var role = _roleManager.FindByIdAsync(permissionsDTO.RoleId).Result;
            if (role == null)
                return null;
            var per = permissionsDTO.Permissions;
            foreach (var claim in _roleManager.GetClaimsAsync(role).Result)
            {
                if (permissionsDTO.Permissions.FirstOrDefault(p => p.ToLower() == claim.Value.ToLower()) == null)
                    await _roleManager.RemoveClaimAsync(role, claim);
                else
                    permissionsDTO.Permissions.Remove(claim.Value);
            }
            per = permissionsDTO.Permissions;
            foreach (var permission in permissionsDTO.Permissions)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));


            return _roleManager.GetClaimsAsync(role).Result.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
        }

        public async Task<List<string>> AllRoles()
        {
            return await _roleManager.Roles.Select(i => i.Name).ToListAsync();
        }

    }
}
