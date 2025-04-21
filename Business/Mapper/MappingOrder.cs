using AutoMapper;
using Business.DTOs.MenuItem;
using Business.DTOs.Order;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingOrder : Profile
    {
        public MappingOrder()
        {
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, OrderGetDto>().ReverseMap();
            CreateMap<Order,OrderStatusDto>().ReverseMap();
        }
       
    }
}
