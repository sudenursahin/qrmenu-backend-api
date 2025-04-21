using Business.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Admin
{
    public class UserGetDto


    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public ICollection<RoleGetDto> Roles { get; set; }

    }

}
