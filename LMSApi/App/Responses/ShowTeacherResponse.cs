﻿using LMSApi.App.Responses.Teacher;
using LMSApi.Database.Enitities;
namespace LMSApi.App.Responses
{
    public class ShowTeacherResponse: TeacherResponse
    {
        public List<CourseResponse> Courses { get; set; }= new List<CourseResponse>();
        public List<SubjectResponse> Subjects { get; set; } = new List<SubjectResponse>();
        public List<SubscriptionResponse> Subscriptions { get; set; }=new List<SubscriptionResponse>();


    }
}
