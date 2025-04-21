using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class UserRoleManager : BaseRepository<UserRole>
    {
        public UserRoleManager(QRMenuDbContext context) : base(context) 
        {
            
        }
    }
}
