using BookStore.Constants;
using BookStore.DTOs.Account;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        // List of roles

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("")]
        public async Task<IActionResult> AllRoles()
        {
            return Ok(await _roleService.AllRoles());
        }

        

        // add new role
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddRole([FromBody] RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.AddRole(roleDTO.roleName);
                if (response.isAuthenticated)
                    return Ok(response.Message);
                return BadRequest(response.Message);
            }
            return BadRequest(ModelState);
        }



        // remove role

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.RemoveRole(roleDTO.roleName);
                if (response.isAuthenticated)
                    return Ok(response.Message);
                return BadRequest(response.Message);
            }
            return BadRequest(ModelState);
        }


        // Get Role Permissions
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("{roleId}/Permissions")]
        public async Task<IActionResult> GetRolePermissions(string roleId)
        {
            var Claims = await _roleService.GetRoleClaimsPermissions(roleId);
            if (Claims == null)
                return NotFound("Role not found !");
            return Ok(Claims);
        }

        // Edit Role Permissions
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}/Permissions/Edit")]
        public async Task<IActionResult> EditRolePermissions([FromBody] RolePermissionsDTO permissionsDTO, string id)
        {
            if (id != permissionsDTO.RoleId)
                return BadRequest();

            var Claims = await _roleService.EditRoleClaimsPermissions(permissionsDTO);
            if (Claims == null)
                return NotFound("Role not found !");

            return Ok(Claims);
        }



        

        // add Add User to Role
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Users/Add")]
        public async Task<IActionResult> AddUserRole([FromBody] AddToRoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.AddToRoleAsync(roleDTO);
                if (response.isAuthenticated)
                    return Ok(response.Message);
                return BadRequest(response.Message);
            }
            return BadRequest(ModelState);
        }

        // Remove User from Role
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Users/Remove")]
        public async Task<IActionResult> RemoveUserRole([FromBody] AddToRoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.RemoveFromRoleAsync(roleDTO);
                if (response.isAuthenticated)
                    return Ok(response.Message);
                return BadRequest(response.Message);
            }
            return BadRequest(ModelState);
        }

        // all system permissions
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("/api/Permissions")]
        public Task<IActionResult> PermissionsList()
        {
            return Task.FromResult<IActionResult>(Ok(Task.Run(()=> Permissions.AllPermissionsList())));
        }

    }
}
