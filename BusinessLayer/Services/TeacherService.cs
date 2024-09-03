using LMSApi.App.Exceptions;
using LMSApi.App.helper;
using LMSApi.App.Interfaces;
using LMSApi.App.Requests;
using LMSApi.App.Requests.Teacher;

namespace BusinessLayer.Services
{
    public class TeacherService(AppDbContext appDbContext, ILogger<TeacherService> logger, ICacheService cache) : ITeacherService
    {
        private readonly AppDbContext appDbContext = appDbContext;
        private readonly ILogger<TeacherService> logger = logger;
        private readonly ICacheService cache = cache;
        public async Task<List<Teacher>> Index()
        {
            try
            {
                var teachers = cache.Get<List<Teacher>>(CacheKeys.Teachers);
                if (teachers is null || teachers.Count() == 0)
                {
                    teachers = await appDbContext.Teachers.ToListAsync();

                    cache.Set(CacheKeys.Teachers, teachers,DateTimeOffset.Now.AddSeconds(45));
                    logger.LogInformation("Teachers from database");
                }
                else
                {
                    logger.LogInformation("Teachers from cache");
                }

                return teachers;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> Delete(int teacherId)
        {

            try
            {

                Teacher? teacher = await appDbContext.Teachers.FindAsync(teacherId);
                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }
                appDbContext.Teachers.Remove(teacher);
                await appDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Teacher> Show(int teacherId)
        {
            try
            {

                Teacher? teacher = await appDbContext.Teachers
                    .Include(t => t.User)
                    .ThenInclude(u => u.Roles)
                    .ThenInclude(r => r.Permissions)
                    .Include(t => t.Courses)
                    .Include(t => t.Subjects)
                    .Include(t => t.Subscriptions)
                    .FirstOrDefaultAsync(t => t.Id == teacherId);

                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }

                return teacher;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Teacher> Store(CreateTeacherRequest teacherRequest)
        {
            try
            {
                if (await appDbContext.Users.AnyAsync(u => u.Email == teacherRequest.Email))
                {
                    throw new ForbidenException("Email already exists");
                }
                if (await appDbContext.Teachers.AnyAsync(u => u.Email == teacherRequest.CommunicationEmail))
                {
                    throw new ForbidenException("Communication Email already exists");
                }

                var teacherRole = await appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Teacher");
                User user = new User()
                {
                    FirstName = teacherRequest.FirstName,
                    LastName = teacherRequest.LastName,
                    Email = teacherRequest.Email,
                    Phone = teacherRequest.Phone,
                    Password = teacherRequest.Password,
                    Roles = new List<Role> { teacherRole ?? new Role { Name = "Teacher" } }
                };
                appDbContext.Users.Add(user);

                Teacher teacher = new Teacher
                {
                    NickName = teacherRequest.NickName,
                    Phone = teacherRequest.CommunicationPhone ?? teacherRequest.Email,
                    Email = teacherRequest.CommunicationEmail ?? teacherRequest.Phone,
                    User = user
                };

                appDbContext.Teachers.Add(teacher);
                await appDbContext.SaveChangesAsync();


                return teacher;

            }
            catch (Exception e)
            {
                logger.LogCritical("Error while storing teacher {ex}", e.Message);
                throw;
            }
        }
        public async Task<Teacher> Update(int teacherId, UpdateTeacherRequest teacherRequest)
        {
            try
            {

                appDbContext.Database.BeginTransaction();
                Teacher? teacher = await appDbContext.Teachers
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == teacherId);
                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }

                teacher.User.FirstName = teacherRequest.FirstName ?? teacher.User.FirstName;
                teacher.User.LastName = teacherRequest.LastName ?? teacher.User.LastName;
                teacher.User.Email = teacherRequest.Email ?? teacher.User.Email;
                teacher.User.Phone = teacherRequest.Phone ?? teacher.User.Phone;
                teacher.User.Password = teacherRequest.Password ?? teacher.User.Password;

                teacher.NickName = teacherRequest.NickName ?? teacher.NickName;
                teacher.Phone = teacherRequest.CommunicationPhone ?? teacher.Phone;
                teacher.Email = teacherRequest.CommunicationEmail ?? teacher.Email;

                await appDbContext.SaveChangesAsync();
                appDbContext.Database.CommitTransaction();

                return teacher;
            }

            catch (Exception)
            {
                appDbContext.Database.RollbackTransaction();
                throw;
            }
        }
        public async Task<List<Course>> CoursesAsync(int teacherId)
        {
            Teacher? teacher = await appDbContext.Teachers.Include(t => t.Courses).FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher.Courses;
        }
        public async Task<List<Subject>> SubjectsAsync(int teacherId)
        {
            Teacher? teacher = await appDbContext.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher.Subjects;
        }
        public async Task<List<Subscription>> SubscriptionsAsync(int teacherId)
        {
            Teacher? teacher = await appDbContext.Teachers.Include(t => t.Subscriptions).FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher.Subscriptions;
        }
        public async Task<List<Class>> ClassesAsync(int teacherId)
        {
            Teacher? teacher = await appDbContext.Teachers.Include(t => t.Courses)
                .ThenInclude(c => c.Class).FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }

            List<Class> classes = new List<Class>();
            foreach (Course course in teacher.Courses)
            {
                classes.Add(course.Class);
            }
            return classes;
        }
        public async Task<Course> StoreCourseAsync(int loggedUserId, StoreCourseRequest storeCourseRequest)
        {
            try
            {

                var loggedUser = await appDbContext.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Teacher)
                    .ThenInclude(t => t.Subjects.Where(s => s.Id == storeCourseRequest.SubjectId)) // Filtering subjects
                    .FirstOrDefaultAsync(u => u.Id == loggedUserId);


                if (loggedUser is null)
                {
                    throw new NotFoundException("User not found");
                }
                Teacher? teacher;
                if (loggedUser.Teacher is not null)
                {
                    if (loggedUser.Teacher.Id != storeCourseRequest.TeacherId)
                    {
                        throw new ForbidenException("You are not allowed to store course for another teacher");
                    }
                    teacher = loggedUser.Teacher;

                }
                else
                {
                    teacher = await appDbContext.Teachers
                        .Include(t => t.Subjects.Where(s => s.Id == storeCourseRequest.SubjectId))
                        .FirstOrDefaultAsync(t => t.Id == storeCourseRequest.TeacherId);
                }

                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }

                if (teacher.Subjects.Count == 0)
                {
                    throw new ForbidenException("Teacher has no such subject");
                }
                Subject subject = teacher.Subjects.First();
                //load subject class
                await appDbContext.Entry(subject).Collection(s => s.Classes).LoadAsync();

                if (subject.Classes is null || !subject.Classes.Any(c => c.Id == storeCourseRequest.ClassId))
                {
                    throw new NotFoundException("This Subject is not in this class");
                }


                Course course = new Course
                {
                    Name = storeCourseRequest.Name,
                    Description = storeCourseRequest.Description,
                    Price = storeCourseRequest.Price,
                    Teacher = teacher,
                    SubjectId = storeCourseRequest.SubjectId,
                    ClassId = storeCourseRequest.ClassId
                };

                if (storeCourseRequest.Lessons.Count > 0)
                {
                    foreach (LessonRequestBase lessonRequest in storeCourseRequest.Lessons)
                    {

                        List<LessonContent> lessonContents = new List<LessonContent>();
                        lessonContents.AddRange(lessonRequest.LessonContents.Select(lc => new LessonContent
                        {
                            Name = lc.Name,
                            Type = lc.Type,
                            Content = lc.Content,
                            Link = lc.Link
                        }));

                        Lesson lesson = new Lesson
                        {
                            Name = lessonRequest.Name,
                            Description = lessonRequest.Description,
                            SectionNumber = lessonRequest.SectionNumber,
                            LessonContents = lessonContents
                        };
                        course.Lessons.Add(lesson);
                    }

                }
                appDbContext.Courses.Add(course);
                await appDbContext.SaveChangesAsync();
                return course;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Subject> AddTeacherToSubjectAsync(int loggedUserId, AddTeacherToSubjectRequest addTeacherToSubjectRequest)
        {
            try
            {

                var loggedUser = await appDbContext.Users.Include(u => u.Roles)
                    .Include(u => u.Teacher)
                    .ThenInclude(t => t.Subjects)
                    .FirstOrDefaultAsync(u => u.Id == loggedUserId);

                if (loggedUser is null)
                {
                    throw new NotFoundException("User not found");
                }
                Teacher? teacher;
                if (loggedUser.Teacher is not null)
                {
                    if (loggedUser.Teacher.Id != addTeacherToSubjectRequest.TeacherId)
                    {
                        throw new ForbidenException("You are not allowed to add subject for another teacher");
                    }
                    teacher = loggedUser.Teacher;

                }
                else
                {
                    teacher = await appDbContext.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.Id == addTeacherToSubjectRequest.TeacherId);
                }

                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }

                if (teacher.Subjects.Any(s => s.Id == addTeacherToSubjectRequest.SubjectId))
                {
                    throw new ForbidenException("Teacher has this subject already");
                }

                Subject? subject = await appDbContext.Subjects.FindAsync(addTeacherToSubjectRequest.SubjectId);
                if (subject is null)
                {
                    throw new NotFoundException("Subject not found");
                }

                teacher.Subjects.Add(subject);
                await appDbContext.SaveChangesAsync();
                return subject;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
