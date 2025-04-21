using Business.DTOs.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Role
{
    public class RoleGetDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserRoleGetDto> UserRoles { get; set; }
    }
}
