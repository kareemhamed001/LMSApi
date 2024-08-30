using System.ComponentModel.DataAnnotations;

namespace LMSApi.App.Requests.Subject
{
    public class UpdateSubjectRequest
    {
        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
    }
}
