using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonContentController : ControllerBase
    {
        private readonly ILessonContentService _lessonContentService;
        private readonly ILogger<LessonContentController> _logger;
        private readonly IMapper _mapper;

        public LessonContentController(ILessonContentService lessonContentService, IMapper mapper, ILogger<LessonContentController> logger)
        {
            _lessonContentService = lessonContentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IApiResponse>> GetById(int id)
        {
            try
            {
                var lessonContent = await _lessonContentService.GetByIdAsync(id);
                if (lessonContent == null)
                {
                    return NotFound(ApiResponseFactory.Create("Lesson content not found", 404, false));
                }

                var lessonContentResponse = _mapper.Map<LessonContentResponse>(lessonContent);
                return Ok(ApiResponseFactory.Create(lessonContentResponse, "Lesson content retrieved successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving lesson content.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpGet]
        public async Task<ActionResult<IApiResponse>> GetAll()
        {
            try
            {
                var lessonContents = await _lessonContentService.GetAllAsync();
                var lessonContentResponses = _mapper.Map<IEnumerable<LessonContentResponse>>(lessonContents);
                return Ok(ApiResponseFactory.Create(lessonContentResponses, "Lesson contents fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching lesson contents.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<IApiResponse>> Create([FromBody] LessonContentRequest request)
        {
            try
            {
                // Validate the LessonId
                var lessonExists = await _lessonContentService.LessonExistsAsync(request.LessonId);
                if (!lessonExists)
                {
                    return BadRequest(ApiResponseFactory.Create("Lesson with the specified ID does not exist.", 400, false));
                }

                var lessonContent = await _lessonContentService.CreateAsync(request);
                var lessonContentResponse = _mapper.Map<LessonContentResponse>(lessonContent);
                return CreatedAtAction(nameof(GetById), new { id = lessonContent.Id }, ApiResponseFactory.Create(lessonContentResponse, "Lesson content created successfully", 201, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating lesson content.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, [FromBody] LessonContentRequest request)
        {
            try
            {
                var existingLessonContent = await _lessonContentService.GetByIdAsync(id);
                if (existingLessonContent == null)
                {
                    return NotFound(ApiResponseFactory.Create("Lesson content not found", 404, false));
                }

                var updatedLessonContent = await _lessonContentService.UpdateAsync(id, request);
                var lessonContentResponse = _mapper.Map<LessonContentResponse>(updatedLessonContent);
                return Ok(ApiResponseFactory.Create(lessonContentResponse, "Lesson content updated successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating lesson content.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var lessonContent = await _lessonContentService.GetByIdAsync(id);
                if (lessonContent == null)
                {
                    return NotFound(ApiResponseFactory.Create("Lesson content not found", 404, false));
                }

                await _lessonContentService.DeleteAsync(id);
                return NoContent(); // Status code 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting lesson content.");
                return StatusCode(500, ApiResponseFactory.Create("Internal server error", 500, false));
            }
        }
    }
}
