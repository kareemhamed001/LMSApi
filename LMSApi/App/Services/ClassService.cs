using LMSApi.App.Exceptions;
using LMSApi.App.Interfaces;
using LMSApi.Database.Data;
using Microsoft.AspNetCore.Http;

namespace LMSApi.App.Services
{

    public class ClassServices : IClassService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassServices(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Translations.Where(t => t.Language.Code == GetCurrentLanguage()))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null) 
                throw new NotFoundException("Class Not Found");

            return classEntity;
        }
        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task CreateClassAsync(Class classEntity)
        {
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClassAsync(int id, Class classEntity)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null) return;

            existingClass.Name = classEntity.Name;
            existingClass.Description = classEntity.Description;


            _context.Classes.Update(existingClass);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return;

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId);

            return classEntity?.Students ?? new List<Student>();
        }
    }
}