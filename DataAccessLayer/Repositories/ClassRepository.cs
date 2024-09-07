using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Repositories
{

    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ClassRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        private string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Translations.Where(t => t.Language.Code == GetCurrentLanguage()))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
                return null;

            return classEntity;
        }
        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            //get current language or default language if not found
            Language? language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == GetCurrentLanguage());
            if (language == null)
                language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == _configuration["DefaultLanguage"]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"The default languae is {_configuration["DefaultLanguage"]} and selected languge from database is {language.Code} and needed from header language is {GetCurrentLanguage()}");
            Console.ResetColor();
            List<Class> classes = new();
            if (language.Code == _configuration["DefaultLanguage"])
            {
                classes = await _context.Classes.ToListAsync();
            }
            else
            {
                classes = await _context.Classes.Include(c => c.Translations.Where(t => t.LanguageId == language.Id))
                   .Select(c => new Class
                   {
                       Id = c.Id,
                       Name = c.Translations.FirstOrDefault().Name ?? c.Name,
                       Description = c.Translations.FirstOrDefault().Description ?? c.Description
                   })
                   .ToListAsync();
            }
            return classes;
        }

        public async Task<Class> CreateClassAsync(Class classEntity)
        {
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Class> UpdateClassAsync(Class classEntity)
        {

            _context.Classes.Update(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<bool> DeleteClassAsync(Class classEntity)
        {
            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId);

            return classEntity?.Students ?? [];
        }
        public async Task<IEnumerable<Course>> GetCoursesByClassIdAsync(int classId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Courses)
                .ThenInclude(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == classId);

            return classEntity?.Courses ?? [];
        }

        public async Task<IEnumerable<Teacher>> GetTeachersByClassIdAsync(int classId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Courses)
                .ThenInclude(c => c.Teacher)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null) return new List<Teacher>();

            var teachers = classEntity.Courses.Select(c => c.Teacher).ToList();

            return teachers;
        }
    }
}