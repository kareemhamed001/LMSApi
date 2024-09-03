﻿using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace BusinessLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;

        public RoleService(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;

        }

        public async Task<Role> CreateRoleAsync(CreateRoleRequest roleRequest)
        {
            var role = new Role { Name = roleRequest.Name };

            var permissions = await permissionRepository.GetPermissionsByIdAsync(roleRequest.Permissions);
            foreach (var permissionId in roleRequest.Permissions)
            {
                var permission = permissions.FirstOrDefault(p => p.Id == permissionId);
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
            role = await roleRepository.CreateRoleAsync(role);

            return role;
        }
        public async Task AddRoleToUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await roleRepository.GetRoleByIdAsync(roleId);

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
                role.Name = roleRequest.Name;

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
            //PermissionSeeder permissionSeeder = new PermissionSeeder(_context);
            //await permissionSeeder.Seed();
            return;
        }

    }
}