using Business.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleGetDto>> GetList();

        Task<RoleGetDto> GetItem(int id);

        Task<RoleCreateDto> PostItem(RoleCreateDto roleCreateDto);

        Task<bool> PutItem(int id, RoleUpdateDto roleUpdateDto);

        Task<bool> DeleteItem(int id);
    }
}
