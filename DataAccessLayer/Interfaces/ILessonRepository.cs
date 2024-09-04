namespace DataAccessLayer.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllLessonsAsync();
        Task<Lesson> CreateLessonAsync(Lesson lesson);
        Task<Lesson> UpdateLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(Lesson lesson);
        Task<bool> CourseExistsAsync(int courseId);
    }
}
