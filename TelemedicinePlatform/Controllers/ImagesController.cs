using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Net;
using TelemedicinePlatform.Filters;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorize]
    public class ImagesController : ControllerBase
    {
   
            private readonly APIDbContext _dbContext;


            public ImagesController(APIDbContext dbContext)
            {
                _dbContext = dbContext;

            }

     
            
  

            [HttpPost("upload")]
            public async Task<ActionResult<ApiResponse>> Upload(IFormFile file)
            {
            var response = new ApiResponse();

        if (file == null || file.Length == 0)
            // return BadRequest("No file uploaded.");
            {
                response.Message = "Doctor's data  save Successfully";
                return response;
            }
              var imageEntity = new ImageEntity
                {
                    FileName = file.FileName
                };

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageEntity.Data = memoryStream.ToArray();
                }

                await _dbContext.Images.AddAsync(imageEntity);
                await _dbContext.SaveChangesAsync();

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "File uploaded  Successfully";
            return response;

            //return Ok(imageEntity.Id);
            }

        [HttpGet("download/{id}")]
        public async Task<ActionResult<ApiResponse>> Download(int id)
        {
            var response = new ApiResponse();
            var imageEntity = await _dbContext.Images.FirstOrDefaultAsync(image => image.Id == id);

            if (imageEntity == null)
            { 
                response.Message = "File not found";
            return response;
            }

                var fileContentResult = new FileContentResult(imageEntity.Data, "application/octet-stream")
                {
                    FileDownloadName = imageEntity.FileName
                };
            response.Result = fileContentResult;

                return response;
            }

        
    }
}
