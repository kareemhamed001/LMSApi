
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities
{
    public class LessonContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LessonContentTypesEnum Type { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
