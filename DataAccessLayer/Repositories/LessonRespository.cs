public class LessonRespository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRespository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson> GetLessonByIdAsync(int id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
    {
        return await _context.Lessons.ToListAsync();
    }

    public async Task<Lesson> CreateLessonAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    public async Task<Lesson> UpdateLessonAsync(int id, Lesson lesson)
    {
        var existingLesson = await _context.Lessons.FindAsync(id);
        if (existingLesson == null) return null;

        existingLesson.Name = lesson.Name;
        existingLesson.Description = lesson.Description;
        existingLesson.CourseId = lesson.CourseId;
        existingLesson.SectionNumber = lesson.SectionNumber;

        _context.Lessons.Update(existingLesson);
        await _context.SaveChangesAsync();
        return existingLesson;
    }

    public async Task DeleteLessonAsync(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson != null)
        {
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<bool> CourseExistsAsync(int courseId)
    {
        return await _context.Courses.AnyAsync(c => c.Id == courseId);
    }
}