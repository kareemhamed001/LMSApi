﻿using AutoMapper;

namespace testApp.helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Class, ClassRequest>();
            CreateMap<ClassTranslationRequest, ClassTranslation>();
            CreateMap<Class, ClassResponse>();
            CreateMap<ClassResponse, Class>();
            CreateMap<ClassRequest, Class>();
            CreateMap<Class, GetStudentOfClassRequest>();
            CreateMap<GetStudentOfClassRequest, Class>();
            CreateMap<Course, CourseRequest>();
            CreateMap<CourseRequest, Course>();
            CreateMap<Course, CourseResponse>();
            CreateMap<Subscription, SubscriptionResponse>();
            CreateMap<CourseResponse, Course>();
            CreateMap<Lesson, LessonRequest>();
            CreateMap<LessonRequest, Lesson>();
            CreateMap<SubjectResponse, Subject>();
            CreateMap<Subject, SubjectResponse>();
            CreateMap<Teacher, TeacherResponse>();
            CreateMap<TeacherResponse, Teacher>();
            CreateMap<UserRequest, User>();
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleResponse>();
            CreateMap<Permission, PermissionResponse>();
            CreateMap<Teacher, TeacherResponse>();
            CreateMap<Teacher, ShowTeacherResponse>();
            CreateMap<User, UserResponse>();
            CreateMap<LessonContentRequest, LessonContent>();
            CreateMap<LessonContent, LessonContentRequest>();
            //new
            CreateMap<ClassTranslation, ClassTranslationRequest>();
            CreateMap<ClassTranslationRequest, ClassTranslation>();
        }

    }
}