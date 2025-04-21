using CoreL.ServiceExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IAdminAuthenticationService
    {
        Task<string> Login(UserAuthentication userAuthentication);
        Task Register(UserRegistration userRegistration);
    }
}
