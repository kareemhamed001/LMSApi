namespace LMSApi.App.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<bool> TeacherExistsAsync(int teacherId);
        Task<bool> SubjectExistsAsync(int subjectId);
        Task<bool> ClassExistsAsync(int classId);
    }
}
