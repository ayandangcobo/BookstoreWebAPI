using System.ComponentModel.DataAnnotations;

namespace BookStoreDomain.Models
{
    public class UserLoginViewModel
    {

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Username/Email is required")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Username: Please provide a valid email address")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class UserRegistrationViewModel : UserLoginViewModel
    {
        [Required(ErrorMessage = "Author Pseudonym is required")]
        public string AuthorPseudonym { get; set; }
        [Compare("Password", ErrorMessage = "Password & Confirmation Password do not match.")]
        [Required(ErrorMessage = "Password Confirmation is required")]
        public string ConfirmPassword { get; set; }
    }
}
