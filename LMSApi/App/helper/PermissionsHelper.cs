namespace LMSApi.App.helper
{
    public class PermissionsHelper
    {
        public static List<Permission> GetPermissions()
        {
            return new List<Permission>() {
            new Permission {Name = "List All Teacher", Category = "Teacher", RouteName = "teacher.index" },
            new Permission {Name = "Store Teacher", Category = "Teacher", RouteName = "teacher.store" },
            new Permission {Name = "Show Teacher", Category = "Teacher", RouteName = "teacher.show" },
            new Permission {Name = "Update Teacher", Category = "Teacher", RouteName = "teacher.update" },
            new Permission {Name = "Delete Teacher", Category = "Teacher", RouteName = "teacher.delete" },
            };
        }
    }
}
