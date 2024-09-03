using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Class : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public List<ClassTranslation> Translations { get; set; } = new List<ClassTranslation>();

    }
}
