using System.ComponentModel.DataAnnotations;
using LMSApi.App.Attributes.Vlidation;
namespace LMSApi.App.Requests
{
    public class CreateRoleRequest
    {
       
        public string Name { get; set; }

        [Required]
        [ValidateList(1)]
        public List<int> Permissions { get; set; }
    }
}
