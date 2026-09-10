namespace Ecommerce.Domain.Enum
{
    public enum PaymentMethod
    {
        Cod,
        VnPay,
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded,
    }
}