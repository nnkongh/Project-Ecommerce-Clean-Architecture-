namespace Ecommerce.Application.DTOs.ModelsRequest.Address
{
    public sealed record CreateAddressRequest
    {
        public string Street { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Province { get; set; }
        public bool IsDefault { get; set; }
    }
}
