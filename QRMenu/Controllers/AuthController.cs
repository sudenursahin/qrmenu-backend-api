using Business.GenericRepository.BaseRep;
using CoreL.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QRMenu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminAuthenticationService _authenticationService;

        public AuthController(IAdminAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserAuthentication userAuthentication)
        {
            var token = await _authenticationService.Login(userAuthentication);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("UserName or Password is incorrect");
            }

            return Ok(new
            {
                Token = token
            });
        }

    }
}
