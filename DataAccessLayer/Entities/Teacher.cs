namespace DataAccessLayer.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string NickName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

    }
}
