namespace Ecommerce.Domain.Models
{
    public class UserAddress
    {
        public int Id { get; private set; }
        public string UserId { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string Ward { get; private set; } = default!;
        public string District { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string? Province { get; private set; }
        public bool IsDefault { get; private set; }

        public static UserAddress Create(string userId, string street, string ward, string district, string city, string? province, bool isDefault)
        {
            return new UserAddress
            {
                UserId = userId,
                Street = street,
                Ward = ward,
                District = district,
                City = city,
                Province = province,
                IsDefault = isDefault
            };
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void UnsetAsDefault()
        {
            IsDefault = false;
        }

        public void Update(string street, string ward, string district, string city, string? province)
        {
            Street = street;
            Ward = ward;
            District = district;
            City = city;
            Province = province;
        }
    }
}
