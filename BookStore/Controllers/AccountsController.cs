using BookStore.DTOs.Account;
using BookStore.DTOs.Response;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService authService;
        public AccountsController(IAuthService _authService)
        {
            authService = _authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                BaseResponse result = await authService.Register(register);
                if (result.isAuthenticated)
                    return Ok(result);
                return Unauthorized(result.Message);
            }
            return BadRequest(register);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                BaseResponse result = await authService.Login(login);
                if (result.isAuthenticated)
                    return Ok(result);
                return Unauthorized(result.Message);
            }
            return BadRequest(login);
        }
    }
}
