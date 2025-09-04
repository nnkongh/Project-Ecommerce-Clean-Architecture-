namespace Ecommerce.Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The value cannot be null.");
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
        public string Code { get; }
        public string Message { get; }

        public bool Equals(Error? other)
        {
            if(other is null)
            {
                return false;
            }
            return Code == other.Code && Message == other.Message;
        }
    }
    
}
