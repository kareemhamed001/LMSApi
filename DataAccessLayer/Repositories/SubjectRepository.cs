
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext appDbContext;

        public SubjectRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Subject>> Index()
        {
            List<Subject> subjects = await appDbContext.Subjects.ToListAsync();
            return subjects;
        }
        public async Task<Subject?> Show(int id)
        {
            Subject? subject = await appDbContext.Subjects.FindAsync(id);
            return subject;
        }
        public async Task<Subject> StoreAsync(Subject request)
        {
            Subject subject = new Subject
            {
                Id = 0,
                Name = request.Name,
                Description = request.Description
            };

            appDbContext.Subjects.Add(subject);
            await appDbContext.SaveChangesAsync();
            return subject;
        }
        public async Task<Subject> UpdateAsync(Subject subject)
        {
            appDbContext.Subjects.Update(subject);
            await appDbContext.SaveChangesAsync();
            return subject;
        }
        public async Task DeleteAsync(Subject subject)
        {
            appDbContext.Subjects.Remove(subject);
            await appDbContext.SaveChangesAsync();
        }
        public async Task<List<Class>> ClassesAsync(int subjectId)
        {
            var subjects = await appDbContext.Subjects.Include(s => s.Classes).Where(s => s.Id == subjectId).FirstAsync();

            if (subjects == null)
            {
                return null;
            }
            List<Class> Classes = subjects.Classes;
            return Classes;
        }
        public async Task<List<Course>> CoursesAsync(int subjectId)
        {
            var subject = await appDbContext.Subjects
                .Include(s => s.Courses)
                .ThenInclude(c => c.Teacher)
                .Select(selector: s => new Subject
                {
                    Id = s.Id,
                    Courses = s.Courses.Select(c => new Course
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        TeacherId = c.TeacherId,
                        Teacher = new Teacher
                        {
                            Id = c.Teacher.Id,
                            NickName = c.Teacher.NickName,
                            Email = c.Teacher.Email
                        }
                    }).ToList()
                })
                .Where(s => s.Id == subjectId)
                .FirstOrDefaultAsync();
            if (subject == null)
            {
                return null;
            }
            List<Course> Courses = subject.Courses;
            return Courses;
        }
        public async Task<List<Student>> StudentsAsync(int subjectId)
        {
            var subjects = await appDbContext.Subjects.Include(s => s.Students).Where(s => s.Id == subjectId).FirstAsync();

            if (subjects == null)
            {
                return null;
            }
            List<Student> Students = subjects.Students;
            return Students;
        }

        public async Task<List<Teacher>> TeachersAsync(int subjectId)
        {
            var subjects = await appDbContext.Subjects.Include(s => s.Teachers).Where(s => s.Id == subjectId).FirstAsync();

            if (subjects == null)
            {
                return null;
            }
            List<Teacher> teachers = subjects.Teachers;
            return teachers;
        }

        public async Task<bool> AddSubjectToClass(Subject subject, Class classEntity)
        {

            var @class = await appDbContext.Classes
                .Include(c => c.Subjects.Where(s => s.Id == subject.Id))
                .Where(c => c.Id == classEntity.Id)
                .FirstAsync();

            if (!@class.Subjects.Contains(subject))
            {
                @class.Subjects.Add(subject);
                await appDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
