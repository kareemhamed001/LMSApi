using LMSApi.App.Requests.Teacher;
using LMSApi.App.Responses;

namespace BusinessLayer.Interfaces
{
    public interface ITeacherController
    {
        public Task<ActionResult<ApiResponse<TeacherIndexResponse>>> Index();
        public Task<ActionResult<ApiResponse<TeacherResponse>>> Store([FromBody] CreateTeacherRequest teacherRequest);
        public Task<ActionResult<ApiResponse<TeacherResponse>>> Udpate(int teacherId, [FromBody] UpdateTeacherRequest teacherRequest);
        public Task<ActionResult<ApiResponse<Teacher>>> Show(int teacherId);
        public Task<ActionResult<ApiResponse<string>>> Delete(int teacherId);


    }
}
