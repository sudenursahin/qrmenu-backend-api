using AutoMapper;
using Business.DTOs.Category;
using Business.DTOs.Order;
using Business.DTOs.Table;
using Business.DTOs.TableItem;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingTable : Profile
    {
        public MappingTable()
        {
            CreateMap<TableCreateDto, Table>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.TableItems, opt => opt.Ignore());

            CreateMap<Table, TableGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<TableUpdateDto, Table>();



        }
    }
}
