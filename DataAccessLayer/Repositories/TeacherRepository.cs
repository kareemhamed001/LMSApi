
using DataAccessLayer.Exceptions;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TeacherRepository(AppDbContext appDbContext) : ITeacherRepository
    {
        private readonly AppDbContext appDbContext = appDbContext;
        public async Task<List<Teacher>> Index()
        {
            try
            {
                var teachers = await appDbContext.Teachers.ToListAsync();
                return teachers;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> Delete(Teacher teacher)
        {
            appDbContext.Teachers.Remove(teacher);
            await appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<Teacher> Show(int teacherId)
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
                return null;
            }

            return teacher;
        }

        public Teacher? GetTeacher(Func<Teacher, bool> condition)
        {
            return appDbContext.Teachers.Where(condition).FirstOrDefault();
        }


        public async Task<Teacher> Store(Teacher teacherRequest)
        {
            try
            {
                if (await appDbContext.Users.AnyAsync(u => u.Email == teacherRequest.Email))
                {
                    throw new ForbidenException("Email already exists");
                }
                if (await appDbContext.Teachers.AnyAsync(u => u.Email == teacherRequest.Email))
                {
                    throw new ForbidenException("Communication Email already exists");
                }

                var teacherRole = await appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Teacher");
                User user = new User()
                {
                    FirstName = teacherRequest.User.FirstName,
                    LastName = teacherRequest.User.LastName,
                    Email = teacherRequest.User.Email,
                    Phone = teacherRequest.User.Phone,
                    Password = teacherRequest.User.Password,
                    Roles = new List<Role> { teacherRole ?? new Role { Name = "Teacher" } }
                };
                appDbContext.Users.Add(user);

                Teacher teacher = new Teacher
                {
                    NickName = teacherRequest.NickName,
                    Phone = teacherRequest.Email ?? teacherRequest.User.Email,
                    Email = teacherRequest.Email ?? teacherRequest.Phone,
                    User = user
                };

                appDbContext.Teachers.Add(teacher);
                await appDbContext.SaveChangesAsync();


                return teacher;

            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<Teacher> Update(Teacher teacher)
        {
            try
            {

                appDbContext.Update(teacher);
                await appDbContext.SaveChangesAsync();

                return teacher;
            }

            catch (Exception)
            {

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
        public async Task<Course> StoreCourseAsync(int loggedUserId, Course storeCourseRequest)
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
                    foreach (Lesson lessonRequest in storeCourseRequest.Lessons)
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
        public async Task<Subject> AddTeacherToSubjectAsync(int loggedUserId, int teacherId, int subjectId)
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
                    if (loggedUser.Teacher.Id != teacherId)
                    {
                        throw new ForbidenException("You are not allowed to add subject for another teacher");
                    }
                    teacher = loggedUser.Teacher;

                }
                else
                {
                    teacher = await appDbContext.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.Id == teacherId);
                }

                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }

                if (teacher.Subjects.Any(s => s.Id == subjectId))
                {
                    throw new ForbidenException("Teacher has this subject already");
                }

                Subject? subject = await appDbContext.Subjects.FindAsync(subjectId);
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
