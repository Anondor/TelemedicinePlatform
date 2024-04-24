using System.ComponentModel.DataAnnotations;

namespace TelemedicinePlatform.Models
{
    public class LoginModel
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
