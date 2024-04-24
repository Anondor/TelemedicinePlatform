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
    public class AppointmentController : ControllerBase
    {
        private readonly APIDbContext _context;
        public AppointmentController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Appointment model)
        {
            var response = new ApiResponse();

            try
            {
                await _context.Appointments.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Appiontment's data save Successfully";
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
        public async Task<ActionResult<ApiResponse>> GetAppointments()
        {
            var response = new ApiResponse();
            try
            {
                var appointmentQuery = _context.Appointments.AsQueryable();

                var appointment = await appointmentQuery.ToListAsync();
                response.Result = appointment;
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
        public async Task<ActionResult<ApiResponse>> GetAppointment(int id)
        {
            var response = new ApiResponse();
            try
            {
                var doctor = await _context.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == id);
                if (doctor == null)
                {
                    response.Message = "Appointment not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = doctor;
                response.Message = "Appointment Data  found";
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
        public async Task<ActionResult<ApiResponse>> PutAppointment(Appointment model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == model.AppointmentId);
                if (dbModel == null)
                {
                    response.Message = "Appointment data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.DoctorId = model.DoctorId;
                dbModel.Date = model.Date;
                dbModel.Time = model.Time;
                dbModel.serviceId = model.serviceId;
                dbModel.Amount = model.Amount;
                dbModel.MeetingLink = model.MeetingLink;
                dbModel.Status = model.Status;
                dbModel.PatientId = model.PatientId;
                _context.Appointments.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Appointment Data Updated";
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
        public async Task<ActionResult<ApiResponse>> DeleteAppointment(int id)
        {
            var response = new ApiResponse();
            if (_context.Appointments == null)
            {
                response.Message = "No Item Available";
                response.IsError = true;
                return response;
            }
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    response.Message = "Appointment data is not found";
                    response.IsError = true;
                    return response;
                }
                _context.Appointments.Remove(appointment);
                response.Message = "Appointment Data Remove";
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
