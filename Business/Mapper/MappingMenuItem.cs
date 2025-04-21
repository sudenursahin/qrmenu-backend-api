using AutoMapper;
using Business.DTOs.Category;
using Business.DTOs.MenuItem;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingMenuItem : Profile
    {
        public MappingMenuItem()
        {
            CreateMap<MenuItem, MenuItemGetDto>().ReverseMap();
            CreateMap<MenuItem, MenuItemUpdateDto>().ReverseMap();
            CreateMap<MenuItem, MenuItemCreateDto>().ReverseMap();
        }
    }
}
