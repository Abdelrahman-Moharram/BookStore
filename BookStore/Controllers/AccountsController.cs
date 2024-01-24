using BookStore.Constants;
using BookStore.DTOs.Account;
using BookStore.DTOs.Response;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                BaseResponse result = await _authService.Register(register);
                if (result.isAuthenticated)
                    return Ok(result);
                return Unauthorized(result.Message);
            }
            return BadRequest(register);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                BaseResponse result = await _authService.Login(login);
                if (result.isAuthenticated)
                    return Ok(result);
                return Unauthorized(result.Message);
            }
            return BadRequest(login);
        }

        
    }
}
