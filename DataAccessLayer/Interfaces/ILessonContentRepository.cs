namespace DataAccessLayer.Interfaces
{
    public interface ILessonContentRepository
    {
        Task<LessonContent> GetByIdAsync(int id);
        Task<IEnumerable<LessonContent>> GetAllAsync();
        Task<LessonContent> CreateAsync(LessonContent lessonContent);
        Task<LessonContent> UpdateAsync(LessonContent lessonContent);
        Task DeleteAsync(LessonContent lessonContent);
    }
}
