namespace DataAccessLayer.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Class> Classes { get; set; }
        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }

        public List<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public List<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public List<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

    }
}
