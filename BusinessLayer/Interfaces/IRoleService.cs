using DataAccessLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(Role roleRequest);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task UpdateRoleAsync(int roleId, Role roleRequest);
        Task DeleteRoleAsync(int roleId);
        Task AddRoleToUserAsync(int userId, int roleId);

        // New methods
        Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId);
        Task<bool> IsUserAssignedRoleAsync(int userId, int roleId);
    }
}
