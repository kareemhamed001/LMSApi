using DataAccessLayer.Entities;
using LMSApi.App.Responses;

namespace BusinessLayer.Interfaces
{
    public interface ICourseService
    {
        Task<CourseResponse> GetByIdAsync(int id);
        Task<IEnumerable<CourseResponse>> GetAllAsync();
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<bool> TeacherExistsAsync(int teacherId);
        Task<bool> SubjectExistsAsync(int subjectId);
        Task<bool> ClassExistsAsync(int classId);
    }
}
