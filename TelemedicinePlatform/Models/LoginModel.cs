using System.ComponentModel.DataAnnotations;

namespace TelemedicinePlatform.Models
{
    public class LoginModel
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }


   

        public string? UserMessage { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
