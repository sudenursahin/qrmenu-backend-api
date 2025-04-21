using AutoMapper;
using Business.GenericRepository.BaseRep;
using CoreL.ServiceClasses;
using CoreL.ServiceExtension;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class UserAuthenticationManager : IAdminAuthenticationService
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHashingService _hashingService;
        private readonly IUserService _adminService;

        public UserAuthenticationManager(IConfiguration configuration, IMapper mapper,
          IHashingService hashingService, IUserService adminService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _hashingService = hashingService;
            _adminService = adminService;

        }

        public async Task<string> Login(UserAuthentication userAuthentication)
        {
            var user = await _adminService.GetByEmail(userAuthentication.Email);
            if (user == null || !_hashingService.VerifyPassword(userAuthentication.Password, user.Password))
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // Claim'leri genişletiyoruz
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, "Admin")  // Admin rolünü ekliyoruz
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30000),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task Register(UserRegistration userRegistration)
        {

        }
    }
}

        
    