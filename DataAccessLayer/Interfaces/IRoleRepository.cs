﻿

namespace DataAccessLayer.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateRoleAsync(Role roleRequest);
        Task<Role> GetRoleByIdAsync(int roleId);
        public Role? GetRole(Func<Role, bool> condition);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task UpdateRoleAsync(int roleId, Role roleRequest);
        Task DeleteRoleAsync(int roleId);
        Task AddRoleToUserAsync(int userId, int roleId);
        public Task SeedPermissions();
    }
}