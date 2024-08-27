using LMSApi.App.Enums;
using LMSApi.App.Interfaces.Teacher;
using LMSApi.App.Options;
using LMSApi.App.Requests.Teacher;
using LMSApi.App.Responses;
using LMSApi.App.Responses.Teacher;
using LMSApi.Database.Data;
using System.IdentityModel.Tokens.Jwt;

namespace LMSApi.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController
        (ITeacherService teacherService, ILogger<TeachersController> logger, JwtOptions jwtOptions) : ControllerBase, ITeacherController
    {
        private readonly ITeacherService teacherService = teacherService;
        private readonly ILogger<TeachersController> _logger = logger;
        private readonly JwtOptions _jwtOptions = jwtOptions;

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ApiResponse<TeacherIndexResponse>>> Index()
        {
            (ServicesMethodsResponsesEnum response, List<TeacherIndexResponse>? teachers) = await teacherService.Index();
            if (response == ServicesMethodsResponsesEnum.Exception)
            {
                return StatusCode(500, new ApiResponse<Teacher>
                {
                    Message = "Internal server error",
                    Success = false,
                    Status = 500
                });
            }
            return Ok(new ApiResponse<TeacherIndexResponse>
            {
                Data = teachers,
                Message = "Teachers fetched successfully",
                Success = true,
                Status = 200
            });
        }
        [HttpDelete]
        [Route("{teacherId}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int teacherId)
        {

            ServicesMethodsResponsesEnum response = await teacherService.Delete(teacherId);
            if (response == ServicesMethodsResponsesEnum.Exception)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Message = "Internal server error",
                    Success = false,
                    Status = 500
                });
            }
            if (response == ServicesMethodsResponsesEnum.NotFound)
            {
                return NotFound(new ApiResponse<string>
                {
                    Message = "Teacher not found",
                    Success = false,
                    Status = 404
                });
            }
            return Ok(new ApiResponse<string>
            {
                Message = "Teacher deleted successfully",
                Success = true,
                Status = 200
            });
        }
        [HttpGet]
        [Route("{teacherId}")]
        public async Task<ActionResult<ApiResponse<Teacher>>> Show(int teacherId)
        {

            (ServicesMethodsResponsesEnum response, Teacher? teacher) = await teacherService.Show(teacherId);
            if (response == ServicesMethodsResponsesEnum.Exception)
            {
                return StatusCode(500, new ApiResponse<Teacher>
                {
                    Message = "Internal server error",
                    Success = false,
                    Status = 500
                });
            }
            if (response == ServicesMethodsResponsesEnum.NotFound)
            {
                return NotFound(new ApiResponse<Teacher>
                {
                    Message = "Teacher not found",
                    Success = false,
                    Status = 404
                });
            }
            return Ok(new ApiResponse<Teacher>
            {
                Data = new List<Teacher> { teacher },
                Message = "Teacher fetched successfully",
                Success = true,
                Status = 200
            });
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TeacherResponse>>> Store([FromBody] CreateTeacherRequest teacherRequest)
        {

            (ServicesMethodsResponsesEnum response, Teacher? teacher, string message) = await teacherService.Store(teacherRequest);
            if (response == ServicesMethodsResponsesEnum.Exception)
            {
                return StatusCode(500, new ApiResponse<Teacher>
                {
                    Message = message,
                    Success = false,
                    Status = 500
                });
            }

            return Ok(new ApiResponse<TeacherResponse>
            {
                Data = new List<TeacherResponse> { new TeacherResponse{
                    Id = teacher.Id,
                    FirstName = teacher.User.FirstName,
                    LastName = teacher.User.LastName,
                    Email = teacher.User.Email,
                    Phone = teacher.User.Phone,
                    CommunicationEmail = teacher.Email,
                    CommunicationPhone = teacher.Phone,
                    NickName = teacher.NickName
                }
               },
                Message = message,
                Success = true,
                Status = 200
            });
        }
        [HttpPut]
        [Route("{teacherId}")]
        public async Task<ActionResult<ApiResponse<TeacherResponse>>> Udpate(int teacherId, [FromBody] UpdateTeacherRequest teacherRequest)
        {
            (ServicesMethodsResponsesEnum response, Teacher? teacher) = await teacherService.Update(teacherId, teacherRequest);
            if (response == ServicesMethodsResponsesEnum.Exception)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Message = "Internal server error",
                    Success = false,
                    Status = 500
                });
            }

            if (response == ServicesMethodsResponsesEnum.NotFound)
            {
                return NotFound(new ApiResponse<string>
                {
                    Message = "Teacher not found",
                    Success = false,
                    Status = 500
                });
            }

            return Ok(new ApiResponse<TeacherResponse>
            {
                Data = new List<TeacherResponse> {
                new TeacherResponse{
                    Id = teacher.Id,
                    FirstName = teacher.User.FirstName,
                    LastName = teacher.User.LastName,
                    Email = teacher.User.Email,
                    Phone = teacher.User.Phone,
                    CommunicationEmail = teacher.Email,
                    CommunicationPhone = teacher.Phone,
                    NickName = teacher.NickName
                }
                },
                Message = "Teacher updated successfully",
                Success = true,
                Status = 200
            });

        }
    }
}
