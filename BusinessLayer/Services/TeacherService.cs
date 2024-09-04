using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ILogger<TeacherService> logger;
        private readonly ICacheService cache;
        private readonly string _filesPath = "FilesUploaded";

        public TeacherService(ITeacherRepository teacherRepository, IUserRepository userRepository, IRoleRepository roleRepository, ICourseRepository courseRepository, ILogger<TeacherService> logger, ICacheService cache)
        {
            this.teacherRepository = teacherRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.logger = logger;
            this.cache = cache;
            this.courseRepository = courseRepository;
        }

        public async Task<List<Teacher>> Index()
        {
            try
            {
                var teachers = cache.Get<List<Teacher>>(CacheKeys.Teachers);
                if (teachers is null || teachers.Count() == 0)
                {
                    teachers = await teacherRepository.Index();
                    cache.Set(CacheKeys.Teachers, teachers, DateTimeOffset.Now.AddSeconds(45));
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

                Teacher? teacher = await teacherRepository.Show(teacherId);
                if (teacher is null)
                {
                    throw new NotFoundException("Teacher not found");
                }
                return await teacherRepository.Delete(teacher);

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

                Teacher? teacher = await teacherRepository.Show(teacherId);

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
                if (userRepository.GetUser(u => u.Email == teacherRequest.Email) is not null)
                {
                    throw new ForbidenException("Email already exists");
                }
                if (teacherRepository.GetTeacher(u => u.Email == teacherRequest.CommunicationEmail) is not null)
                {
                    throw new ForbidenException("Communication Email already exists");
                }

                var teacherRole = roleRepository.GetRole(r => r.Name == "Teacher");
                User user = new User()
                {
                    FirstName = teacherRequest.FirstName,
                    LastName = teacherRequest.LastName,
                    Email = teacherRequest.Email,
                    Phone = teacherRequest.Phone,
                    Password = teacherRequest.Password,
                    Roles = new List<Role> { teacherRole ?? new Role { Name = "Teacher" } }
                };

                await userRepository.StoreAsync(user);

                Teacher teacher = new Teacher
                {
                    NickName = teacherRequest.NickName,
                    Phone = teacherRequest.CommunicationPhone ?? teacherRequest.Email,
                    Email = teacherRequest.CommunicationEmail ?? teacherRequest.Phone,
                    User = user
                };

                await teacherRepository.Store(teacher);


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
                Teacher? teacher = await teacherRepository.Show(teacherId);
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

                await teacherRepository.Update(teacher);

                return teacher;
            }

            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Course>> CoursesAsync(int teacherId)
        {
            Teacher? teacher = await teacherRepository.Show(teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher.Courses;
        }
        public async Task<List<Subject>> SubjectsAsync(int teacherId)
        {
            Teacher? teacher = await teacherRepository.Show(teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher.Subjects;
        }
        public async Task<List<Subscription>> SubscriptionsAsync(int teacherId)
        {
            Teacher? teacher = await teacherRepository.Show(teacherId);
            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }
            return teacher.Subscriptions;
        }
        public async Task<List<Class>> ClassesAsync(int teacherId)
        {
            Teacher? teacher = await teacherRepository.Show(teacherId);
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

                User? loggedUser = await userRepository.GetUserByIdAsync(loggedUserId);


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
                    teacher = await teacherRepository.Show(storeCourseRequest.TeacherId);
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


                        foreach (LessonContentRequest content in lessonRequest.LessonContents)
                        {
                            var link = $"{Guid.NewGuid()}{Path.GetExtension(content.Link.FileName)}";
                            var path = Path.Combine(_filesPath, link);

                            using (var stream = File.Create(path))
                            {
                                {
                                    await stream.CopyToAsync(stream);
                                }
                            }

                            lessonContents.Add(new LessonContent
                            {
                                Name = content.Name,
                                Type = content.Type,
                                Content = content.Content,
                                Link = link
                            });
                        }
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
                await courseRepository.AddAsync(course);
                return course;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public async Task<Subject> AddTeacherToSubjectAsync(int loggedUserId, AddTeacherToSubjectRequest addTeacherToSubjectRequest)
        //{
        //    try
        //    {

        //        var loggedUser = await appDbContext.Users.Include(u => u.Roles)
        //            .Include(u => u.Teacher)
        //            .ThenInclude(t => t.Subjects)
        //            .FirstOrDefaultAsync(u => u.Id == loggedUserId);

        //        if (loggedUser is null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }
        //        Teacher? teacher;
        //        if (loggedUser.Teacher is not null)
        //        {
        //            if (loggedUser.Teacher.Id != addTeacherToSubjectRequest.TeacherId)
        //            {
        //                throw new ForbidenException("You are not allowed to add subject for another teacher");
        //            }
        //            teacher = loggedUser.Teacher;

        //        }
        //        else
        //        {
        //            teacher = await appDbContext.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.Id == addTeacherToSubjectRequest.TeacherId);
        //        }

        //        if (teacher is null)
        //        {
        //            throw new NotFoundException("Teacher not found");
        //        }

        //        if (teacher.Subjects.Any(s => s.Id == addTeacherToSubjectRequest.SubjectId))
        //        {
        //            throw new ForbidenException("Teacher has this subject already");
        //        }

        //        Subject? subject = await appDbContext.Subjects.FindAsync(addTeacherToSubjectRequest.SubjectId);
        //        if (subject is null)
        //        {
        //            throw new NotFoundException("Subject not found");
        //        }

        //        teacher.Subjects.Add(subject);
        //        await appDbContext.SaveChangesAsync();
        //        return subject;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
