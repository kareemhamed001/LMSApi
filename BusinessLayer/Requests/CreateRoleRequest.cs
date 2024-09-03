using BusinessLayer.Attributes.Vlidation;
using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.Requests
{
    public class CreateRoleRequest
    {
       
        public string Name { get; set; }

        [Required]
        [ValidateList(1)]
        public List<int> Permissions { get; set; }
    }
}
