namespace DataAccessLayer.Interfaces
{
    public interface ITeacherRepository
    {

        public Task<List<Teacher>> Index();
        public Task<Teacher> Store(Teacher teacherRequest);
        public Task<Teacher> Update( Teacher teacherRequest);
        public Task<Teacher> Show(int teacherId);
        public Teacher? GetTeacher(Func<Teacher, bool> condition);
        public Task<bool> Delete(Teacher teacher);
        public Task<List<Course>> CoursesAsync(int teacherId);
        public Task<List<Subject>> SubjectsAsync(int teacherId);
        public Task<List<Subscription>> SubscriptionsAsync(int teacherId);
        public Task<List<Class>> ClassesAsync(int teacherId);
        public Task<Course> StoreCourseAsync(int loggedUserId, Course storeCourseRequest);
        public Task<Subject> AddTeacherToSubjectAsync(int loggedUserId, int teacherId, int subjectId);
    }
}
