using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Services.Doctor.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorModel>> GetAllForSelectAsync();
    }
}
