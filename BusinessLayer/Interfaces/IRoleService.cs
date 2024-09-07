using DataAccessLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(CreateRoleRequest roleRequest);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> UpdateRoleAsync(int roleId, CreateRoleRequest roleRequest);
        Task DeleteRoleAsync(int roleId);
        Task AddRoleToUserAsync(int userId, int roleId);

        // New methods
        Task<bool> IsRoleAssignedToUserAsync(int userId, int roleId);
        Task<bool> IsUserAssignedRoleAsync(int userId, int roleId);
    }
}
