
namespace BusinessLayer.Interfaces
{
    public interface ISubjectService
    {
        public Task<Subject?> Show(int id);
        public Task<IEnumerable<Subject>> Index();
        public Task<Subject> StoreAsync(SubjectRequest request);
        public Task<Subject> UpdateAsync(int id, UpdateSubjectRequest request);
        public Task DeleteAsync(int id);
        public Task<List<Teacher>> TeachersAsync(int subjectId);
        public Task<List<Student>> StudentsAsync(int subjectId);
        public Task<List<Class>> ClassesAsync(int subjectId);
        public Task<List<Course>> CoursesAsync(int subjectId);
        public Task<bool> AddSubjectToClass(AddSubjectToClassRequest request);
    }
}
