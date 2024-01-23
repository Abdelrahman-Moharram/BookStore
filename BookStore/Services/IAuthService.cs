using BookStore.DTOs.Account;
using BookStore.DTOs.Response;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IAuthService
    {
        Task<BaseResponse> Register(RegisterDTO userDTO);
        Task<BaseResponse> Login(LoginDTO loginDTO);
        Task<ApplicationUser> AddUser(RegisterDTO userDTO);
        Task<BaseResponse> AddUserToRole(ApplicationUser user, string roleName);
        Task<BaseResponse> AddRole(string roleName);
        Task<BaseResponse> AddToRoleAsync(AddToRoleDTO addRole);
        Task<BaseResponse> RemoveFromRoleAsync(AddToRoleDTO addRole);
    }
}
