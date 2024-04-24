using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly APIDbContext _context;

        public LoginController(APIDbContext context)
        {
            _context = context;
        }

        [HttpPost("Doctor")]
        public async Task<ActionResult<ApiResponse>> Login(LoginModel model)
        {
            var response= new ApiResponse();
            try
            {
                var doctorUser=await _context.Doctors.FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Password==model.Password);
                if (doctorUser!=null)
                {
                    response.Result = doctorUser;
                    response.Message = "Doctor Login  Successfully";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    return response;
                }
                else
                {
                    response.Message = "Phone number or password is wrong";
                    response.StatusCode= (int)HttpStatusCode.BadRequest;
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
