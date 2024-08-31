using LMSApi.App.Enums;

namespace LMSApi.App.Requests
{
    public class LessonContentRequest
    {
        public string Name { get; set; }
        public LessonContentTypesEnum Type { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
    }
}
