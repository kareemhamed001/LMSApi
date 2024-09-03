namespace DataAccessLayer.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.ParentPhone).IsRequired().HasMaxLength(20);

            builder.HasOne(t => t.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(t => t.UserId);

            builder.HasMany(t => t.Subjects)
                .WithMany(c => c.Students)
                .UsingEntity<StudentSubject>(
                    j => j.HasOne(ur => ur.Subject)
                        .WithMany(r => r.StudentSubjects)
                        .HasForeignKey(ur => ur.SubjectId),
                    j => j.HasOne(ur => ur.Student)
                        .WithMany(u => u.StudentSubjects)
                        .HasForeignKey(ur => ur.StudentId)
                );

            builder.HasOne(t => t.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(c => c.ClassId);


            builder.ToTable("Students");


        }
    }
}
