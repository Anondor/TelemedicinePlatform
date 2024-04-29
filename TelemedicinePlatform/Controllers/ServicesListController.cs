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
    public class ServicesListController : ControllerBase
    {
        private readonly APIDbContext _context;
        public ServicesListController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(ServicesList model)
        {
            var response = new ApiResponse();

            try
            {
                await _context.ServicesLists.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Services data save Successfully";
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
        public async Task<ActionResult<ApiResponse>> GetServices()
        {
            var response =new ApiResponse();
            try
            {
                var serviceQuery = _context.ServicesLists.AsQueryable();

                var service = await serviceQuery.ToListAsync();
                response.Result = service;
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
        public async Task<ActionResult<ApiResponse>> GetService(int id)
        {
            var response = new ApiResponse();
            try
            {
                var service = await _context.ServicesLists.FirstOrDefaultAsync(x => x.ServiceId == id);
                if (service == null)
                {
                    response.Message = "Service not found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = service;
                response.Message = "Service Data  found";
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
        public async Task<ActionResult<ApiResponse>> PutService(ServicesList model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.ServicesLists.FirstOrDefaultAsync(x => x.ServiceId == model.ServiceId);
                if (dbModel == null)
                {
                    response.Message = "Services data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.PaymentId = model.PaymentId;
                dbModel.AppointmentId = model.AppointmentId;
                dbModel.PatientId = model.PatientId;
                dbModel.ProofOfPayment = model.ProofOfPayment;
                dbModel.Remarks = model.Remarks;
                dbModel.processBy = model.processBy;
                dbModel.Status = model.Status;
                dbModel.Amount = model.Amount;
                dbModel.DoctorId = model.DoctorId;
                _context.ServicesLists.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Services Data Updated";
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
        public async Task<ActionResult<ApiResponse>> DeleteServices(int id)
        {
            var response = new ApiResponse();
            if (_context.ServicesLists == null)
            {
                response.Message = "No Item Available";
                response.IsError = true;
                return response;
            }
            try
            {
                var services = await _context.ServicesLists.FindAsync(id);
                if (services == null)
                {
                    response.Message = "Services data is not found";
                    response.IsError = true;
                    return response;
                }
                _context.ServicesLists.Remove(services);
                response.Message = "Services Data Remove";
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
