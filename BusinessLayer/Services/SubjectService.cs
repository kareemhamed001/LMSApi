using AutoMapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;


namespace BusinessLayer.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IClassRepository classRepository;
        private readonly ILogger<SubjectService> logger;
        private readonly IMapper mapper;

        public SubjectService(ISubjectRepository subjectRepository, IClassRepository classRepository, ILogger<SubjectService> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.subjectRepository = subjectRepository;
            this.classRepository = classRepository;
        }

        public async Task<IEnumerable<Subject>> Index()
        {
            List<Subject> subjects = (List<Subject>)await subjectRepository.Index();
            return subjects;
        }
        public async Task<Subject?> Show(int id)
        {
            Subject? subject = await subjectRepository.Show(id);
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

            return await subjectRepository.StoreAsync(subject);
        }
        public async Task<Subject> UpdateAsync(int id, UpdateSubjectRequest request)
        {
            var subject = await subjectRepository.Show(id);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            subject.Name = request.Name;
            subject.Description = request.Description;

            return await subjectRepository.UpdateAsync(subject);
        }
        public async Task DeleteAsync(int id)
        {
            var subject = await subjectRepository.Show(id);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            await subjectRepository.DeleteAsync(subject);

        }
        public async Task<List<Class>> ClassesAsync(int subjectId)
        {
            List<Class> Classes = await subjectRepository.ClassesAsync(subjectId);
            return Classes;
        }
        public async Task<List<Course>> CoursesAsync(int subjectId)
        {
            List<Course> Courses = await subjectRepository.CoursesAsync(subjectId);
            return Courses;
        }
        public async Task<List<Student>> StudentsAsync(int subjectId)
        {

            List<Student> Students = await subjectRepository.StudentsAsync(subjectId);
            return Students;
        }

        public async Task<List<Teacher>> TeachersAsync(int subjectId)
        {
            List<Teacher> teachers = await subjectRepository.TeachersAsync(subjectId);
            return teachers;
        }

        public async Task<bool> AddSubjectToClass(AddSubjectToClassRequest request)
        {
            var subject = await subjectRepository.Show(request.SubjectId);
            if (subject == null)
            {
                throw new NotFoundException("Subject not found");
            }
            var @class = await classRepository.GetClassByIdAsync(request.ClassId);
            if (@class == null)
            {
                throw new NotFoundException("Class not found");
            }

            return await subjectRepository.AddSubjectToClass(subject, @class);
        }
    }
}
