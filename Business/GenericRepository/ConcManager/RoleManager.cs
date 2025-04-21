using AutoMapper;
using Business.DTOs.Role;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class RoleManager : IRoleService
    {
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleManager(RoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }



        public async Task<IEnumerable<RoleGetDto>> GetList()
        {
            var roles = await _roleRepository.GetItems();
            var roleDtos = _mapper.Map<List<RoleGetDto>>(roles);
            return roleDtos;
        }

        public async Task<RoleGetDto> GetItem(int id)
        {
            var role = await _roleRepository.GetItem(id);

            if (role == null)
            {
                return null;
            }

            var roleDto = _mapper.Map<RoleGetDto>(role);

            return roleDto;
        }

        public async Task<RoleCreateDto> PostItem(RoleCreateDto roleCreateDto)
        {
            var role = _mapper.Map<Role>(roleCreateDto);

            await _roleRepository.AddAsync(role);

            var resultDto = _mapper.Map<RoleCreateDto>(role);
            return resultDto;
        }

        public async Task<bool> PutItem(int id, RoleUpdateDto roleUpdateDto)
        {
            var existingRole = await _roleRepository.GetItem(id);

            if (existingRole == null)
            {
                return false;
            }

            _mapper.Map(roleUpdateDto, existingRole);

            await _roleRepository.UpdateAsync(existingRole);

            return true;
        }

        public async Task<bool> DeleteItem(int id)
        {
           
            var userRole = await GetItem(id);
            if (userRole == null)
            {
                return false; // UserRole bulunamadı, silme işlemi başarısız
            }

            var result = await _roleRepository.DeleteAsync(id);

            return true; // Silme işlemi başarılı

        }

    }
}
