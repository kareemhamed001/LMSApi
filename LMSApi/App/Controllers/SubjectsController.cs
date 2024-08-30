using AutoMapper;
using LMSApi.App.Exceptions;
using LMSApi.App.Interfaces;
using LMSApi.App.Requests;
using LMSApi.App.Requests.Subject;
using LMSApi.App.Responses;
using LMSApi.App.Responses.Teacher;

namespace LMSApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService subjectService;
        private readonly IMapper mapper;

        public SubjectsController(ISubjectService subjectService, IMapper mapper)
        {
            this.subjectService = subjectService;
            this.mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IApiResponse>> Index()
        {
            List<SubjectResponse> subjects = mapper.Map<List<SubjectResponse>>(await subjectService.Index());
            return Ok(ApiResponseFactory.Create(subjects, "Subjects fetched successfully", 200, true));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IApiResponse>> Show(int id)
        {
            try
            {
                SubjectResponse subject = mapper.Map<SubjectResponse>(await subjectService.Show(id));
                return Ok(ApiResponseFactory.Create(subject, "Subject fetched successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<IApiResponse>> Store([FromForm] SubjectRequest request)
        {
            try
            {
                SubjectResponse subject = mapper.Map<SubjectResponse>(await subjectService.StoreAsync(request));


                return Ok(ApiResponseFactory.Create(subject, "Subject created successfully", 201, true));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, UpdateSubjectRequest request)
        {
            try
            {
                SubjectResponse subject = mapper.Map<SubjectResponse>(await subjectService.UpdateAsync(id, request));
                return Ok(ApiResponseFactory.Create(subject, "Subject updated successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                await subjectService.DeleteAsync(id);
                return Ok(ApiResponseFactory.Create("Subject deleted successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpGet("{subjectId}/teachers")]
        public async Task<ActionResult<IApiResponse>> Teachers(int subjectId)
        {
            try
            {
                List<TeacherResponse> teachers = mapper.Map<List<TeacherResponse>>(await subjectService.TeachersAsync(subjectId));
                return Ok(ApiResponseFactory.Create(teachers, "Teachers fetched successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpGet("{subjectId}/classes")]
        public async Task<ActionResult<IApiResponse>> Classes(int subjectId)
        {
            try
            {
                List<ClassResponse> classes = mapper.Map<List<ClassResponse>>(await subjectService.ClassesAsync(subjectId));
                return Ok(ApiResponseFactory.Create(classes, "Classes fetched successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpGet("{subjectId}/courses")]
        public async Task<ActionResult<IApiResponse>> Courses(int subjectId)
        {
            try
            {
                var courses = mapper.Map<List<CourseResponse>>(await subjectService.CoursesAsync(subjectId));
                return Ok(ApiResponseFactory.Create(courses, "Courses fetched successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

        [HttpPost("add-class")]
        public async Task<ActionResult<IApiResponse>> AddClass([FromBody] AddSubjectToClassRequest request)
        {
            try
            {
                await subjectService.AddSubjectToClass(request);
                return Ok(ApiResponseFactory.Create("Class added successfully", 200, true));
            }
            catch (NotFoundException e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 404, false));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseFactory.Create(e.Message, 500, false));
            }
        }

    }
}
