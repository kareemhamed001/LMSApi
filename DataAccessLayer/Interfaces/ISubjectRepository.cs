

namespace DataAccessLayer.Interfaces
{
    public interface ISubjectRepository
    {
        public Task<Subject?> Show(int id);
        public Task<IEnumerable<Subject>> Index();
        public Task<Subject> StoreAsync(Subject subject);
        public Task<Subject> UpdateAsync(Subject subject);
        public Task DeleteAsync(Subject subject);
        public Task<List<Teacher>> TeachersAsync(int subjectId);
        public Task<List<Student>> StudentsAsync(int subjectId);
        public Task<List<Class>> ClassesAsync(int subjectId);
        public Task<List<Course>> CoursesAsync(int subjectId);
        public Task<bool> AddSubjectToClass(Subject subject, Class @class);
    }
}
