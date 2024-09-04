namespace DataAccessLayer.Interfaces
{
    public interface IPermissionRepository
    {
        public Task<Permission> GetPermissionByIdAsync(int id);
        public Task<List<Permission>> GetPermissionsByIdAsync(List<int> ids);
        public Task<IEnumerable<Permission>> GetAllPermissionsAsync();

    }
}
