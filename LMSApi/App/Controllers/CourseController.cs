using AutoMapper;
using LMSApi.App.Interfaces;
using LMSApi.App.Requests;
using LMSApi.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMSApi.App.Controllers
{


    [ApiController]
        [Route("api/[controller]")]
        public class CoursesController : ControllerBase
        {
            private readonly ICourseService _courseRepository;
            private readonly IMapper _mapper;

            public CoursesController(ICourseService courseRepository, IMapper mapper)
            {
                _courseRepository = courseRepository;
                _mapper = mapper;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<CourseRequest>> GetById(int id)
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                    return NotFound();

                var courseDto = _mapper.Map<CourseRequest>(course);
                return Ok(courseDto);
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<CourseRequest>>> GetAll()
            {
                var courses = await _courseRepository.GetAllAsync();
                var courseDtos = _mapper.Map<IEnumerable<CourseRequest>>(courses);
                return Ok(courseDtos);
            }
        [HttpPost]
        public async Task<ActionResult<CourseRequest>> Create(CourseRequest courseDto)
        {
            // Check if Teacher exists
            if (!await _courseRepository.TeacherExistsAsync(courseDto.TeacherId))
            {
                return BadRequest($"Teacher with ID {courseDto.TeacherId} does not exist.");
            }

            // Check if Subject exists
            if (!await _courseRepository.SubjectExistsAsync(courseDto.SubjectId))
            {
                return BadRequest($"Subject with ID {courseDto.SubjectId} does not exist.");
            }

            // Check if Class exists
            if (!await _courseRepository.ClassExistsAsync(courseDto.ClassId))
            {
                return BadRequest($"Class with ID {courseDto.ClassId} does not exist.");
            }

            var course = _mapper.Map<Course>(courseDto);
            await _courseRepository.AddAsync(course);

            var createdCourseDto = _mapper.Map<CourseRequest>(course);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, createdCourseDto);
        }

        [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                await _courseRepository.DeleteAsync(id);
                return NoContent();
            }
        }
    }

