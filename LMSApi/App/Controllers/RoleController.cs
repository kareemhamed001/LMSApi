using AutoMapper;
using LMSApi.App.Responses;
using LMSApi.App.Requests;
using DataAccessLayer.Interfaces;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RolesController> _logger;
        private readonly IMapper _mapper;

        public RolesController(IRoleRepository roleRepository, ILogger<RolesController> logger, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllRoles()
        {
            try
            {
                var roles = await _roleRepository.GetAllRolesAsync();
                var roleResponses = _mapper.Map<List<RoleResponse>>(roles);
                return Ok(ApiResponseFactory.Create(roleResponses, "Roles fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching roles. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{roleId}")]
        public async Task<ActionResult<IApiResponse>> GetRoleById(int roleId)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                    return NotFound(ApiResponseFactory.Create("Role not found", 404, false));

                var roleResponse = _mapper.Map<RoleResponse>(role);
                return Ok(ApiResponseFactory.Create(roleResponse, "Role fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching role with id {roleId}. Log message: {logMessage}", roleId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<IApiResponse>> CreateRole([FromBody] CreateRoleRequest roleRequest)
        {
            try
            {
                var role = await _roleRepository.CreateRoleAsync(_mapper.Map<Role>(roleRequest));
                var roleResponse = _mapper.Map<RoleResponse>(role);
                return CreatedAtAction(nameof(GetRoleById), new { roleId = role.Id }, ApiResponseFactory.Create(roleResponse, "Role created successfully", 201, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while creating role. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPut]
        [Route("{roleId}")]
        public async Task<ActionResult<IApiResponse>> UpdateRole(int roleId, [FromBody] CreateRoleRequest roleRequest)
        {
            try
            {
                var existingRole = await _roleRepository.GetRoleByIdAsync(roleId);
                if (existingRole == null)
                    return NotFound(ApiResponseFactory.Create("Role not found", 404, false));

                await _roleRepository.UpdateRoleAsync(roleId, _mapper.Map<Role>(roleRequest));
                var updatedRole = await _roleRepository.GetRoleByIdAsync(roleId);
                var roleResponse = _mapper.Map<RoleResponse>(updatedRole);
                return Ok(ApiResponseFactory.Create(roleResponse, "Role updated successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating role with id {roleId}. Log message: {logMessage}", roleId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpDelete]
        [Route("{roleId}")]
        public async Task<ActionResult<IApiResponse>> DeleteRole(int roleId)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                    return NotFound(ApiResponseFactory.Create("Role not found", 404, false));

                await _roleRepository.DeleteRoleAsync(roleId);
                return Ok(ApiResponseFactory.Create("Role deleted successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting role with id {roleId}. Log message: {logMessage}", roleId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPost]
        [Route("assign")]
        public async Task<ActionResult<IApiResponse>> AssignRoleToUser([FromBody] AssignRoleRequest request)
        {
            try
            {
                var userRoleAssignmentResult = await _roleRepository.IsRoleAssignedToUserAsync(request.UserId, request.RoleId);
                if (!userRoleAssignmentResult)
                    return NotFound(ApiResponseFactory.Create("User or role not found", 404, false));

                return Ok(ApiResponseFactory.Create("Role assigned to user successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while assigning role to user. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

    }
}
