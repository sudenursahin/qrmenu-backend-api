using AutoMapper;
using Business.DTOs.OrderItem;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingOrderItem:Profile
    {
        public MappingOrderItem()
        {
            CreateMap<OrderItem,OrderItemGetDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemCreateDto>().ReverseMap();
            CreateMap<OrderItem,OrderItemUpdateDto>().ReverseMap();
        }
    }
}
