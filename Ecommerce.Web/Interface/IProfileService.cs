using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Web.Interface
{
    public interface IProfileService
    {
        Task<Result<ProfileModel>> GetProfileAsync();
        
    }
}
