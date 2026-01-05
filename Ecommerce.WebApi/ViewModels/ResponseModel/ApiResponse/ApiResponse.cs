namespace Ecommerce.Web.ViewModels.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public ApiError? Error { get; set; }
        public string? Message {  get; set; }

        public static ApiResponse<T> Success(T value)
        {
            return new ApiResponse<T> { Value = value, IsSuccess = true };
        }
        public static ApiResponse<T> Fail(string message,ApiError? error) => new ApiResponse<T> { IsSuccess = false, Error = error }; 
        public static ApiResponse<T> Fail(string message) => new ApiResponse<T> { IsSuccess = false}; 
    }
}
