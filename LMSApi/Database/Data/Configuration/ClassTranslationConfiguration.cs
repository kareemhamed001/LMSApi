
namespace LMSApi.Database.Data.Configuration
{
    public class ClassTranslationConfiguration : IEntityTypeConfiguration<ClassTranslation>
    {
        public void Configure(EntityTypeBuilder<ClassTranslation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasColumnType("text");

            builder.HasOne(x => x.Class)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.ClassId);


        }
    }
}
