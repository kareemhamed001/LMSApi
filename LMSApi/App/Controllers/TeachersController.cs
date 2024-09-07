using AutoMapper;
using DataAccessLayer.Exceptions;
using LMSApi.App.Atrributes;

using LMSApi.App.Options;
using System.Security.Claims;

namespace LMSApi.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController
        (ITeacherService teacherService, ILogger<TeachersController> logger, JwtOptions jwtOptions, IMapper mapper) : ControllerBase
    {
        private readonly ITeacherService teacherService = teacherService;
        private readonly ILogger<TeachersController> _logger = logger;
        private readonly JwtOptions _jwtOptions = jwtOptions;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Route("")]
        [CheckPermission("teachers.list")]
        public async Task<ActionResult<IApiResponse>> Index()
        {
            try
            {
                List<Teacher> teachers = await teacherService.Index();

                return Ok(ApiResponseFactory.Create(mapper.Map<List<TeacherResponse>>(teachers), "Teachers fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching teachers,log message is {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpDelete]
        [Route("{teacherId}")]
        [CheckPermission("teachers.delete")]
        public async Task<ActionResult<IApiResponse>> Delete(int teacherId)
        {
            try
            {
                await teacherService.Delete(teacherId);
                return Ok(ApiResponseFactory.Create("Teacher deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }

        }


        [HttpGet]
        [Route("{teacherId}")]
        [CheckPermission("teachers.show")]
        public async Task<ActionResult<IApiResponse>> Show(int teacherId)
        {
            try
            {
                Teacher teacher = await teacherService.Show(teacherId);
                return Ok(ApiResponseFactory.Create(mapper.Map<ShowTeacherResponse>(teacher), "Teacher fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while showin teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }
        [HttpPost]
        [CheckPermission("teachers.store")]
        public async Task<ActionResult<ApiResponse<TeacherResponse>>> Store([FromBody] CreateTeacherRequest teacherRequest)
        {
            try
            {
                Teacher teacher = await teacherService.Store(teacherRequest);

                return Ok(ApiResponseFactory.Create(mapper.Map<TeacherResponse>(teacher), "Teacher created successfully", 201, true));
            }
            catch (ForbidenException e)
            {
                return StatusCode(400, ApiResponseFactory.Create(e.Message, 400, false));
            }
            catch (Exception e)
            {
                return StatusCode(500, ApiResponseFactory.Create(e.Message, 500, false));
            }

        }
        [HttpPut]
        [Route("{teacherId}")]
        [CheckPermission("teachers.update")]
        public async Task<ActionResult<ApiResponse<TeacherResponse>>> Udpate(int teacherId, [FromBody] UpdateTeacherRequest teacherRequest)
        {
            try
            {
                Teacher teacher = await teacherService.Update(teacherId, teacherRequest);

                return Ok(ApiResponseFactory.Create(mapper.Map<TeacherResponse>(teacher), "Update Successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }


        }

        [HttpGet]
        [Route("{teacherId}/courses")]
        [CheckPermission("teachers.list_courses")]
        public async Task<ActionResult<IApiResponse>> Courses(int teacherId)
        {
            try
            {
                List<Course> courses = await teacherService.CoursesAsync(teacherId);
                return Ok(ApiResponseFactory.Create(mapper.Map<List<CourseResponse>>(courses), "Courses fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching courses for teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }

        }
        [HttpGet]
        [Route("{teacherId}/subjects")]
        [CheckPermission("teachers.list_subjects")]
        public async Task<ActionResult<IApiResponse>> Subjects(int teacherId)
        {
            try
            {
                var Subjects = await teacherService.SubjectsAsync(teacherId);
                return Ok(ApiResponseFactory.Create(mapper.Map<List<SubjectResponse>>(Subjects), "Subjects fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching subjects for teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }

        }


        [HttpGet]
        [Route("{teacherId}/subscriptions")]
        [CheckPermission("teachers.list_subscriptions")]
        public async Task<ActionResult<IApiResponse>> Subcriptions(int teacherId)
        {
            try
            {
                var Subcriptions = await teacherService.SubscriptionsAsync(teacherId);
                return Ok(ApiResponseFactory.Create(mapper.Map<List<SubscriptionResponse>>(Subcriptions), "Subcriptions fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching subcriptions for teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }

        }


        [HttpGet]
        [Route("{teacherId}/classes")]
        [CheckPermission("teachers.list_classes")]
        public async Task<ActionResult<IApiResponse>> Classes(int teacherId)
        {
            try
            {
                var Classes = await teacherService.ClassesAsync(teacherId);
                return Ok(ApiResponseFactory.Create(mapper.Map<List<ClassResponse>>(Classes), "Classes fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching classes for teacher with id {teacherId} ,log message is {logMessage}", teacherId, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }

        }

        [HttpPost]
        [Route("courses")]
        [CheckPermission("teachers.store_courses")]
        public async Task<ActionResult<IApiResponse>> StoreCourse([FromBody] StoreCourseRequest storeCourseRequest)
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity;
                if (userIdentity == null || !userIdentity.IsAuthenticated)
                {
                    HttpContext.Response.StatusCode = 401;
                }
                var userId = int.Parse(userIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var course = await teacherService.StoreCourseAsync(userId, storeCourseRequest);
                return Ok(ApiResponseFactory.Create(mapper.Map<CourseResponse>(course), "Course created successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (ForbidenException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 400, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while creating course,log message is {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }

        }

    }
}
