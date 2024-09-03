namespace BusinessLayer.Responses
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public string ParentPhone { get; set; }
        public UserResponse User { get; set; }
        public ClassResponse Class { get; set; }
    }
}
