using LMSApi.App.Requests.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(CreateRoleRequest roleRequest);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task UpdateRoleAsync(int roleId, CreateRoleRequest roleRequest);
        Task DeleteRoleAsync(int roleId);
        Task AddRoleToUserAsync(int userId, int roleId);
        public Task SeedPermissions();
    }
}