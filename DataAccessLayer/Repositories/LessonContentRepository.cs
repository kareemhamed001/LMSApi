
namespace DataAccessLayer.Repositories
{
    public class LessonContentRepository : ILessonContentRepository
    {
        private readonly AppDbContext _context;
        public LessonContentRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<LessonContent> GetByIdAsync(int id)
        {
            return await _context.LessonContents.FindAsync(id);
        }

        public async Task<IEnumerable<LessonContent>> GetAllAsync()
        {
            return await _context.LessonContents.ToListAsync();
        }

        public async Task<LessonContent> CreateAsync(LessonContent lessonContent)
        {
            _context.LessonContents.Add(lessonContent);
            await _context.SaveChangesAsync();
            return lessonContent;
        }
        public async Task<LessonContent> UpdateAsync(LessonContent lessonContent)
        {
            _context.LessonContents.Update(lessonContent);
            await _context.SaveChangesAsync();
            return lessonContent;
        }

        public async Task DeleteAsync(LessonContent lessonContent)
        {

            if (lessonContent == null) return;
            _context.LessonContents.Remove(lessonContent);
            await _context.SaveChangesAsync();
        }
    }
}
