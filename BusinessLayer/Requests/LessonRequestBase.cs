
namespace BusinessLayer.Requests
{
    public class LessonRequestBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SectionNumber { get; set; }
        public List<LessonContentRequest> LessonContents { get; set; } = new List<LessonContentRequest>();

    }
}
