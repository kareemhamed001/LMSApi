
using DataAccessLayer.Data;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> CreateRoleAsync(Role roleRequest)
        {
            _context.Roles.Add(roleRequest);
            await _context.SaveChangesAsync();
            return roleRequest;
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
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

        public async Task AddRoleToUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (user != null && role != null)
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.Include(u => u.Roles)
                                           .FirstOrDefaultAsync(u => u.Id == userId);
            return user != null && user.Roles.Any(r => r.Id == roleId);
        }

        public async Task<bool> IsUserAssignedRoleAsync(int userId, int roleId)
        {
            return await IsRoleAssignedToUserAsync(userId, roleId); // Same check as above
        }
        public Role? GetRole(Func<Role, bool> condition)
        {
            return _context.Roles.Where(condition).FirstOrDefault();
        }
    }
}
