
using LMSApi.App.Requests;

using AutoMapper;
using DataAccessLayer.Exceptions;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;
        private readonly IMapper _mapper;

        public RolesController(IRoleService _roleService, ILogger<RolesController> logger, IMapper mapper)
        {
            this._roleService = _roleService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                var roleResponses = _mapper.Map<List<RoleResponse>>(roles);
                return Ok(ApiResponseFactory.Create(roleResponses, "Roles fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching roles. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{roleId}")]
        public async Task<ActionResult<IApiResponse>> GetRoleById(int roleId)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(roleId);

                var roleResponse = _mapper.Map<RoleResponse>(role);
                return Ok(ApiResponseFactory.Create(roleResponse, "Role fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching role with id {roleId}. Log message: {logMessage}", roleId, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseSingleStrategy>> CreateRole([FromBody] CreateRoleRequest roleRequest)
        {
            try
            {
                var role = await _roleService.CreateRoleAsync(roleRequest);
                var roleResponse = _mapper.Map<RoleResponse>(role);
                return Ok(ApiResponseFactory.Create(roleResponse, "Role created successfully", 201, true));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponseFactory.Create(ex.Message, 400, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while creating role. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{roleId}")]
        public async Task<ActionResult<IApiResponse>> UpdateRole(int roleId, [FromBody] CreateRoleRequest roleRequest)
        {
            try
            {
                var existingRole = await _roleService.GetRoleByIdAsync(roleId);
        
                var updatedRole = await _roleService.UpdateRoleAsync(roleId, roleRequest);
                var roleResponse = _mapper.Map<RoleResponse>(updatedRole);

                return Ok(ApiResponseFactory.Create(roleResponse, "Role updated successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating role with id {roleId}. Log message: {logMessage}", roleId, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{roleId}")]
        public async Task<ActionResult<IApiResponse>> DeleteRole(int roleId)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(roleId);
              
                await _roleService.DeleteRoleAsync(roleId);
                return Ok(ApiResponseFactory.Create("Role deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting role with id {roleId}. Log message: {logMessage}", roleId, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("assign")]
        public async Task<ActionResult<IApiResponse>> AssignRoleToUser([FromBody] AssignRoleRequest request)
        {
            try
            {
                var userRoleAssignmentResult = await _roleService.IsRoleAssignedToUserAsync(request.UserId, request.RoleId);
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
