using TelemedicinePlatform.Models;
using TelemedicinePlatform.Services.Doctor.Interfaces;

namespace TelemedicinePlatform.Services.Doctor.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Domain.Doctor.DoctorDetails> _doctorDetails
        public async Task<IEnumerable<DoctorModel>> GetAllForSelectAsync()
        {
            var result= await (from a in )
        }
    }
}
