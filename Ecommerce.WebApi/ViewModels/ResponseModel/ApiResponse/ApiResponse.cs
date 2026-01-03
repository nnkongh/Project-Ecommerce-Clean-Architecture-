namespace Ecommerce.Web.ViewModels.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public ApiError Error { get; set; }
    }
}
