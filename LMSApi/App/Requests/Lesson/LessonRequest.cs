namespace LMSApi.App.Requests.Lesson
{
    public class LessonRequest
    {
        public String Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public int? SectionNumber { get; set; }
    }
}
