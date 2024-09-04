using BusinessLayer.Services;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILogger<LessonsController> _logger;

        public LessonsController(ILessonService lessonService, ILogger<LessonsController> logger)
        {
            _lessonService = lessonService;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllLessons()
        {
            try
            {
                var lessons = await _lessonService.GetAllLessonsAsync();
                var response = lessons; 
                return Ok(ApiResponseFactory.Create(response, "Lessons fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching lessons.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IApiResponse>> GetLessonById(int id)
        {
            try
            {
                var lesson = await _lessonService.GetLessonByIdAsync(id);
                if (lesson == null)
                {
                    return NotFound(ApiResponseFactory.Create("Lesson not found", 404, false));
                }

                return Ok(ApiResponseFactory.Create(lesson, "Lesson fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the lesson.");
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
