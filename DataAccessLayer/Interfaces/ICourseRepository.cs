namespace DataAccessLayer.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(Course course);
        Task<bool> TeacherExistsAsync(int teacherId);
        Task<bool> SubjectExistsAsync(int subjectId);
        Task<bool> ClassExistsAsync(int classId);
    }
}
