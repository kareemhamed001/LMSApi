namespace DataAccessLayer.Data.Configuration
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.Name).IsRequired().HasMaxLength(50);
            builder.Property(l => l.Code).IsRequired().HasMaxLength(5);
            builder.ToTable("Languages");
        }
    }
}
