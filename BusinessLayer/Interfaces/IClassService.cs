using DataAccessLayer.Entities;


namespace BusinessLayer.Interfaces
{
    public interface IClassService
    {
        public Task<Class?> GetClassByIdAsync(int id);
        public Task<IEnumerable<Class>> GetAllClassesAsync();
        public Task<Class> CreateClassAsync(ClassRequest classRequest);
        public Task<Class> UpdateClassAsync(int id, ClassRequest classRequest);
        public Task<bool> DeleteClassAsync(int id);
        public Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
        public Task<IEnumerable<Course>> GetCoursesByClassIdAsync(int classId);
        public Task<IEnumerable<Teacher>> GetTeachersByClassIdAsync(int classId);
    }
}