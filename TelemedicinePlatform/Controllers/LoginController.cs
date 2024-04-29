using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using TelemedicinePlatform.Models;
using TelemedicinePlatform.Services.AuthService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly APIDbContext _context;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;



        public LoginController(APIDbContext context, IConfiguration config,
            IAuthService authService
            )
        {
            _context = context;
            _config = config;
           _authService = authService;
       
        }

        [HttpPost("DoctorLogin")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            /* var response= new ApiResponse();
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
            */
            var apiResult = new ApiResponse<IEnumerable<LoginModel>>
            {
                Data = new List<LoginModel>()
            };

            if (ModelState.IsValid)
            {
                try
                {
                    var doctorUser = await _context.Doctors.FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Password == model.Password);
                    if (doctorUser != null)
                    {
                        var result = await _authService.GetJWTToken(model);
                        return OkResult(result);
                    }
                    else
                    {
                        throw new Exception("Invalid username or password.");
                    }


                }
                catch (Exception ex)
                {
                   // ex.ToWriteLog();

                    apiResult.StatusCode = 500;
                    apiResult.Status = "Fail";
                    apiResult.Msg = ex.Message;
                    return BadRequest(apiResult);
                }

            }
            return BadRequest();

        }

        private IActionResult OkResult(object data)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = 200,
                Message = "Successful",
                Result = data 
            
        };
            return ObjectResult(apiResult);
        }
        protected IActionResult ObjectResult(ApiResponse model)
        {
            var result = new ObjectResult(model)
            {
                StatusCode = model.StatusCode
            };
            return result;
        }

        [HttpPost("PatientLogin")]
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
