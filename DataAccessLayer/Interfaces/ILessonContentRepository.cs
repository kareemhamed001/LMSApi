namespace DataAccessLayer.Interfaces
{
    public interface ILessonContentRepository
    {
        Task<LessonContent> GetByIdAsync(int id);
        Task<IEnumerable<LessonContent>> GetAllAsync();
        Task<LessonContent> CreateAsync(LessonContent lessonContent);
        Task<LessonContent> UpdateAsync(int id, LessonContent lessonContent);
        Task DeleteAsync(int id);
    }
}
