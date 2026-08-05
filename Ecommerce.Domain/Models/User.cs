using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class User
    {
        public string Id { get; private set; } = default!;
        public bool EmailConfirmed { get; private set; }
        public string? UserName { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string Email { get; private set; } = default!;
        public bool IsActive { get; private set; }
        public List<string?> Role { get; private set; } = [];
        public DateTime CreateAt { get; private set; }
        public Address? Address { get; private set; }
        public Cart? Cart { get; private set; }
        public IReadOnlyList<Order> Orders => _orders.AsReadOnly();
        public IReadOnlyList<Wishlist> Wishlist => _wishlist.AsReadOnly();

        private readonly List<Order> _orders = new List<Order>();
        private readonly List<Wishlist> _wishlist = new List<Wishlist>();

        public static User Create(string userName, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new DomainException("Tên người dùng không được trống");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email người dùng không được trống");
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new DomainException("Số điện thoại người dùng không được trống");

            return new User
            {
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                CreateAt = DateTime.Now,
                Role = {"User"},
                IsActive = true
            };
        }
        public void UpdateAddress(Address address) {
            if (address != null) Address = address;
        }
        public void UpdateAvatarUrl(string avatarUrl)
        {
            if (avatarUrl != null) ImageUrl = avatarUrl;
        }
        public void UpdatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber != null) PhoneNumber = phoneNumber;
        }
        public void MarkAsEmailConfirmed() => EmailConfirmed = true;
        public void DeactivateAccount() => IsActive = false;
        public void ReactiveAccount() => IsActive = true;
    }
}
