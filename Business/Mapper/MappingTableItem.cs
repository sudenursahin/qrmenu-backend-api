using AutoMapper;
using Business.DTOs.TableItem;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingTableItem : Profile
    {
        public MappingTableItem()
        {
            CreateMap<TableItem, TableItemCreateDto>().ReverseMap();
            CreateMap<TableItem, TableItemUpdateDto>().ReverseMap();
            CreateMap<TableItem, TableItemGetDto>().ReverseMap();
        }
    }
}
