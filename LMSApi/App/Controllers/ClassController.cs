using AutoMapper;
using LMSApi.App.Atrributes;
using DataAccessLayer.Exceptions;
using System.Collections.Generic;
using DataAccessLayer.Entities;


namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ClassController(IClassService classService, IMapper mapper, ILogger<ClassController> logger)
        {
            _classService = classService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [CheckPermission("Class.index")]
        public async Task<ActionResult<ClassResponse>> GetClassById(int id)
        {
            try
            {
                var classEntity = await _classService.GetClassByIdAsync(id);

                ClassResponse classResponse = _mapper.Map<ClassResponse>(classEntity);
                return Ok(ApiResponseFactory.Create(classResponse, "Class Exists", 201, false));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }


        }

        [HttpGet]
        [CheckPermission("Class.AllClass")]
        public async Task<ActionResult<IEnumerable<ClassResponse>>> GetAllClasses()
        {
            var classEntities = await _classService.GetAllClassesAsync();
            return Ok(ApiResponseFactory.Create(_mapper.Map<IEnumerable<ClassResponse>>(classEntities), "Classes Fetched Successfully", 201, true));
        }

        [HttpPost]
        [CheckPermission("Class.createClass")]
        public async Task<ActionResult<ClassResponse>> CreateClass(ClassRequest classDto)
        {
            try
            {
                ClassResponse ClassResponse = _mapper.Map<ClassResponse>(await _classService.CreateClassAsync(classDto));

                return Ok(ApiResponseFactory.Create(ClassResponse, "Success", 201, true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }

        }

        [HttpPut("{id}")]
        [CheckPermission("class.updateClass")]
        public async Task<ActionResult<ClassResponse>> UpdateClass(int id, ClassRequest classDto)
        {
            try
            {

                var updatedClass = await _classService.UpdateClassAsync(id, classDto);

                return Ok(ApiResponseFactory.Create(_mapper.Map<ClassResponse>(updatedClass), "Class Updated Successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }

        }



        [HttpDelete("{id}")]
        [CheckPermission("class.deleteClass")]
        public async Task<ActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteClassAsync(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }

        }

        [HttpGet("{classId}/students")]
        [CheckPermission("class.getStudentsByClassId")]
        public async Task<ActionResult<ApiResponseListStrategy<StudentResponse>>> GetStudentsByClassId(int classId)
        {
            try
            {
                // Get students for the class
                List<Student> students = (List<Student>)await _classService.GetStudentsByClassIdAsync(classId);

                // Map the class entity to GetStudentOfClassRequest
                List<StudentResponse> getStudentOfClassRequest = _mapper.Map<List<StudentResponse>>(students);

                return Ok(ApiResponseFactory.Create(getStudentOfClassRequest, "Students Fetched Successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{classId}/courses")]
        [CheckPermission("class.getCoursesByClassId")]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetCoursesByClassId(int classId)
        {
            try
            {
                var courses = await _classService.GetCoursesByClassIdAsync(classId);
                return Ok(ApiResponseFactory.Create(_mapper.Map<List<CourseResponse>>(courses), "Courses Fetched Successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ApiResponseFactory.Create(ex.Message, 500, false));
            }

        }

        [HttpGet("{classId}/teachers")]
        [CheckPermission("class.getTeachersByClassId")]
        public async Task<ActionResult<ApiResponseListStrategy<TeacherResponse>>> GetTeachersByClassId(int classId)
        {
            try
            {
                var teachers = await _classService.GetTeachersByClassIdAsync(classId);
                List<TeacherResponse> teachersResponse = _mapper.Map<List<TeacherResponse>>(teachers);

                return Ok(ApiResponseFactory.Create(teachersResponse, "Teachers Fetched Successfully", 201, true));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ApiResponseFactory.Create(ex.Message, 500, false));
            }

        }
    }
}