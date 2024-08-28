using LMSApi.App.Enums;
using LMSApi.App.Interfaces;
using LMSApi.Database.Data;

namespace LMSApi.App.Services
{
    public class CourseService:ICourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
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
