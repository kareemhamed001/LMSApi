namespace BusinessLayer.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> AddAsync(CourseRequest course);
        Task UpdateAsync(CourseRequest course);
        Task DeleteAsync(int id);
        Task<bool> TeacherExistsAsync(int teacherId);
        Task<bool> SubjectExistsAsync(int subjectId);
        Task<bool> ClassExistsAsync(int classId);
    }
}
