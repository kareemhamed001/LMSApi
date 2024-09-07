using AutoMapper;
using DataAccessLayer.Exceptions;


namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(ISubjectService subjectService, IMapper mapper, ILogger<SubjectsController> logger)
        {
            _subjectService = subjectService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Index()
        {
            try
            {
                var subjects = await _subjectService.Index();
                var response = _mapper.Map<List<SubjectResponse>>(subjects);
                return Ok(ApiResponseFactory.Create(response, "Subjects fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching subjects.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> Show(int id)
        {
            try
            {
                var subject = await _subjectService.Show(id);
                var response = _mapper.Map<SubjectResponse>(subject);
                return Ok(ApiResponseFactory.Create(response, "Subject fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Store([FromBody] SubjectRequest request)
        {
            try
            {
                var subject = await _subjectService.StoreAsync(request);
                var response = _mapper.Map<SubjectResponse>(subject);
                return CreatedAtAction(nameof(Show), new { id = subject.Id }, ApiResponseFactory.Create(response, "Subject created successfully", 201, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, [FromBody] UpdateSubjectRequest request)
        {
            try
            {
                var subject = await _subjectService.UpdateAsync(id, request);
                var response = _mapper.Map<SubjectResponse>(subject);
                return Ok(ApiResponseFactory.Create(response, "Subject updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                await _subjectService.DeleteAsync(id);
                return NoContent(); // Status code 204
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{subjectId}/classes")]
        public async Task<ActionResult<IApiResponse>> GetClasses(int subjectId)
        {
            try
            {
                var classes = await _subjectService.ClassesAsync(subjectId);
                var response = _mapper.Map<List<ClassResponse>>(classes);
                return Ok(ApiResponseFactory.Create(response, "Classes fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching classes for the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{subjectId}/courses")]
        public async Task<ActionResult<IApiResponse>> GetCourses(int subjectId)
        {
            try
            {
                var courses = await _subjectService.CoursesAsync(subjectId);
                var response = _mapper.Map<List<CourseResponse>>(courses);
                return Ok(ApiResponseFactory.Create(response, "Courses fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching courses for the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{subjectId}/students")]
        public async Task<ActionResult<IApiResponse>> GetStudents(int subjectId)
        {
            try
            {
                var students = await _subjectService.StudentsAsync(subjectId);
                var response = _mapper.Map<List<StudentResponse>>(students);
                return Ok(ApiResponseFactory.Create(response, "Students fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching students for the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{subjectId}/teachers")]
        public async Task<ActionResult<IApiResponse>> GetTeachers(int subjectId)
        {
            try
            {
                var teachers = await _subjectService.TeachersAsync(subjectId);
                var response = _mapper.Map<List<TeacherResponse>>(teachers);
                return Ok(ApiResponseFactory.Create(response, "Teachers fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching teachers for the subject.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPost]
        [Route("add-to-class")]
        public async Task<ActionResult<IApiResponse>> AddSubjectToClass([FromBody] AddSubjectToClassRequest request)
        {
            try
            {
                var result = await _subjectService.AddSubjectToClass(request);
                return Ok(ApiResponseFactory.Create("Subject added to class successfully", 200, result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the subject to the class.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }
    }
}
