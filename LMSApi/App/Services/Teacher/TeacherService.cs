using LMSApi.App.Enums;
using LMSApi.App.Interfaces.Teacher;
using LMSApi.App.Requests.Teacher;
using LMSApi.App.Responses.Teacher;
using LMSApi.Database.Data;
using LMSApi.Database.Enitities;
using TeacherEntity = LMSApi.Database.Enitities.Teacher;

namespace LMSApi.App.Services.Teacher
{
    public class TeacherService(AppDbContext appDbContext) : ITeacherService
    {
        private readonly AppDbContext appDbContext = appDbContext;
        public async Task<(ServicesMethodsResponsesEnum, List<TeacherIndexResponse>?)> Index()
        {
            try
            {

                return (ServicesMethodsResponsesEnum.Success, await appDbContext.Teachers.Select(t => new TeacherIndexResponse
                {
                    Id = t.Id,
                    NickName = t.NickName,
                    Email = t.User.Email,
                    Phone = t.User.Phone,
                }).ToListAsync());

            }
            catch (Exception e)
            {
                return (ServicesMethodsResponsesEnum.Exception, null);
            }
        }
        public async Task<ServicesMethodsResponsesEnum> Delete(int teacherId)
        {

            try
            {

                TeacherEntity? teacher = await appDbContext.Teachers.FindAsync(teacherId);
                if (teacher is null)
                {
                    return ServicesMethodsResponsesEnum.NotFound;
                }
                appDbContext.Teachers.Remove(teacher);
                await appDbContext.SaveChangesAsync();
                return ServicesMethodsResponsesEnum.Success;

            }
            catch (Exception)
            {
                return ServicesMethodsResponsesEnum.Exception;
            }
        }
        public async Task<(ServicesMethodsResponsesEnum, TeacherEntity?)> Show(int teacherId)
        {
            try
            {

                TeacherEntity? teacher = await appDbContext.Teachers.FindAsync(teacherId);
                if (teacher is null)
                {
                    return (ServicesMethodsResponsesEnum.NotFound, null);
                }

                return (ServicesMethodsResponsesEnum.Success, teacher);

            }
            catch (Exception)
            {
                return (ServicesMethodsResponsesEnum.Exception, null);
            }
        }
        public async Task<(ServicesMethodsResponsesEnum, TeacherEntity?, string message)> Store(CreateTeacherRequest teacherRequest)
        {
            try
            {
                if (await appDbContext.Users.AnyAsync(u => u.Email == teacherRequest.Email))
                {
                    return (ServicesMethodsResponsesEnum.Exception, null, "Email already exists");
                }
                if (await appDbContext.Teachers.AnyAsync(u => u.Email == teacherRequest.CommunicationEmail))
                {
                    return (ServicesMethodsResponsesEnum.Exception, null, "Communication Email already exists");
                }

                appDbContext.Database.BeginTransaction();
                User user = new User()
                {
                    FirstName = teacherRequest.FirstName,
                    LastName = teacherRequest.LastName,
                    Email = teacherRequest.Email,
                    Phone = teacherRequest.Phone,
                    Password = teacherRequest.Password,
                };
                appDbContext.Users.Add(user);
                await appDbContext.SaveChangesAsync();


                TeacherEntity teacher = new TeacherEntity
                {
                    NickName = teacherRequest.NickName,
                    Phone = teacherRequest.CommunicationPhone ?? teacherRequest.Email,
                    Email = teacherRequest.CommunicationEmail ?? teacherRequest.Phone,
                    User=user
                };

                appDbContext.Teachers.Add(teacher);
                await appDbContext.SaveChangesAsync();

                appDbContext.Database.CommitTransaction();

                return (ServicesMethodsResponsesEnum.Success, teacher, "Success");

            }
            catch (Exception e)
            {
                appDbContext.Database.RollbackTransaction();
                return (ServicesMethodsResponsesEnum.Exception, null, e.Message);
            }
        }
        public async Task<(ServicesMethodsResponsesEnum, TeacherEntity?)> Update(int teacherId, UpdateTeacherRequest teacherRequest)
        {
            try
            {

                appDbContext.Database.BeginTransaction();
                TeacherEntity? teacher = await appDbContext.Teachers
                    .Include(t => t.User)
                    .FirstAsync(t => t.Id == teacherId);
                if (teacher is null)
                {
                    return (ServicesMethodsResponsesEnum.NotFound, null);
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

                return (ServicesMethodsResponsesEnum.Success, teacher);
            }

            catch (Exception)
            {
                appDbContext.Database.RollbackTransaction();
                return (ServicesMethodsResponsesEnum.Exception, null);
            }
        }
    }
}
