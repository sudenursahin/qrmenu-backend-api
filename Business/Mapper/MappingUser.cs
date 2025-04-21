using AutoMapper;
using Business.DTOs.Admin;
using Business.DTOs.Category;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingAdmin : Profile
    {
        public MappingAdmin()
        {
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserGetDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}
