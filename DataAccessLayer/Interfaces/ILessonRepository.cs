namespace DataAccessLayer.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllLessonsAsync();
        Task<Lesson> CreateLessonAsync(Lesson lesson);
        Task<Lesson> UpdateLessonAsync(int id, Lesson lesson);
        Task DeleteLessonAsync(int id);
        Task<bool> CourseExistsAsync(int courseId);
    }
}
