namespace BusinessLayer.Interfaces
{
    public interface ILessonService
    {
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllLessonsAsync();
        Task<Lesson> CreateLessonAsync(Lesson lesson);
        Task<Lesson> UpdateLessonAsync(int id, Lesson lesson);
        Task DeleteLessonAsync(int id);
        Task<bool> CourseExistsAsync(int courseId);
    }
}
