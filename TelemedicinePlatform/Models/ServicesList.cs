using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelemedicinePlatform.Models
{
    [Table("ServicesList")]
    public class ServicesList
    {
        [Key]
        public int ServiceId { get; set; }
        public int PaymentId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string ProofOfPayment { get; set; }
        public string Remarks { get; set; }
        public string processBy { get; set; }
        public int Status { get; set; }
        public int Amount { get; set; }
        public int DoctorId { get; set; }
    }
}
