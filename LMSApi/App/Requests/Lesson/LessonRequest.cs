namespace LMSApi.App.Requests.Lesson
{
    public class LessonRequest: LessonRequestBase
    {
        public int CourseId { get; set; }
        public int? SectionNumber { get; set; }
    }
}
