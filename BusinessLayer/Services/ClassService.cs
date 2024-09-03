
using DataAccessLayer.Interfaces;
using DataAccessLayer.Entities;
using LMSApi.App.Responses;
using AutoMapper;

namespace BusinessLayer.Services
{

    public class ClassServices(IClassRepository classRepository, IMapper mapper) : IClassService
    {
        private string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
        public async Task<ClassResponse> GetClassByIdAsync(int id)
        {
            Class classEntity = await classRepository.GetClassByIdAsync(id);
            return mapper.Map<ClassResponse>(classEntity);
        }
        public async Task<IEnumerable<ClassResponse>> GetAllClassesAsync()
        {
            List<Class> classes = (List<Class>)await classRepository.GetAllClassesAsync();
            return mapper.Map<IEnumerable<ClassResponse>>(classes);
        }

        public async Task<ClassResponse> CreateClassAsync(Class classEntity)
        {
            Class newClass = await classRepository.CreateClassAsync(classEntity);
            return mapper.Map<ClassResponse>(newClass);
        }

        public async Task<ClassResponse> UpdateClassAsync(int id, Class classEntity)
        {
            Class updatedClass = await classRepository.UpdateClassAsync(id, classEntity);
            return mapper.Map<ClassResponse>(updatedClass);
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            return await classRepository.DeleteClassAsync(id);
        }
        public async Task<IEnumerable<StudentResponse>> GetStudentsByClassIdAsync(int classId)
        {
            List<Student> students = (List<Student>)await classRepository.GetStudentsByClassIdAsync(classId);
            return mapper.Map<IEnumerable<StudentResponse>>(students);
        }
        public async Task<IEnumerable<CourseResponse>> GetCoursesByClassIdAsync(int classId)
        {
            List<Course> courses = (List<Course>)await classRepository.GetCoursesByClassIdAsync(classId);
            return mapper.Map<IEnumerable<CourseResponse>>(courses);
        }
        public async Task<IEnumerable<TeacherResponse>> GetTeachersByClassIdAsync(int classId)
        {
            List<Teacher> teachers = (List<Teacher>)await classRepository.GetTeachersByClassIdAsync(classId);
            return mapper.Map<IEnumerable<TeacherResponse>>(teachers);
        }


    }
}