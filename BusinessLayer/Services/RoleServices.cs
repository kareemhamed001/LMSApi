using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<Role> CreateRoleAsync(CreateRoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                _logger.LogError("Role request is null.");
                throw new ArgumentNullException(nameof(roleRequest), "Role request cannot be null.");
            }

            try
            {
                List<RolePermission> rolPermissions = new List<RolePermission>();
                foreach (var permission in roleRequest.Permissions)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        PermissionId = permission,
                    };
                    rolPermissions.Add(rolePermission);
                }

                Role role = new Role
                {
                    Name = roleRequest.Name,
                    RolePermissions = rolPermissions
                };

                return await _roleRepository.CreateRoleAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating role.");
                throw; // Re-throw the exception to be handled by higher-level code if needed
            }
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleId);

                if (role == null)
                {
                    _logger.LogWarning("Role with ID {RoleId} not found.", roleId);
                }

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving role.");
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all roles.");
                return await _roleRepository.GetAllRolesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all roles.");
                throw;
            }
        }

        public async Task<Role> UpdateRoleAsync(int roleId, CreateRoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                _logger.LogError("Role request is null.");
                throw new ArgumentNullException(nameof(roleRequest), "Role request cannot be null.");
            }

            try
            {
                var existingRole = await _roleRepository.GetRoleByIdAsync(roleId);
                if (existingRole == null)
                {
                    throw new NotFoundException($"Role with ID {roleId} not found.");
                }
                List<RolePermission> rolPermissions = new List<RolePermission>();
                foreach (var permission in roleRequest.Permissions)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        PermissionId = permission,
                    };
                    rolPermissions.Add(rolePermission);
                }

                existingRole.Name = roleRequest.Name;
                existingRole.RolePermissions = rolPermissions;


               return await _roleRepository.UpdateRoleAsync(existingRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role with ID {RoleId}.", roleId);
                throw;
            }
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            try
            {
                _logger.LogInformation("Checking if role with ID {RoleId} exists for deletion.", roleId);
                var existingRole = await _roleRepository.GetRoleByIdAsync(roleId);
                if (existingRole == null)
                {
                    _logger.LogWarning("Role with ID {RoleId} not found for deletion.", roleId);
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                }

                _logger.LogInformation("Deleting role with ID: {RoleId}.", roleId);
                await _roleRepository.DeleteRoleAsync(roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role with ID {RoleId}.", roleId);
                throw;
            }
        }

        public async Task AddRoleToUserAsync(int userId, int roleId)
        {
            try
            {
                _logger.LogInformation("Checking if user with ID {UserId} and role with ID {RoleId} exist for assignment.", userId, roleId);

                var userExists = await _roleRepository.GetRoleByIdAsync(roleId) != null;
                var roleExists = await _roleRepository.GetRoleByIdAsync(roleId) != null;

                if (!userExists)
                {
                    _logger.LogWarning("User with ID {UserId} not found for role assignment.", userId);
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                if (!roleExists)
                {
                    _logger.LogWarning("Role with ID {RoleId} not found for role assignment.", roleId);
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                }

                _logger.LogInformation("Adding role with ID {RoleId} to user with ID {UserId}.", roleId, userId);
                await _roleRepository.AddRoleToUserAsync(userId, roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding role with ID {RoleId} to user with ID {UserId}.", roleId, userId);
                throw;
            }
        }

        public async Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId)
        {
            try
            {
                _logger.LogInformation("Checking if role with ID {RoleId} is assigned to user with ID {UserId}.", roleId, userId);
                return await _roleRepository.IsRoleAssignedToUserAsync(userId, roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if role with ID {RoleId} is assigned to user with ID {UserId}.", roleId, userId);
                throw;
            }
        }

        public async Task<bool> IsUserAssignedRoleAsync(int userId, int roleId)
        {
            try
            {
                _logger.LogInformation("Checking if user with ID {UserId} has role with ID {RoleId}.", userId, roleId);
                return await _roleRepository.IsUserAssignedRoleAsync(userId, roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if user with ID {UserId} has role with ID {RoleId}.", userId, roleId);
                throw;
            }
        }
    }
}
