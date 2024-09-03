using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
