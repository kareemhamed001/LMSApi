﻿namespace LMSApi.App.Requests
{
    public class LessonRequest: LessonRequestBase
    {
        public int CourseId { get; set; }
        public int? SectionNumber { get; set; }
    }
}