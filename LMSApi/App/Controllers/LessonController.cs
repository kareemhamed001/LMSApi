using AutoMapper;
using LMSApi.App.Interfaces;
using LMSApi.App.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;
    private readonly IMapper _mapper;

    public LessonController(ILessonService lessonService, IMapper mapper)
    {
        _lessonService = lessonService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LessonRequest>> GetLessonById(int id)
    {
        var lesson = await _lessonService.GetLessonByIdAsync(id);
        if (lesson == null) return NotFound();

        var lessonDto = _mapper.Map<LessonRequest>(lesson);
        return Ok(lessonDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonRequest>>> GetAllLessons()
    {
        var lessons = await _lessonService.GetAllLessonsAsync();
        var lessonDtos = _mapper.Map<IEnumerable<LessonRequest>>(lessons);
        return Ok(lessonDtos);
    }

    [HttpPost]
    public async Task<ActionResult<LessonRequest>> CreateLesson(LessonRequest lessonDto)
    {
        // Check if the CourseId is valid
        if (!await _lessonService.CourseExistsAsync(lessonDto.CourseId))
        {
            return BadRequest(new { message = "Invalid CourseId" });
        }

        var lesson = _mapper.Map<Lesson>(lessonDto);
        var createdLesson = await _lessonService.CreateLessonAsync(lesson);

        var createdLessonDto = _mapper.Map<LessonRequest>(createdLesson);
        return Ok("Lesson Created Successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson(int id, LessonRequest lessonDto)
    {
        var lesson = _mapper.Map<Lesson>(lessonDto);
        var updatedLesson = await _lessonService.UpdateLessonAsync(id, lesson);

        if (updatedLesson == null) return NotFound();

        var updatedLessonDto = _mapper.Map<LessonRequest>(updatedLesson);
        return Ok(updatedLessonDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        await _lessonService.DeleteLessonAsync(id);
        return NoContent();
    }
}