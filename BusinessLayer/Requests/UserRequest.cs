using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests
{
    public class UserRequest
    {
        [Required(ErrorMessage = "First Name Is Required")]
        [MaxLength(50, ErrorMessage = "First Name Cannot Be More Than 50 Characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Is Required")]
        [MaxLength(50, ErrorMessage = "Last Name Cannot Be More Than 50 Characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone Is Required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(8, ErrorMessage = "Password Must Be At Least 6 Characters")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z0-9]*$", ErrorMessage = "Password Must Start With Capital Letter")]
        public string Password { get; set; }

        ////not greater than 100
        //[Range(0, 100, ErrorMessage = "Sallery Cannot Be More Than 100")]
        //public int Sallery { get; set; }

        ////accept only male and female
        //[AllowedValues(["Male", "Female"], ErrorMessage = "Allowed values are male and female")]
        //public string Gender { get; set; }

    }
}
