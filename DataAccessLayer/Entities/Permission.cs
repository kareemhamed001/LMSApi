namespace DataAccessLayer.Entities  
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string RouteName { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
