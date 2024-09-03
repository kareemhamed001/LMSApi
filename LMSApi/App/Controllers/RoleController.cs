using LMSApi.App.Interfaces;
using LMSApi.App.Requests;
using LMSApi.App.Requests;
using LMSApi.App.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSApi.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly AppDbContext _appDbContext;
        public RoleController(IRoleService roleService, AppDbContext appDbContext)
        {
            _roleService = roleService;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ApiResponse<Role>>> CreateRole([FromBody] CreateRoleRequest request)
        {
            var role = await _roleService.CreateRoleAsync(request);
            return Ok(new ApiResponse<Role>
            {

                Message = "Role created successfully",
                Success = true,
                Status = 201
            });
        }

        [HttpGet]
        [Route("{roleId}")]
        public async Task<ActionResult<ApiResponse<Role>>> GetRole(int roleId)
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role == null)
                return NotFound("Role not found");

            return Ok(role);
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Role>>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPut]
        [Route("{roleId}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateRole(int roleId, [FromBody] CreateRoleRequest roleRequest)
        {
            await _roleService.UpdateRoleAsync(roleId, roleRequest);
            return Ok(new ApiResponse<string>
            {

                Message = "Role updated successfully",
                Success = true,
                Status = 200
            });
        }

        [HttpDelete]
        [Route("{roleId}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRole(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Ok(new ApiResponse<string>
            {

                Message = "Role deleted successfully",
                Success = true,
                Status = 200
            });
        }
        [HttpPost]
        [Route("assign-role")]
        public async Task<ActionResult<ApiResponse<string>>> AssignRoleToUser(int userId, int roleId)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role == null)
                return NotFound("Role not found");

            await _roleService.AddRoleToUserAsync(userId, roleId);

            return Ok("Role assigned successfully");
        }

        [HttpGet]
        [Route("seed-permissions")]
        public async Task<ActionResult> seedPermissionsSeedPermissions()
        {
            await _roleService.SeedPermissions();
            return Ok("Permissions seeded successfully");
        }
    }
}
