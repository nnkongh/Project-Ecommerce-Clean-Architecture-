namespace Ecommerce.Web.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.")
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }
}