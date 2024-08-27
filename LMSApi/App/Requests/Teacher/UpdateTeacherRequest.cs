namespace LMSApi.App.Requests.Teacher
{
    public class UpdateTeacherRequest
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;        
        public string? Email { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? NickName { get; set; }
        public string? CommunicationEmail { get; set; }
        public string? CommunicationPhone { get; set; }

    }
}
