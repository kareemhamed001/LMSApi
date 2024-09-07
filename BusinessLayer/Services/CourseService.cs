using AutoMapper;
using BusinessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMemoryCache _cache;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper mapper;
        public CourseService(ICourseRepository courseRepository, IMemoryCache cache, IMapper mapper)
        {

            _cache = cache;
            _courseRepository = courseRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {

            if (!_cache.TryGetValue(CacheKeys.Courses, out List<Course>? courses))
            {
                courses = (List<Course>)await _courseRepository.GetAllAsync();
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
            Course? course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                throw new NotFoundException("Course not found");
            return course;
        }

        public async Task<Course> AddAsync(CourseRequest courseRequest)
        {
            // Validate Teacher, Subject, and Class existence
            if (courseRequest.TeacherId > 0 && !await _courseRepository.TeacherExistsAsync(courseRequest.TeacherId))
            {
                throw new NotFoundException("Teacher not found");
            }

            if (courseRequest.SubjectId > 0 && !await _courseRepository.SubjectExistsAsync(courseRequest.SubjectId))
            {
                throw new NotFoundException("Subject not found");
            }

            if (courseRequest.ClassId > 0 && !await _courseRepository.ClassExistsAsync(courseRequest.ClassId))
            {
                throw new NotFoundException("Class not found");
            }

            var course = mapper.Map<Course>(courseRequest);
            return await _courseRepository.AddAsync(course);
        }
        public async Task UpdateAsync(CourseRequest course)
        {
            await _courseRepository.UpdateAsync(mapper.Map<Course>(course));
        }
        public async Task DeleteAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                throw new NotFoundException("Course not found");

            await _courseRepository.DeleteAsync(course);
        }

        public async Task<bool> TeacherExistsAsync(int teacherId)
        {
            return await _courseRepository.TeacherExistsAsync(teacherId);
        }

        public async Task<bool> SubjectExistsAsync(int subjectId)
        {
            return await _courseRepository.SubjectExistsAsync(subjectId);
        }

        public async Task<bool> ClassExistsAsync(int classId)
        {
            return await _courseRepository.ClassExistsAsync(classId);
        }

    }
}
