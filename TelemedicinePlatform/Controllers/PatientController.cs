using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly APIDbContext _context;

        public PatientController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Patient model)
        {
            var response = new ApiResponse();

            try
            {
                await _context.Patients.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Patient's data  save Successfully";
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
        public async Task<ActionResult<ApiResponse>> GetPatients()
        {
            var response = new ApiResponse();
            try
            {
                var patientQuery = _context.Patients.AsQueryable();

                var doctor = await patientQuery.ToListAsync();
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
        public async Task<ActionResult<ApiResponse>> GetPatient(int id)
        {
            var response = new ApiResponse();
            try
            {
                var doctor = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == id);
                if (doctor == null)
                {
                    response.Message = "Patient not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = doctor;
                response.Message = "Patient Data  found";
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

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> PutPatient(Patient model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == model.PatientId);
                if (dbModel == null)
                {
                    response.Message = "Patient data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.Name = model.Name;
                dbModel.Gender = model.Gender;
                dbModel.DateOfBirth = model.DateOfBirth;
                dbModel.Phone = model.Phone;
                dbModel.Email = model.Email;
                dbModel.Address = model.Address;
                dbModel.ProfilePicture = model.ProfilePicture;
                dbModel.UserName = model.UserName;
                dbModel.Password = model.Password;
                dbModel.AccountStatus = model.AccountStatus;

                _context.Patients.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Doctor Data Updated";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsError = true;
                return response;

            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeletePatient(int id)
        {
            var response = new ApiResponse();
            if (_context.Patients == null)
            {
                response.Message = "No Item Available";
                response.IsError = true;
                return response;
            }
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    response.Message = "Patient data is not found";
                    response.IsError = true;
                    return response;
                }
                _context.Patients.Remove(patient);
                response.Message = "Patient Data Remove";
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsError = true;
                return response;
            }

        }

    }
}
