namespace BusinessLayer.Interfaces
{
    public interface ITeacherService
    {

        public Task< List<Teacher>> Index();
        public Task<Teacher> Store(CreateTeacherRequest teacherRequest);
        public Task<Teacher> Update(int teacherId, UpdateTeacherRequest teacherRequest);
        public Task<Teacher> Show(int teacherId);
        public Task<bool> Delete(int teacherId);
        public Task<List<Course>> CoursesAsync(int teacherId);
        public Task<List<Subject>> SubjectsAsync(int teacherId);
        public Task<List<Subscription>> SubscriptionsAsync(int teacherId);
        public Task<List<Class>> ClassesAsync(int teacherId);
        public Task<Course> StoreCourseAsync(int loggedUserId,StoreCourseRequest storeCourseRequest);
        //public Task<Subject> AddTeacherToSubjectAsync(int loggedUserId,AddTeacherToSubjectRequest storeCourseRequest);
    }
}
