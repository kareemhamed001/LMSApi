using Microsoft.AspNetCore.Mvc;
using LMSApi.App.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonContentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILessonContentService _lessonContentService;
        private readonly IMapper _mapper;

        public LessonContentController(AppDbContext context, ILessonContentService lessonContentService, IMapper mapper)
        {
            _context = context;
            _lessonContentService = lessonContentService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lessonContent = await _lessonContentService.GetByIdAsync(id);
            if (lessonContent == null) return NotFound();
            return Ok(lessonContent);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lessonContents = await _lessonContentService.GetAllAsync();
            return Ok(lessonContents);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] LessonContentRequest request)
        {
            // Validate the LessonId
            var lessonExists = await _context.Lessons.AnyAsync(l => l.Id == request.LessonId);
            if (!lessonExists)
            {
                return BadRequest("Lesson with the specified ID does not exist.");
            }

            try
            {
                var lessonContent = await _lessonContentService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = lessonContent.Id }, lessonContent);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] LessonContentRequest lessonContentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLessonContent = await _lessonContentService.GetByIdAsync(id);
            if (existingLessonContent == null)
            {
                return NotFound();
            }

            try
            {
                var updatedLessonContent = await _lessonContentService.UpdateAsync(id, lessonContentRequest);
                return Ok(updatedLessonContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // Return 500 for server error
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lessonContent = await _lessonContentService.GetByIdAsync(id);
            if (lessonContent == null) return NotFound();

            try
            {
                await _lessonContentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
