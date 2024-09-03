namespace DataAccessLayer.Entities
{
    public class Support
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
