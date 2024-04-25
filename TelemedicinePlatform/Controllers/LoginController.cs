using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly APIDbContext _context;
        IConfiguration _configuration;

        public LoginController(APIDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Doctor")]
        public async Task<ActionResult<ApiResponse>> Login(LoginModel model)
        {
            var doctorUser = await _context.Doctors.FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Password == model.Password);
            if(doctorUser == null) { return BadRequest(); }
            else
            {
                model.UserMessage= "Login Success";
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                model.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(model);


            }


        }

        [HttpPost("Patient")]
        public async Task<ActionResult<ApiResponse>> LoginPatien(LoginModel model)
        {
            var response = new ApiResponse();
            try
            {
                var patientUser = await _context.Patients.FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Password == model.Password);
                if (patientUser != null)
                {
                    response.Result = patientUser;
                    response.Message = "Patient Login  Successfully";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    return response;
                }
                else
                {
                    response.Message = "Phone number or password is wrong";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return response;

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ResponseException = ex.Message;
                response.IsError = true;
                return response;
            }
        }
    }
}
