namespace DataAccessLayer.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
