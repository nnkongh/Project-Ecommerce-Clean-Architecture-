using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Category;
using Ecommerce.Application.DTOs.ModelsRequest.Product;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Mappers
{
    public class ObjectMapper : Profile
    {
       public ObjectMapper()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<OrderItem, OrderItemModel >().ReverseMap();
            CreateMap<Cart,CartModel>().ReverseMap();
            CreateMap<User,UserModel>().ReverseMap();
            CreateMap<CartItem,CartItemModel>().ReverseMap();
            CreateMap<ItemWishList,ItemWishlistModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();
            CreateMap<Wishlist, WishlistModel>().ReverseMap();

            // Mapping for specific models
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();

            CreateMap<ProfileModel, User>();
            CreateMap<User, ProfileModel>();


        }
    }
}
