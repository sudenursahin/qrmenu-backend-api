using AutoMapper;
using Business.DTOs.Admin;
using Business.DTOs.Category;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using CoreL.ServiceClasses;
using CoreL.ServiceExtension;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class UserManager : IUserService
    {

        private readonly IMapper _mapper;
        private readonly UserRepository _adminRepository;
        private readonly IHashingService _hashingService;

        public UserManager(IMapper mapper, UserRepository adminRepository, IHashingService hashingservice)
        {
            _hashingService = hashingservice;
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteItem(int id)
        {

            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
            {
                return false; // Kullanıcı bulunamadı
            }

            await _adminRepository.DeleteAsync(id);
            return true; // Başarıyla silindi



        }




        public async Task<UserGetDto> GetByEmail(string email)
        {
            var user = await _adminRepository.GetItemsAsQueryable()

                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                Console.WriteLine($"No user found with email: {email}");
            }
            else
            {
                Console.WriteLine($"User found: {user.Email}");


            }

            return _mapper.Map<UserGetDto>(user);
        }

        public Task<UserGetDto> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserGetDto>> GetList()
        {
            var admins = _adminRepository.GetItemsAsQueryable(); // IQueryable<Admin> al
            var adminList = await admins.ToListAsync(); // Listeye dönüştür
            return _mapper.Map<IEnumerable<UserGetDto>>(adminList); // DTO'ya dönüştürerek döner
        }

        public async Task<User> PostItem(UserCreateDto adminCreateDto)
        {
            var admin = _mapper.Map<User>(adminCreateDto);

            admin.Password = _hashingService.HashPassword(admin.Password);

            await _adminRepository.AddAsync(admin);

            return _mapper.Map<User>(admin);
        }

        public async Task<bool> PutItem(int id, UserUpdateDto adminUpdateDto)
        {
            var existingAdmin = await _adminRepository.GetByIdAsync(id);
            if (existingAdmin == null)
            {
                return false;
            }

            _mapper.Map(adminUpdateDto, existingAdmin);

         
                existingAdmin.Password = _hashingService.HashPassword(existingAdmin.Password);


            await _adminRepository.UpdateAsync(existingAdmin);

            return true;
        }




    }

   
}
