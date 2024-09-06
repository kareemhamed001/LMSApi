
using DataAccessLayer.Interfaces;
using AutoMapper;


namespace BusinessLayer.Services
{

    public class ClassServices(IClassRepository classRepository, IMapper mapper) : IClassService
    {
        private string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
        public async Task<Class> GetClassByIdAsync(int id)
        {
            Class classEntity = await classRepository.GetClassByIdAsync(id);
            if (classEntity is null)
                throw new NotFoundException("Class Not Found");
            return classEntity;
        }
        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return (List<Class>)await classRepository.GetAllClassesAsync();
        }

        public async Task<Class> CreateClassAsync(ClassRequest classRequest)
        {
            Class classEntity = mapper.Map<Class>(classRequest);
            return await classRepository.CreateClassAsync(classEntity);
        }

        public async Task<Class> UpdateClassAsync(int id, ClassRequest classRequest)
        {
            var existingClass = await classRepository.GetClassByIdAsync(id);
            if (existingClass is null)
                throw new NotFoundException("Class Not Found");


            existingClass.Name = classRequest.Name;
            existingClass.Description = classRequest.Description;

            if (classRequest.Translations is not null)
            {
                //update or add translations
                foreach (var translation in classRequest.Translations)
                {
                    var existingTranslation = existingClass.Translations.FirstOrDefault(t => t.LanguageId == translation.LanguageId);
                    if (existingTranslation == null)
                    {
                        existingClass.Translations.Add(new ClassTranslation
                        {
                            Name = translation.Name,
                            Description = translation.Description,
                            LanguageId = translation.LanguageId
                        });
                    }
                    else
                    {
                        existingTranslation.Name = translation.Name;
                        existingTranslation.Description = translation.Description;
                    }
                }
            }


            return await classRepository.UpdateClassAsync(existingClass);
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var existingClass = await classRepository.GetClassByIdAsync(id);
            if (existingClass is null)
                throw new NotFoundException("Class Not Found");

            return await classRepository.DeleteClassAsync(existingClass);
        }
        public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId)
        {
            return (List<Student>)await classRepository.GetStudentsByClassIdAsync(classId);
        }
        public async Task<IEnumerable<Course>> GetCoursesByClassIdAsync(int classId)
        {
            return (List<Course>)await classRepository.GetCoursesByClassIdAsync(classId);
        }
        public async Task<IEnumerable<Teacher>> GetTeachersByClassIdAsync(int classId)
        {
            return (List<Teacher>)await classRepository.GetTeachersByClassIdAsync(classId);
        }


    }
}