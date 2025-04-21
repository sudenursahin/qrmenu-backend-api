using CoreL.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  
        public class User :  BaseEntity
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
    
}
