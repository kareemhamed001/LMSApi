using AutoMapper;
using DataAccessLayer.Exceptions;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILogger<LessonsController> _logger;
        private readonly IMapper _mapper;
        public LessonsController(ILessonService lessonService, IMapper mapper, ILogger<LessonsController> logger)
        {
            _lessonService = lessonService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<LessonResponse>>> GetAllLessons()
        {

            var lessons = await _lessonService.GetAllLessonsAsync();

            // Wrap the list in ApiResponseListStrategy
            return Ok(ApiResponseFactory.Create(_mapper.Map<IEnumerable<LessonResponse>>(lessons), "lessons Fetched Successfully", 201, true));
        }

        [HttpGet]
        [HttpGet("{id}")]
        public async Task<ActionResult<IApiResponse>> GetLessonById(int id)
        {
            try
            {
                var lesson = await _lessonService.GetLessonByIdAsync(id);

                var lessonResponse = _mapper.Map<LessonResponse>(lesson);
                return Ok(ApiResponseFactory.Create(lessonResponse, "Lesson retrieved successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the lesson.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> CreateLesson([FromBody] LessonRequest request)
        {
            try
            {
                var lesson = new Lesson
                {
                    Name = request.Name,
                    Description = request.Description,
                    CourseId = request.CourseId,
                    SectionNumber = request.SectionNumber
                };

                var createdLesson = await _lessonService.CreateLessonAsync(lesson);
                return CreatedAtAction(nameof(GetLessonById), new { id = createdLesson.Id }, ApiResponseFactory.Create(createdLesson, "Lesson created successfully", 201, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the lesson.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> UpdateLesson(int id, [FromBody] LessonRequest request)
        {
            try
            {
                var lesson = new Lesson
                {
                    Name = request.Name,
                    Description = request.Description,
                    CourseId = request.CourseId,
                    SectionNumber = request.SectionNumber
                };

                var updatedLesson = await _lessonService.UpdateLessonAsync(id, lesson);
                if (updatedLesson == null)
                {
                    return NotFound(ApiResponseFactory.Create("Lesson not found", 404, false));
                }

                return Ok(ApiResponseFactory.Create(updatedLesson, "Lesson updated successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the lesson.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> DeleteLesson(int id)
        {
            try
            {
                await _lessonService.DeleteLessonAsync(id);
                return NoContent(); // Status code 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the lesson.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("course/{courseId}/exists")]
        public async Task<ActionResult<IApiResponse>> CourseExists(int courseId)
        {
            try
            {
                var exists = await _lessonService.CourseExistsAsync(courseId);
                return Ok(ApiResponseFactory.Create(exists, "Course existence checked", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the course exists.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }
    }
}
