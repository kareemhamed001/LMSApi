using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSApi.App.Interfaces
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(string roleName);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task UpdateRoleAsync(int roleId, string newRoleName);
        Task DeleteRoleAsync(int roleId);
        Task AddRoleToUserAsync(int userId, int roleId);
    }
}