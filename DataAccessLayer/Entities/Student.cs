namespace DataAccessLayer.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string ParentPhone { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Subject> Subjects { get; set; }
        public List<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();


    }
}
