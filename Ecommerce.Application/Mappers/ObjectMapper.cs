using AutoMapper;
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
            CreateMap<OrderItem, OrderItemModel >().ReverseMap();
            CreateMap<Cart,CartModel>().ReverseMap();
        }
    }
}
