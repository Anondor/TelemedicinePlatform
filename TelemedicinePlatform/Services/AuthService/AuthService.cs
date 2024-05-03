using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TelemedicinePlatform.Core;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly APIDbContext _context;

        public AuthService(IConfiguration config, APIDbContext context)
        {
            configuration = config;
            _context = context;
        }

        public async Task<object> GetJWTToken(LoginModel model)
        {
            try
            {
                var doctorUser = await _context.Doctors.FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Password == model.Password);
                if (doctorUser == null)
                    throw new Exception("Invalid HHT User");

                var user = new AppUserPrinciple("brainstation23")
                {
                    DoctorId = doctorUser.DoctorId,
                    Name = doctorUser.Name,
                    Gender = doctorUser.Gender,
                    RoleIdList = new List<int> { 0 },
                    Avatar = "/img/user.png",
                    DateOfBirth = doctorUser.DateOfBirth,
                    Phone = doctorUser.Phone,
                    Address= doctorUser.Address,
                    UserName= doctorUser.UserName,
                    Email = doctorUser.Email,
                    UserAgentInfo = "127.0.0.1",

                };
                var appClaimes = user
                                .GetByName()
                                .Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                    {

                            new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Sub,user.DoctorId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                claims.AddRange(appClaimes);
                foreach (var role in user.RoleIdList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    configuration["Tokens:Issuer"],
                    configuration["Tokens:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(5.00),
                    signingCredentials: cred
                    );
                var results = new
                {
                    DoctorId = doctorUser.DoctorId,
                    Name = doctorUser.Name,
                    Gender = doctorUser.Gender ?? "",
                    Phone = doctorUser.Phone,
                    Address = doctorUser.Address,
                    UserName = doctorUser.UserName,
                    Email = doctorUser.Email,

                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
       
                };

                return results;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Task<object> GetJWTToken(AdLoginModel model)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetJWTTokenForNonSSO(LoginModel model)
        {
            throw new NotImplementedException();
        }
    }
}
