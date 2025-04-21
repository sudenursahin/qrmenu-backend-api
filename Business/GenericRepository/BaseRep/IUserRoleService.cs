using Business.DTOs.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleGetDto>> GetList();

        Task<UserRoleGetDto> GetItem(int id);

        Task<UserRoleCreateDto> PostItem(UserRoleCreateDto userRoleCreateDto);

        Task<bool> PutItem(int id, UserRoleUpdateDto userRoleUpdateDto);

        Task<bool> DeleteItem(int id);
    }
}
