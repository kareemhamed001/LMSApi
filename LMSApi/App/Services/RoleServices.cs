using LMSApi.App.helper;
using LMSApi.App.Interfaces;
using LMSApi.App.Requests.Role;
using LMSApi.Database.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSApi.App.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> CreateRoleAsync(CreateRoleRequest roleRequest)
        {
            var role = new Role { Name = roleRequest.RoleName };
            foreach (var permissionId in roleRequest.Permissions)
            {
                var permission = await _context.Permissions.FindAsync(permissionId);
                if (permission != null)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id,
                        Role = role,
                        Permission = permission
                    };
                    role.RolePermissions.Add(rolePermission);
                }
            }
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }
        public async Task AddRoleToUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (user != null && role != null)
            {
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    User = user,
                    Role = role
                };

                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task UpdateRoleAsync(int roleId, CreateRoleRequest roleRequest)
        {
            var role = await _context.Roles
                                     .Include(r => r.RolePermissions)
                                     .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role != null)
            {
                role.Name = roleRequest.RoleName;

                // Find the permissions that need to be removed
                var currentPermissionIds = role.RolePermissions.ToList();
                _context.RolePermissions.RemoveRange(currentPermissionIds);
                                                           
                var permissionsToAdd = _context.Permissions.Where(p => roleRequest.Permissions.Contains(p.Id)).Select(p => p.Id).ToList();

                // Add new permissions
                foreach (var permissionId in permissionsToAdd)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permissionId
                    };
                    role.RolePermissions.Add(rolePermission);
                }

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SeedPermissions()
        {
            var permissions = PermissionsHelper.GetPermissions();
            foreach (var permission in permissions)
            {
                var existingPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.RouteName == permission.RouteName);
                if (existingPermission == null)
                {
                    _context.Permissions.Add(permission);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    existingPermission.Name = permission.Name;
                    existingPermission.Category = permission.Category;
                    existingPermission.RouteName = permission.RouteName;
                    _context.Permissions.Update(existingPermission);
                    await _context.SaveChangesAsync();
                }
            }
        }

    }
}
