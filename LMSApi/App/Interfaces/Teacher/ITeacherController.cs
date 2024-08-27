using LMSApi.App.Requests.Teacher;
using LMSApi.App.Responses;
using LMSApi.App.Responses.Teacher;
using TeacherEntity= LMSApi.Database.Enitities.Teacher;

namespace LMSApi.App.Interfaces.Teacher
{
    public interface ITeacherController
    {
        public Task<ActionResult<ApiResponse<TeacherIndexResponse>>> Index();
        public Task<ActionResult<ApiResponse<TeacherResponse>>> Store([FromBody] CreateTeacherRequest teacherRequest);
        public Task<ActionResult<ApiResponse<TeacherResponse>>> Udpate(int teacherId, [FromBody] UpdateTeacherRequest teacherRequest);
        public Task<ActionResult<ApiResponse<TeacherEntity>>> Show(int teacherId);
        public Task<ActionResult<ApiResponse<string>>> Delete(int teacherId);


    }
}
