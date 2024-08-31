using AutoMapper;
using LMSApi.App.Responses.Teacher;

namespace LMSApi.App.Responses
{
    public class CourseResponse()
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int TeacherId { get; set; }
    }
}
