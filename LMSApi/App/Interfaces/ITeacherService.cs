using LMSApi.App.Enums;
using LMSApi.App.Requests;
using LMSApi.App.Requests.Teacher;
using LMSApi.App.Responses;
using LMSApi.App.Responses.Teacher;
using TeacherEntity = LMSApi.Database.Enitities.Teacher;

namespace LMSApi.App.Interfaces
{
    public interface ITeacherService
    {

        public Task< List<Teacher>> Index();
        public Task<TeacherEntity> Store(CreateTeacherRequest teacherRequest);
        public Task<TeacherEntity> Update(int teacherId, UpdateTeacherRequest teacherRequest);
        public Task<TeacherEntity> Show(int teacherId);
        public Task<bool> Delete(int teacherId);
        public Task<List<Course>> CoursesAsync(int teacherId);
        public Task<List<Subject>> SubjectsAsync(int teacherId);
        public Task<List<Subscription>> SubscriptionsAsync(int teacherId);
        public Task<List<Class>> ClassesAsync(int teacherId);
        public Task<Course> StoreCourseAsync(int loggedUserId,StoreCourseRequest storeCourseRequest);
        public Task<Subject> AddTeacherToSubjectAsync(int loggedUserId,AddTeacherToSubjectRequest storeCourseRequest);
    }
}
