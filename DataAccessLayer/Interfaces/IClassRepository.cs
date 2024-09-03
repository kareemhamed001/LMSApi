
namespace DataAccessLayer.Interfaces
{
    public interface IClassRepository
    {
        Task<Class> GetClassByIdAsync(int id);
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class> CreateClassAsync(Class classEntity);
        Task<Class> UpdateClassAsync(int id, Class classEntity);
        Task<bool> DeleteClassAsync(int id);
        Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
        Task<IEnumerable<Course>> GetCoursesByClassIdAsync(int classId);
        Task<IEnumerable<Teacher>> GetTeachersByClassIdAsync(int classId);
    }
}