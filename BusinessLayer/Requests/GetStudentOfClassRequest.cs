namespace BusinessLayer.Requests
{
    public class GetStudentOfClassRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }

    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}