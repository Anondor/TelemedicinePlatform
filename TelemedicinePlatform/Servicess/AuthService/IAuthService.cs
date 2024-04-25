using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Servicess.AuthService
{
    public interface IAuthService
    {
        Task<object> GetJWTToken(LoginModel model);
        Task<object> GetJWTToken(AdLoginModel model);
        Task<object> GetJWTTokenForNonSSO(LoginModel model);
    }
}
