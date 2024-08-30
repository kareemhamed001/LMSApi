using LMSApi.App.Enums;
using LMSApi.App.Requests.Teacher;
using LMSApi.App.Responses.Teacher;
using TeacherEntity = LMSApi.Database.Enitities.Teacher;

namespace LMSApi.App.Interfaces
{
    public interface ITeacherService
    {

        public Task<(ServicesMethodsResponsesEnum, List<TeacherIndexResponse>?)> Index();
        public Task<(ServicesMethodsResponsesEnum, TeacherEntity?, string message)> Store(CreateTeacherRequest teacherRequest);
        public Task<(ServicesMethodsResponsesEnum, TeacherEntity?)> Update(int teacherId, UpdateTeacherRequest teacherRequest);
        public Task<(ServicesMethodsResponsesEnum, TeacherEntity?)> Show(int teacherId);
        public Task<ServicesMethodsResponsesEnum> Delete(int teacherId);

    }
}
