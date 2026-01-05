using Ecommerce.Domain.Shared;

namespace Ecommerce.Web.ViewModels.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string Message { get; set; }
        public Error Error { get; set; }

        public ApiResponse() { }
        public static ApiResponse<T> Success(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Value = data,
                Message = message
            }; 
        }
        public static ApiResponse<T> Fail(string message, Error? error)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Error = error
            };
        }
        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
            };
        }
    }
}
