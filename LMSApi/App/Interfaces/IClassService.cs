
namespace LMSApi.App.Interfaces
{
    public interface IClassService
    {
        Task<Class> GetClassByIdAsync(int id);
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task CreateClassAsync(Class classEntity);
        Task<Class> UpdateClassAsync(int id, Class classEntity);
        Task DeleteClassAsync(int id);
        Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
        Task<IEnumerable<Course>> GetCoursesByClassIdAsync(int classId);
        Task<IEnumerable<Teacher>> GetTeachersByClassIdAsync(int classId);
    }
}