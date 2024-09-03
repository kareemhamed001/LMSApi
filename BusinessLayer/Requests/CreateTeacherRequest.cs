using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests
{
    public class CreateTeacherRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string NickName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? CommunicationEmail { get; set; }
        [Required]
        [Phone]
        [MaxLength(20)]
        public string? CommunicationPhone { get; set; }
    }
}
