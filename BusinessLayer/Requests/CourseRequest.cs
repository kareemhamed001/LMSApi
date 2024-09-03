namespace BusinessLayer.Requests
{
    public class CourseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TeacherId { get; set; } // Nullable
        public int SubjectId { get; set; } // Nullable
        public decimal Price { get; set; }
        public int ClassId { get; set; } // Nullable
    }
}
