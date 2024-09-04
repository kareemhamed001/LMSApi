using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests
{
    public class UpdateTeacherRequest
    {
 
        [MaxLength(50)]
        public string? FirstName { get; set; } = null!;
  
        [MaxLength(50)]
        public string? LastName { get; set; } = null!;
   
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; } = null!;

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; } = null!;

        [MinLength(8)]
        public string? Password { get; set; } = null!;

        [MaxLength(50)]
        public string? NickName { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? CommunicationEmail { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? CommunicationPhone { get; set; }

    }
}
