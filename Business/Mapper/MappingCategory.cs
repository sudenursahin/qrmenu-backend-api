using AutoMapper;
using Business.DTOs.Category;
using Business.DTOs.MenuItem;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Mapper
{
    public class MappingCategory : Profile
    {
        public MappingCategory()
        {
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryGetDto>().ReverseMap();
        }
    }
}
