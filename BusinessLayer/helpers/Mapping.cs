using AutoMapper;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
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
            CreateMap<CreateRoleRequest, Role>();

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

            CreateMap<Lesson, LessonResponse>();
            CreateMap<LessonContent, LessonContentResponse>();
        }

    }
}