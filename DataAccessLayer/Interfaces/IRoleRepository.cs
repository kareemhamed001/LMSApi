namespace DataAccessLayer.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateRoleAsync(Role roleRequest);
        Task<Role> GetRoleByIdAsync(int roleId);
        public Role? GetRole(Func<Role, bool> condition);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
        Task AddRoleToUserAsync(int userId, int roleId);

        // New methods
        Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId);
        Task<bool> IsUserAssignedRoleAsync(int userId, int roleId);
    }
}
