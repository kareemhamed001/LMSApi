namespace BusinessLayer.Responses
{
    public class CourseResponse()
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public TeacherResponse Teacher { get; set; }
    }
}
