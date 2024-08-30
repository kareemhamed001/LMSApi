﻿using LMSApi.App.Requests.LessonContent;

namespace LMSApi.App.Interfaces
{
    public interface ILessonContentService
    {
        Task<LessonContent> GetByIdAsync(int id);
        Task<IEnumerable<LessonContent>> GetAllAsync();
        Task<LessonContent> CreateAsync(LessonContentRequest lessonContentDto);
        Task<LessonContent> UpdateAsync(int id, LessonContentRequest lessonContentDto);
        Task DeleteAsync(int id);
    }
}
