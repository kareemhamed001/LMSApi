using LMSApi.App.Enums;

namespace LMSApi.Database.Enitities
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
