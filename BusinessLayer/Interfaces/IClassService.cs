
using DataAccessLayer.Entities;
using LMSApi.App.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IClassService
    {
        public Task<ClassResponse> GetClassByIdAsync(int id);
        public Task<IEnumerable<ClassResponse>> GetAllClassesAsync();
        public Task<ClassResponse> CreateClassAsync(Class classEntity);
        public Task<ClassResponse> UpdateClassAsync(int id, Class classEntity);
        public Task<bool> DeleteClassAsync(int id);
        public Task<IEnumerable<StudentResponse>> GetStudentsByClassIdAsync(int classId);
        public Task<IEnumerable<CourseResponse>> GetCoursesByClassIdAsync(int classId);
        public Task<IEnumerable<TeacherResponse>> GetTeachersByClassIdAsync(int classId);
    }
}