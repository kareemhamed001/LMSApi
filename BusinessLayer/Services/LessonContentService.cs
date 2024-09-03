using AutoMapper;
using DataAccessLayer.Interfaces;
using LMSApi.App.Requests;

namespace BusinessLayer.Services
{
    public class LessonContentService : ILessonContentService
    {
        private readonly ILessonContentRepository lessonContentRepository;
        private readonly IMapper _mapper;

        private readonly String _filesPath;
        public LessonContentService(ILessonContentRepository lessonContentRepository, IMapper mapper)
        {
            this.lessonContentRepository = lessonContentRepository;
            _mapper = mapper;

            _filesPath = "FilesUploaded";
        }

        public async Task<LessonContent> GetByIdAsync(int id)
        {
            return await lessonContentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LessonContent>> GetAllAsync()
        {
            return await lessonContentRepository.GetAllAsync();
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


            return await lessonContentRepository.CreateAsync(lessonContent);
        }
        public async Task<LessonContent> UpdateAsync(int id, LessonContentRequest lessonContentDto)
        {

            var existingLessonContent = await lessonContentRepository.GetByIdAsync(id);
            if (existingLessonContent == null) return null;

            string newFilePath = string.Empty;
            if (lessonContentDto.Link != null)
            {
                var link = $"{Guid.NewGuid()}{Path.GetExtension(lessonContentDto.Link.FileName)}";
                newFilePath = Path.Combine(_filesPath, link);
            }

            existingLessonContent.Name = lessonContentDto.Name;
            existingLessonContent.Type = lessonContentDto.Type;
            existingLessonContent.Content = lessonContentDto.Content;
            existingLessonContent.LessonId = lessonContentDto.LessonId;

            try
            {
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


                return await lessonContentRepository.UpdateAsync(existingLessonContent);
            }
            catch (Exception e)

            {
                if (newFilePath != null && File.Exists(newFilePath))
                    File.Delete(newFilePath);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {

            var lessonContent = await lessonContentRepository.GetByIdAsync(id);
            if (lessonContent == null) return;

            var filePath = Path.Combine(_filesPath, lessonContent.Link);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await lessonContentRepository.DeleteAsync(lessonContent);

        }
    }
}
