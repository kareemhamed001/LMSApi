using AutoMapper;
using LMSApi.App.Enums;
using LMSApi.App.Interfaces;
using LMSApi.App.Requests.LessonContent;

namespace BusinessLayer.Services
{
    public class LessonContentService : ILessonContentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
      
        private readonly String _filesPath;
        public LessonContentService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
          
            _filesPath="FilesUploaded";
        }

        public async Task<LessonContent> GetByIdAsync(int id)
        {
            return await _context.LessonContents.FindAsync(id);
        }

        public async Task<IEnumerable<LessonContent>> GetAllAsync()
        {
            return await _context.LessonContents.ToListAsync();
        }

        public async Task<LessonContent> CreateAsync(LessonContentRequest lessonContentDto)
        {
  
            var link = $"{Guid.NewGuid()}{Path.GetExtension(lessonContentDto.Link.FileName)}";
            var path = Path.Combine(_filesPath, link);

            LessonContent lessonContent = new()
            {
                Name = lessonContentDto.Name,
                //Type = lessonContentDto.Type,
                Link = link,
                Content = lessonContentDto.Content,
                LessonId = lessonContentDto.LessonId,
            };

            // Begin a database transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.LessonContents.Add(lessonContent);
                await _context.SaveChangesAsync();

          
                using (var stream = File.Create(path))
                {
                    await lessonContentDto.Link.CopyToAsync(stream);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                throw;
            }

            return lessonContent;
        }
        public async Task<LessonContent> UpdateAsync(int id, LessonContentRequest lessonContentDto)
        {
            var existingLessonContent = await _context.LessonContents.FindAsync(id);
            if (existingLessonContent == null) return null;

            string newFilePath = null;
            if (lessonContentDto.Link != null)
            {
                var link = $"{Guid.NewGuid()}{Path.GetExtension(lessonContentDto.Link.FileName)}";
                newFilePath = Path.Combine(_filesPath, link);
            }


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
               
                existingLessonContent.Name = lessonContentDto.Name;
                existingLessonContent.Type = lessonContentDto.Type;
                existingLessonContent.Content = lessonContentDto.Content;
                existingLessonContent.LessonId = lessonContentDto.LessonId;

        
                if (lessonContentDto.Link != null)
                {
                 
                    var oldFilePath = Path.Combine(_filesPath, existingLessonContent.Link);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }

                    existingLessonContent.Link = Path.GetFileName(newFilePath);

                    using (var stream = File.Create(newFilePath))
                    {
                        await lessonContentDto.Link.CopyToAsync(stream);
                    }
                }

               
                _context.LessonContents.Update(existingLessonContent);
                await _context.SaveChangesAsync();

       
                await transaction.CommitAsync();
            }
            catch
            {
                
                await transaction.RollbackAsync();
                if (newFilePath != null && File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                throw;
            }

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
