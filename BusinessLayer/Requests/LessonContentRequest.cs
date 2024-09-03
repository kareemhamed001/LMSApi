

using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Requests
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
