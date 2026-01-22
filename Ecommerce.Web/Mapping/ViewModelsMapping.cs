using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Product;
using Ecommerce.Application.DTOs.ModelsRequest.User;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.Profile;

namespace Ecommerce.Web.Mapping
{
    public class ViewModelsMapping : Profile
    {
        public ViewModelsMapping() {
            CreateMap<ProfileModel, ProfileViewModel>();
            CreateMap<ProfileModel, UpdateProfileRequest>();
            CreateMap<AddressModel, AddressRequest>();


            CreateMap<CategoryModel, CategoryViewModel>();
            
            CreateMap<ProductModel, ProductViewModel>();
            CreateMap<ProductViewModel,UpdateProductRequest>();


            CreateMap<CartModel, CartViewModel>();
            CreateMap<CartItemModel, CartItemViewModel>();

            CreateMap<OrderModel, OrderViewModel>();
            CreateMap<OrderItemModel, OrderItemViewModel >();


        }
    }
}
