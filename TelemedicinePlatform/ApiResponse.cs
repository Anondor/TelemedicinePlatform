using System.Net;
namespace TelemedicinePlatform
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public ApiResponse()
        {
            Success = true;
        }
    }
}
