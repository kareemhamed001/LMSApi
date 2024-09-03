namespace DataAccessLayer.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public decimal Price { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();



    }
}
