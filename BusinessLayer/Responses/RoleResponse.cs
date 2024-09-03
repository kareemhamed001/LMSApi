namespace BusinessLayer.Responses
{
    public class RoleResponse
    {
        public string Name { get; set; }
        public List<PermissionResponse> Permissions { get; set; }
    }
}
