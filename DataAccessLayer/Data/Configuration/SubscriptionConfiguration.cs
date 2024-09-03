namespace DataAccessLayer.Data.Configuration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(s => s.ExpiryDate).HasColumnType("datetime2").IsRequired();


            builder.HasOne(s => s.Student)
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(s => s.StudentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Course)
               .WithMany(s => s.Subscriptions)
               .HasForeignKey(s => s.CourseId)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
