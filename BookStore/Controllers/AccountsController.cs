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
        public async Task<IActionResult> Register(RegisterDTO register)
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
        public async Task<IActionResult> Login(LoginDTO login)
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

        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("Roles/Remove")]
        public async Task<IActionResult> RemoveRole(AddToRoleDTO roleDTO)
        {
            if(ModelState.IsValid)
            {
                var response = await _authService.RemoveFromRoleAsync(roleDTO);
                if (response.isAuthenticated)
                    return Ok(response.Message);
                return BadRequest(response.Message);
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Roles/Add")]
        public async Task<IActionResult> AddRole(AddToRoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.AddToRoleAsync(roleDTO);
                if (response.isAuthenticated)
                    return Ok(response.Message);
                return BadRequest(response.Message);
            }
            return BadRequest(ModelState);
        }
    }
}
