namespace BusinessLayer.Responses
{
    public class TeacherResponse
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public UserResponse User { get; set; }
    }
}
