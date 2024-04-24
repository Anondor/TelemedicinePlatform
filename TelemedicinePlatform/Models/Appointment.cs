using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelemedicinePlatform.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int serviceId { get; set; }
        public int Amount { get; set; }
        public string MeetingLink { get; set; }
        public int Status { get; set; }
        public int PatientId { get; set; }
        
    }
}
