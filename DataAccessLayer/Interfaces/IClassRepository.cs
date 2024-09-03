
namespace DataAccessLayer.Interfaces
{
    public interface IClassRepository
    {
        Task<Class> GetClassByIdAsync(int id);
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class> CreateClassAsync(Class classEntity);
        Task<Class> UpdateClassAsync(Class classEntity);
        Task<bool> DeleteClassAsync(Class classEntity);
        Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
        Task<IEnumerable<Course>> GetCoursesByClassIdAsync(int classId);
        Task<IEnumerable<Teacher>> GetTeachersByClassIdAsync(int classId);
    }
}