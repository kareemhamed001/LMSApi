using LMSApi.App.Enums;

namespace LMSApi.App.Requests.LessonContent
{
    public class LessonContentRequest
    {
        public string Name { get; set; }
        public LessonContentTypesEnum Type { get; set; }
        public IFormFile Link { get; set; } = default!;
        public string Content { get; set; }
        public int LessonId { get; set; }
    }
}
