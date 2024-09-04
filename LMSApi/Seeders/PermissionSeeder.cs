using LMSApi.App.Atrributes;
using System.Reflection;

namespace LMSApi.Seeders
{
    public class PermissionSeeder(AppDbContext appDbContext)
    {

        private readonly List<Permission> permissions = new List<Permission>();

        private readonly Dictionary<string, List<string>> Roles = new Dictionary<string, List<string>>()
        {
            {"Admin",new List<string>{"*"} },
            {"Teacher",new List<string>{
                "teachers.show",
                "teachers.update",
                "teachers.list_courses",
                "teachers.list_subjects",
                "teachers.list_subcriptions",
                "teachers.list_classes",
            }
            }
        };
        public async Task Seed()
        {
            InspectControllersPermissions();
            using (appDbContext)
            {
                foreach (var permission in permissions)
                {
                    if (!appDbContext.Permissions.Any(p => p.Name == permission.Name))
                    {
                        appDbContext.Permissions.Add(permission);
                    }
                }
                await appDbContext.SaveChangesAsync();
                await SeedRoles();
                //await Task.WhenAll(appDbContext.SaveChangesAsync(), SeedRoles());

            }
        }

        public async Task SeedRoles()
        {
            using (appDbContext)
            {


                var StoredPermissions = await appDbContext.Permissions.ToListAsync();
                foreach (var role in Roles)
                {
                    Role? existingRole = await appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == role.Key);
                    if (existingRole is null)
                    {
                        existingRole = new Role { Name = role.Key };
                        appDbContext.Roles.Add(existingRole);
                        await appDbContext.SaveChangesAsync();
                    }

                    foreach (var permission in role.Value)
                    {
                        if (permission == "*")
                        {
                            foreach (var p in StoredPermissions)
                            {
                                if (!appDbContext.RolePermissions.Any(rp => rp.RoleId == existingRole.Id && rp.PermissionId == p.Id))
                                {
                                    appDbContext.RolePermissions.Add(new RolePermission { RoleId = existingRole.Id, PermissionId = p.Id });
                                }
                            }
                        }
                        else
                        {
                            var permissionEntity = StoredPermissions.Count > 0 ? StoredPermissions.FirstOrDefault(p => p.RouteName == permission) : null;
                            if (permissionEntity is not null)
                            {
                                if (!appDbContext.RolePermissions.Any(rp => rp.RoleId == existingRole.Id && rp.PermissionId == permissionEntity.Id))
                                {
                                    appDbContext.RolePermissions.Add(new RolePermission { RoleId = existingRole.Id, PermissionId = permissionEntity.Id });
                                }
                            }
                        }


                    }

                }
                await appDbContext.SaveChangesAsync();
            }
        }

        private void InspectControllersPermissions()
        {

            var assembly = Assembly.GetExecutingAssembly();

            var controllerTypes = assembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var controller in controllerTypes)
            {
                Console.WriteLine($"Controller: {controller.Name}");

                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

                foreach (var method in methods)
                {
                    Console.WriteLine($" - Method: {method.Name}");

                    var checkPermissionAttributes = method.GetCustomAttributes(typeof(CheckPermissionAttribute), false);
                    if (checkPermissionAttributes.Length > 0)
                        foreach (var attribute in checkPermissionAttributes)
                        {
                            var permissionName = ((CheckPermissionAttribute)attribute).permissionRouteName;
                            if (permissionName is not null)
                            {

                                var PermissionCategory = permissionName.Split('.')[0].ToUpperInvariant();
                                var Name = permissionName.Split('.')[1].Split('_');
                                string NameJoined = string.Join(" ", Name).ToUpperInvariant();

                                NameJoined = $"{NameJoined} {PermissionCategory}";
                                permissions.Add(new Permission() { Name = NameJoined, Category = PermissionCategory, RouteName = permissionName });
                            }
                        }
                }
            }
        }
    }

}

