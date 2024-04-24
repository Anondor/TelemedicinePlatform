using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelemedicinePlatform.Models;
using System.Net;


namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly APIDbContext _context;

        public DoctorController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Doctor model)
        {
            var response =new ApiResponse();
            
            try
            {
                await _context.Doctors.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Doctor's data  save Successfully";
                return response;
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
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetDoctors()
        {
            var response = new ApiResponse();
            try
            {
                var doctorQuery = _context.Doctors.AsQueryable();

                var doctor = await doctorQuery.ToListAsync();
                response.Result = doctor;
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;

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
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>>GetDoctor(int id)
        {
            var response= new ApiResponse();
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.DoctorId == id);
                if(doctor == null)
                {
                    response.Message = "Doctor not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = doctor;
                response.Message = "Doctor Data  found";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
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
