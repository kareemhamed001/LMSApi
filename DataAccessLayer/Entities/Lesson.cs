namespace DataAccessLayer.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? SectionNumber { get; set; }

        public List<LessonContent> LessonContents { get; set; }
    }
}
