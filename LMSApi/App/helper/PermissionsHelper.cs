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
            new Permission {Name = "List All Teacher", Category = "Class", RouteName = "Class.index" },
            new Permission {Name = "All Classes", Category = "Class", RouteName = "Class.AllClass" },
            new Permission {Name = "Create Class", Category = "Class", RouteName = "Class.createClass" },
            new Permission {Name = "Update Class", Category = "Class", RouteName = "class.updateClass" },
            new Permission {Name = "Delete Class", Category = "Class", RouteName = "class.deleteClass" },
                new Permission {Name = "getStudentsByClassId", Category = "Class", RouteName = "class.getStudentsByClassId" },
            };
        }
    }
}
