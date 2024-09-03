namespace DataAccessLayer.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(20);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasColumnType("text");

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    j => j.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId),
                    j => j.HasOne(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId),
                    j => j.ToTable("UserRoles")
                );



            builder.ToTable("Users");
        }
    }
}
