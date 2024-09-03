namespace DataAccessLayer.Data.Configuration
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).IsRequired().HasMaxLength(50);
            builder.Property(l => l.Description).IsRequired().HasColumnType("text");
            builder.Property(l => l.SectionNumber).IsRequired(false).HasColumnType("integer");

            builder.HasOne(l => l.Course)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseId);

        }
    }
}
