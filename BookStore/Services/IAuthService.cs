using BookStore.DTOs.Account;
using BookStore.DTOs.Response;

namespace BookStore.Services
{
    public interface IAuthService
    {
        Task<BaseResponse> Register(RegisterDTO userDTO);
        Task<BaseResponse> Login(LoginDTO loginDTO);
    }
}
