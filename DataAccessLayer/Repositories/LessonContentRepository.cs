
namespace LMSApi.App.Services
{
    public class LessonContentService : ILessonContentRepository
    {
        private readonly AppDbContext _context;

        private readonly String _filesPath;
        public LessonContentService(AppDbContext context)
        {
            _context = context;
            _filesPath = "FilesUploaded";
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
        public async Task<LessonContent> UpdateAsync(int id, LessonContent lessonContent)
        {
            var existingLessonContent = await _context.LessonContents.FindAsync(id);
            if (existingLessonContent == null) return null;


            existingLessonContent.Name = lessonContent.Name;
            existingLessonContent.Type = lessonContent.Type;
            existingLessonContent.Content = lessonContent.Content;
            existingLessonContent.LessonId = lessonContent.LessonId;
            existingLessonContent.Link = lessonContent.Link;


            _context.LessonContents.Update(existingLessonContent);
            await _context.SaveChangesAsync();

            return existingLessonContent;
        }

        public async Task DeleteAsync(int id)
        {

            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null) return;


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                _context.LessonContents.Remove(lessonContent);
                await _context.SaveChangesAsync();


                var filePath = Path.Combine(_filesPath, lessonContent.Link);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                await transaction.CommitAsync();
            }
            catch
            {

                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
