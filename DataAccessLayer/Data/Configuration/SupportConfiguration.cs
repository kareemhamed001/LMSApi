namespace DataAccessLayer.Data.Configuration
{
    public class SupportConfiguration : IEntityTypeConfiguration<Support>
    {
        public void Configure(EntityTypeBuilder<Support> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.HasOne(builder => builder.User)
                .WithOne(user => user.Support)
                .HasForeignKey<Support>(s => s.UserId);

            builder.ToTable("Supports");
        }
    }
}
