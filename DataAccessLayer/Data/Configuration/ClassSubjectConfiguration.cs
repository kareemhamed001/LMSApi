
namespace DataAccessLayer.Data.Configuration
{
    public class ClassSubjectConfiguration : IEntityTypeConfiguration<ClassSubject>
    {
        public void Configure(EntityTypeBuilder<ClassSubject> builder)
        {

            builder.HasKey(cs => new { cs.ClassId, cs.SubjectId });

            builder.HasOne(cs => cs.Class)
                .WithMany(c => c.ClassSubjects)
                .HasForeignKey(cs => cs.ClassId);

            builder.HasOne(cs => cs.Subject)
                .WithMany(s => s.ClassSubjects)
                .HasForeignKey(cs => cs.SubjectId);

            builder.ToTable("ClassSubjects");
        }
    }
}
