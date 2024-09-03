
namespace DataAccessLayer.Data.Configuration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasColumnType("nvarchar");

            builder.HasMany(c => c.Students)
                .WithOne(s => s.Class)
                .HasForeignKey(s => s.ClassId);

            builder.HasMany(c => c.Courses)
                .WithOne(c => c.Class)
                .HasForeignKey(c => c.ClassId);


            builder.HasMany(c => c.Translations)
                .WithOne(c => c.Class)
                .HasForeignKey(c => c.ClassId);


            builder.HasMany(c => c.Subjects)
                .WithMany(s => s.Classes)
                .UsingEntity<ClassSubject>(
                j => j.HasOne(cs => cs.Subject)
                    .WithMany(s => s.ClassSubjects)
                    .HasForeignKey(cs => cs.SubjectId),
                j => j.HasOne(cs => cs.Class)
                .WithMany(c => c.ClassSubjects)
                    .HasForeignKey(cs => cs.ClassId)
                );



            builder.ToTable("Classes");


        }
    }
}
