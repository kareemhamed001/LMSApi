namespace DataAccessLayer.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; } = new List<Permission>();
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public List<User> Users { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
