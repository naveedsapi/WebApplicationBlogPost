using System.ComponentModel.DataAnnotations;

namespace WebApplicationBlogPost.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email Is Requird Field")]
        [EmailAddress(ErrorMessage = "Email Must be In Correct Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Requird Field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "Password Must match The ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
