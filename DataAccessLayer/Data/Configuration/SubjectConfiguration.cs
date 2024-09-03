namespace DataAccessLayer.Data.Configuration
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(50);
            builder.Property(s => s.Description).IsRequired().HasColumnType("text");

            builder.HasMany(s => s.Classes)
                .WithMany(c => c.Subjects)
               .UsingEntity<ClassSubject>(
                   j => j.HasOne(cs => cs.Class)
                       .WithMany(c => c.ClassSubjects)
                       .HasForeignKey(cs => cs.ClassId),
                   j => j.HasOne(cs => cs.Subject)
                       .WithMany(s => s.ClassSubjects)
                       .HasForeignKey(cs => cs.SubjectId)
               );

            builder.HasMany(s => s.Courses)
                .WithOne(c => c.Subject)
                .HasForeignKey(c => c.SubjectId);

            builder.HasMany(s => s.Students)
                .WithMany(s => s.Subjects)
                .UsingEntity<StudentSubject>(
                    j => j.HasOne(ss => ss.Student)
                        .WithMany(s => s.StudentSubjects)
                        .HasForeignKey(ss => ss.StudentId),
                    j => j.HasOne(ss => ss.Subject)
                        .WithMany(s => s.StudentSubjects)
                        .HasForeignKey(ss => ss.SubjectId)
                );

            builder.HasMany(s => s.Teachers)
                .WithMany(t => t.Subjects)
                .UsingEntity<TeacherSubject>(
                    j => j.HasOne(ts => ts.Teacher)
                        .WithMany(t => t.TeacherSubjects)
                        .HasForeignKey(ts => ts.TeacherId),
                    j => j.HasOne(ts => ts.Subject)
                        .WithMany(s => s.TeacherSubjects)
                        .HasForeignKey(ts => ts.SubjectId)
                );

        }
    }
}
