using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models{
    public class LoginViewModel{

        [EmailAddress(ErrorMessage = "Invalid email address")]  
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]  
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

