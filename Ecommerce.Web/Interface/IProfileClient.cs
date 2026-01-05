using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Web.Interface
{
    public interface IProfileClient
    {
        Task<Result<ProfileModel>> GetProfileAsync();
        
    }
}
