using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AntonS.Models
{
    public class LoginPageModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
