using AutoMapper;
using DataAccessLayer.Entities;
using LMSApi.App.Requests;
using LMSApi.App.Responses;

namespace BusinessLayer.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            //user mapping
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();

            //teacher mapping
            CreateMap<CreateTeacherRequest, Teacher>();
            CreateMap<Teacher, TeacherResponse>();
            CreateMap<Teacher, ShowTeacherResponse>();


            //role mapping
            CreateMap<Role, RoleResponse>();

            //permission mapping
            CreateMap<Permission, PermissionResponse>();

            //class mapping
            CreateMap<ClassRequest, Class>();
            CreateMap<Class, ClassResponse>();
            CreateMap<GetStudentOfClassRequest, Class>();
            CreateMap<ClassTranslationRequest, ClassTranslation>();
            CreateMap<ClassTranslation, ClassTranslationResponse>();

            //subject mapping
            CreateMap<SubjectRequest, Subject>();
            CreateMap<Subject, SubjectResponse>();

            //course mapping
            CreateMap<CourseRequest, Course>();
            CreateMap<Course, CourseResponse>();

            //lesson mapping
            CreateMap<LessonRequest, Lesson>();

            //lesson content mapping
            CreateMap<LessonContentRequest, LessonContent>();
            CreateMap<LessonContent, LessonContentRequest>();

            //subscription mapping
            CreateMap<Subscription, SubscriptionResponse>();
        }

    }
}