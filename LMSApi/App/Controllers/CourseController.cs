using AutoMapper;
using DataAccessLayer.Exceptions;

namespace LMSApi.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService courseService, IMapper mapper, ILogger<CoursesController> logger)
        {
            _courseService = courseService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ApiResponseListStrategy<CourseResponse>>> GetAll()
        {
            try
            {
                var courses = await _courseService.GetAllAsync();
                var courseResponses = _mapper.Map<IEnumerable<CourseResponse>>(courses);
                return Ok(ApiResponseFactory.Create(courseResponses, "Courses fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching courses. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> GetById(int id)
        {
            try
            {
                var course = await _courseService.GetByIdAsync(id);

                var courseResponse = _mapper.Map<CourseResponse>(course);
                return Ok(ApiResponseFactory.Create(courseResponse, "Course fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching course with id {id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ApiResponseSingleStrategy>> Add([FromBody] CourseRequest courseRequest)
        {
            try
            {
                Course newCourse = await _courseService.AddAsync(courseRequest);
                CourseResponse courseResponse = _mapper.Map<CourseResponse>(newCourse);
                return Ok(ApiResponseFactory.Create(courseResponse, "Course created successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while creating course. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, [FromBody] CourseRequest courseRequest)
        {
            try
            {
                var existingCourse = await _courseService.GetByIdAsync(id);

                await _courseService.UpdateAsync(courseRequest);
                return Ok(ApiResponseFactory.Create(courseRequest, "Course updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating course with id {id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
                return Ok(ApiResponseFactory.Create("Course deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting course with id {id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }


    }
}
