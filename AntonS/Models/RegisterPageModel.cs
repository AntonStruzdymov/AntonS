using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AntonS.Models
{
    public class RegisterPageModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        [Remote("IsEmailAlreadyUsed",
        "Login",
        ErrorMessage = "This email already used")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare (nameof(Password))]
        public string PasswordConfirmation { get; set; }
    }
}
