
using ClassService = LMSApi.Database.Enitities.Class;
namespace LMSApi.App.Interfaces.Class
{
    public interface IClassService
    {
        Task<ClassService> GetClassByIdAsync(int id);
        Task<IEnumerable<ClassService>> GetAllClassesAsync();
        Task CreateClassAsync(ClassService classEntity);
        Task UpdateClassAsync(int id, ClassService classEntity);
        Task DeleteClassAsync(int id);
        Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
    }
}