using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreL.ServiceClasses
{
    public interface IHashingService
    {

        string HashPassword(string password);
        bool VerifyPassword(string inputPassword, string hashedPassword);
    }
}
