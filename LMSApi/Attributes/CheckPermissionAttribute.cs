namespace LMSApi.App.Atrributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttribute(string permissionRouteName) : Attribute
    {
        public string permissionRouteName { get; set; } = permissionRouteName;
    }
}
