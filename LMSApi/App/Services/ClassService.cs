using LMSApi.App.Interfaces.Class;
using LMSApi.Database.Data;

namespace LMSApi.App.Services
{

    public class ClassServices : IClassService
    {
        private readonly AppDbContext _context;

        public ClassServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            return await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
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