namespace DataAccessLayer.Data.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.NickName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(t => t.Phone)
                .IsRequired()
                   .HasColumnType("nvarchar(20)");

            builder.Property(t => t.Email)
                .IsRequired()
                   .HasColumnType("nvarchar(100)");

            builder.HasOne(t => t.User)
         .WithOne(u => u.Teacher)
         .HasForeignKey<Teacher>(t => t.UserId);

            builder.HasMany(t => t.Courses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId);

            builder.HasIndex(t => t.Email).IsUnique();
            builder.HasIndex(t => t.UserId).IsUnique();

            builder.ToTable("Teachers");
        }
    }
}
