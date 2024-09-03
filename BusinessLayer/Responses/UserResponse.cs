namespace BusinessLayer.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public List<RoleResponse> Roles { get; set; } = new List<RoleResponse>();

    }
}
