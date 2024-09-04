
namespace DataAccessLayer.Repositories
{

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

        public async Task<Lesson> UpdateLessonAsync(Lesson lesson)
        {

            if (lesson == null) return null;

            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task DeleteLessonAsync(Lesson lesson)
        {

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

        }
        public async Task<bool> CourseExistsAsync(int courseId)
        {
            return await _context.Courses.AnyAsync(c => c.Id == courseId);
        }
    }
}