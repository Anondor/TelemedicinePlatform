using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TelemedicinePlatform.Filters;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorize]
    public class PaymentController : ControllerBase
    {
        private readonly APIDbContext _context;
        public PaymentController(APIDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Payment model)
        {
            var response = new ApiResponse();

            try
            {
                await _context.Payments.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Payment's data save Successfully";
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
        public async Task<ActionResult<ApiResponse>> GetPayment()
        {
            var response = new ApiResponse();
            try
            {
                var paymentQuery = _context.Payments.AsQueryable();

                var payment = await paymentQuery.ToListAsync();
                response.Result = payment;
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
        public async Task<ActionResult<ApiResponse>> GetPayment(int id)
        {
            var response = new ApiResponse();
            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(x => x.PaymentId == id);
                if (payment == null)
                {
                    response.Message = "Payment not found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = payment;
                response.Message = "Payment Data  found";
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
        public async Task<ActionResult<ApiResponse>> PutPayment(Payment model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Payments.FirstOrDefaultAsync(x => x.PaymentId == model.PaymentId);
                if (dbModel == null)
                {
                    response.Message = "Appointment data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.AppointmentId = model.AppointmentId;
                dbModel.PatientId = model.PatientId;
                dbModel.ProofOfPayment = model.ProofOfPayment;
                dbModel.Remarks = model.Remarks;
                dbModel.processBy = model.processBy;
                dbModel.Status = model.Status;
                dbModel.Amount = model.Amount;
                dbModel.ServiceId = model.ServiceId;
                dbModel.DoctorId = model.DoctorId;
                _context.Payments.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Payment Data Updated";
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
        public async Task<ActionResult<ApiResponse>> DeletePayment(int id)
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
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                {
                    response.Message = "Payment data is not found";
                    response.IsError = true;
                    return response;
                }
                _context.Payments.Remove(payment);
                response.Message = "Payment Data Remove";
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
