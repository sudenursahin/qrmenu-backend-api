using Business.DTOs.Admin;
using Business.DTOs.Category;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IUserService
    {
        Task<UserGetDto> GetByEmail(string email);
        Task<UserGetDto> GetItem(int id);

        Task<User> PostItem(UserCreateDto adminCreateDto);

        Task<bool> PutItem(int id ,UserUpdateDto adminUpdateDto);

        Task<bool> DeleteItem(int id);

        Task<IEnumerable<UserGetDto>> GetList();
    }
}
