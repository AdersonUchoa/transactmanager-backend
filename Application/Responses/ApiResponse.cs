using System.Net;

namespace Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }

        public ApiResponse(bool success, HttpStatusCode statusCode, object? data, string message, string? details)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Data = data;
            Details = details;
        }
    }
}
