using LMSApi.App.Interfaces;
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

        public async Task<Role> CreateRoleAsync(string roleName)
        {
            var role = new Role { Name = roleName };
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

        public async Task UpdateRoleAsync(int roleId, string newRoleName)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                role.Name = newRoleName;
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
    }
}
