using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMemoryCache _cache;
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository, IMemoryCache cache)
        {

            _cache = cache;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {

            if (!_cache.TryGetValue(CacheKeys.Courses, out List<Course>? courses))
            {
                courses = await _context.Courses.ToListAsync();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
                cacheOptions.SetSlidingExpiration(TimeSpan.FromSeconds(45));
                cacheOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                cacheOptions.SetPriority(CacheItemPriority.Normal);

                _cache.Set(CacheKeys.Courses, courses, cacheOptions);
            }

            return courses;
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TeacherExistsAsync(int teacherId)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == teacherId);
        }

        public async Task<bool> SubjectExistsAsync(int subjectId)
        {
            return await _context.Subjects.AnyAsync(s => s.Id == subjectId);
        }

        public async Task<bool> ClassExistsAsync(int classId)
        {
            return await _context.Classes.AnyAsync(c => c.Id == classId);
        }

    }
}
