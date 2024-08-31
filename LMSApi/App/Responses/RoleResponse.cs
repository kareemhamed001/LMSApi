using LMSApi.App.Attributes.Vlidation;
using System.ComponentModel.DataAnnotations;

namespace LMSApi.App.Responses
{
    public class RoleResponse
    {
        public string Name { get; set; }
        public List<PermissionResponse> Permissions { get; set; }
    }
}
