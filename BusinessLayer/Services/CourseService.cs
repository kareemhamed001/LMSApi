﻿using AutoMapper;
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
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(CourseRequest course)
        {
            await _courseRepository.AddAsync(mapper.Map<Course>(course));
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