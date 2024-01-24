using BookStore.DTOs.Account;
using BookStore.DTOs.Response;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IRoleService
    {
        Task<BaseResponse> AddUserToRole(ApplicationUser user, string roleName);


        Task<BaseResponse> AddToRoleAsync(AddToRoleDTO addRole);
        Task<BaseResponse> RemoveFromRoleAsync(AddToRoleDTO addRole);


        Task<BaseResponse> AddRole(string roleName);
        Task<BaseResponse> RemoveRole(string roleName);

        Task<List<string>> GetRoleClaimsPermissions(string roleId);
        Task<List<string>> AllRoles();
        Task<List<string>> EditRoleClaimsPermissions(RolePermissionsDTO permissionsDTO);
    }
}
