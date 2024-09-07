using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace BusinessLayer.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            this.lessonRepository = lessonRepository;
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            var lesson = await lessonRepository.GetLessonByIdAsync(id);
            if (lesson == null)
            {
                throw new NotFoundException($"Lesson with id {id} not found");
            }
            return lesson;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            return await lessonRepository.GetAllLessonsAsync();
        }

        public async Task<Lesson> CreateLessonAsync(Lesson lesson)
        {
            return await lessonRepository.CreateLessonAsync(lesson);
        }

        public async Task<Lesson> UpdateLessonAsync(int id, Lesson lesson)
        {
            var existingLesson = await lessonRepository.GetLessonByIdAsync(id);
            if (existingLesson == null) return null;

            existingLesson.Name = lesson.Name;
            existingLesson.Description = lesson.Description;
            existingLesson.CourseId = lesson.CourseId;
            existingLesson.SectionNumber = lesson.SectionNumber;


            return await lessonRepository.UpdateLessonAsync(existingLesson);
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await lessonRepository.GetLessonByIdAsync(id);
            if (lesson != null)
            {
                await lessonRepository.DeleteLessonAsync(lesson);
            }
        }
        public async Task<bool> CourseExistsAsync(int courseId)
        {
            return await lessonRepository.CourseExistsAsync(courseId);
        }
    }
}