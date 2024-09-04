namespace BusinessLayer.Responses
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }
        public StudentResponse Student { get; set; } = null!;
        public CourseResponse Course { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
