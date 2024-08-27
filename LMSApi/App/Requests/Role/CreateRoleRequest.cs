using System.ComponentModel.DataAnnotations;
using LMSApi.App.Attributes.Vlidation;
namespace LMSApi.App.Requests.Role
{
    public class CreateRoleRequest
    {
       
        public string RoleName { get; set; }

        [Required]
        [ValidateList(1)]
        public List<int> Permissions { get; set; }
    }
}
