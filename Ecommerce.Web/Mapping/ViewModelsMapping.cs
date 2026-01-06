using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.ViewModels.Profile;

namespace Ecommerce.Web.Mapping
{
    public class ViewModelsMapping : Profile
    {
        public ViewModelsMapping() {
            CreateMap<ProfileModel, ProfileViewModel>();
        }
    }
}
