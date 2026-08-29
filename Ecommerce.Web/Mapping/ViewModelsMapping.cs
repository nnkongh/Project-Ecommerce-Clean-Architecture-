using AutoMapper;
using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Product;
using Ecommerce.Application.DTOs.ModelsRequest.Users;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.Models;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.Profile;

namespace Ecommerce.Web.Mapping
{
    public class ViewModelsMapping : Profile
    {
        public ViewModelsMapping() {
            CreateMap<ProfileModel, ProfileViewModel>();
            CreateMap<ProfileModel, UpdateProfileRequest>()
                .ForMember(d => d.AvatarUrl, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<AddressModel, AddressRequest>();


            CreateMap<CategoryModel, CategoryViewModel>();
            CreateMap<CategoryWithProductModel, CategoryViewModel>();
            
            CreateMap<ProductModel, ProductViewModel>();
            CreateMap<ProductViewModel, UpdateProductRequest>();
            CreateMap<ProductViewModel, CreateProductRequest>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(d => d.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(d => d.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(d => d.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId))
                .ForMember(d => d.ChildCategoryId, opt => opt.MapFrom(src => src.ChildCategoryId));


            CreateMap<CartModel, CartViewModel>();
            CreateMap<CartItemModel, CartItemViewModel>();

            CreateMap<OrderModel, OrderViewModel>();
            CreateMap<OrderItemModel, OrderItemViewModel >();

            CreateMap<UpdateProfileRequest, ProfileModel>();
            CreateMap<AddressRequest, AddressModel>();

            CreateMap<UserAddressModel, UserAddressViewModel>();
            CreateMap<WishlistModel, WishlistViewModel>();
            CreateMap<ItemWishlistModel, ItemWishlistViewModel>();

            CreateMap<CommentModel, CommentViewModel>();

            CreateMap<NotificationModel, NotificationViewModel>();

            CreateMap<PagedResult<ProductModel>, PagedResult<ProductViewModel>>();
            CreateMap<PagedResult<CategoryModel>, PagedResult<CategoryViewModel>>();

            CreateMap<ShopModel, ShopViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.ShopName))
                .ForMember(d => d.HasShop, opt => opt.MapFrom(_ => true));
        }
    }
}
