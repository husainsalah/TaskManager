
using System.ComponentModel.DataAnnotations;


namespace TaskManager.Models{
    public class RegisterViewModel{
        [Required(ErrorMessage = "Email is required")]  
        [EmailAddress(ErrorMessage = "Invalid email address")]  
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]  
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; } 
    }
}