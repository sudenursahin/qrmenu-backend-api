using CoreL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }
}
