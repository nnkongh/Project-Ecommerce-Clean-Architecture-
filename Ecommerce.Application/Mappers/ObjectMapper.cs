using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.CRUD.Category;
using Ecommerce.Application.DTOs.CRUD.Product;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.DTOs.Product;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


            // Mapping for specific models
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
        }
    }
}
