using System.Net;
using System.Numerics;

namespace TelemedicinePlatform.Models
{
    public class DoctorModel
    {
        public int DoctorId { get; set; }
        public int Name { get; set; }

        public int Phone { get; set; }
        public int Email { get; set; }
        public int Address { get; set; }
        public int ProfilePicture { get; set; }
        public int PatientId { get; set; }
        public int ProofOfPayment { get; set; }
        public int Amount { get; set; }

    }
}
