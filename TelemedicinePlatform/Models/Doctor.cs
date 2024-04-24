using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TelemedicinePlatform.Models
{
    [Table("Doctor")]
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
