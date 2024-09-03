namespace DataAccessLayer.Data.Configuration
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.Category)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.RouteName)
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            builder.HasMany(builder => builder.Roles)
                .WithMany(builder => builder.Permissions)
                .UsingEntity<RolePermission>(
                    j => j.HasOne(rp => rp.Role)
                        .WithMany(r => r.RolePermissions)
                        .HasForeignKey(rp => rp.RoleId),
                    j => j.HasOne(rp => rp.Permission)
                        .WithMany(p => p.RolePermissions)
                        .HasForeignKey(rp => rp.PermissionId)
                  );




        }
    }
}
