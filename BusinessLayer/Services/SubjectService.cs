using LMSApi.App.Interfaces;
using LMSApi.App.Requests.Subject;
using LMSApi.App.Exceptions;

using LMSApi.App.Responses;
using LMSApi.App.Responses.Teacher;
using AutoMapper;
using LMSApi.App.Requests;

namespace BusinessLayer.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger<SubjectService> logger;
        private readonly IMapper mapper;

        public SubjectService(AppDbContext appDbContext, ILogger<SubjectService> logger, IMapper mapper)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Subject>> Index()
        {
            List<Subject> subjects = await appDbContext.Subjects.ToListAsync();
            return subjects;
        }
        public async Task<Subject?> Show(int id)
        {
            Subject? subject = await appDbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            return subject;
        }
        public async Task<Subject> StoreAsync(SubjectRequest request)
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
        public async Task<Subject> UpdateAsync(int id, UpdateSubjectRequest request)
        {
            var subject = await appDbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            subject.Name = request.Name;
            subject.Description = request.Description;
            await appDbContext.SaveChangesAsync();
            return subject;
        }
        public async Task DeleteAsync(int id)
        {
            var subject = await appDbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            appDbContext.Subjects.Remove(subject);
            await appDbContext.SaveChangesAsync();
        }
        public async Task<List<Class>> ClassesAsync(int subjectId)
        {
            var subjects = await appDbContext.Subjects.Include(s => s.Classes).Where(s => s.Id == subjectId).FirstAsync();

            if (subjects == null)
            {
                throw new NotFoundException("Subject not found");
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
                throw new NotFoundException("Subject not found");
            }
            List<Course> Courses = subject.Courses;
            return Courses;
        }
        public async Task<List<Student>> StudentsAsync(int subjectId)
        {
            var subjects = await appDbContext.Subjects.Include(s => s.Students).Where(s => s.Id == subjectId).FirstAsync();

            if (subjects == null)
            {
                throw new NotFoundException("Subject not found");
            }
            List<Student> Students = subjects.Students;
            return Students;
        }

        public async Task<List<Teacher>> TeachersAsync(int subjectId)
        {
            var subjects = await appDbContext.Subjects.Include(s => s.Teachers).Where(s => s.Id == subjectId).FirstAsync();

            if (subjects == null)
            {
                throw new NotFoundException("Subject not found");
            }
            List<Teacher> teachers = subjects.Teachers;
            return teachers;
        }

        public async Task<bool> AddSubjectToClass(AddSubjectToClassRequest request)
        {
            var subject = await appDbContext.Subjects.FindAsync(request.SubjectId);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            var classs = await appDbContext.Classes
                .Include(c => c.Subjects)
                .Where(c => c.Id == request.ClassId)
                .FirstAsync();
            if (classs == null)
            {
                throw new NotFoundException("Class not found");
            }


            if (!classs.Subjects.Contains(subject))
            {
                classs.Subjects.Add(subject);
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
