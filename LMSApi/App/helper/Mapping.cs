using AutoMapper;
using LMSApi.App.DTOs;
using LMSApi.App.Requests.Class;
using LMSApi.App.Requests.Course;
using LMSApi.App.Requests.Lesson;
using LMSApi.App.Requests.LessonContent;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace testApp.helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Class, ClassRequest>();
            CreateMap<ClassRequest, Class>();
            CreateMap<Class, GetStudentOfClassRequest>();
            CreateMap<GetStudentOfClassRequest, Class>();
            CreateMap<Course, CourseRequest>();
            CreateMap<CourseRequest, Course>();
            CreateMap<Lesson, LessonRequest>();
            CreateMap<LessonRequest, Lesson>();
            CreateMap<LessonContentRequest, LessonContent>();
            CreateMap<LessonContent, LessonContentRequest>();
        }

    }
}