
namespace DataAccessLayer.Repositories
{
    public class PermissionRepository(AppDbContext appDbContext) : IPermissionRepository
    {
        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await appDbContext.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(int id)
        {
            var permission = await appDbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id);

            return permission;
        }
        public async Task<List<Permission>> GetPermissionsByIdAsync(List<int> ids)
        {

            return await appDbContext.Permissions.Where(p => ids.Contains(p.Id)).ToListAsync();
        }
    }
}
