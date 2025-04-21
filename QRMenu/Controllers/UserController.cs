using Business.DTOs.Admin;
using Business.GenericRepository.BaseRep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Business.ServiceSettings;
using Microsoft.Extensions.Options;
using Domain;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using CoreL.ServiceClasses; // For validation
using CoreL.ServiceManager;

namespace QRMenu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _adminService;

        public UserController(IUserService adminService)
        {
            _adminService = adminService;
        }


        [HttpPost]
        public async Task<ActionResult<UserCreateDto>> PostItem(UserCreateDto adminCreateDto)
        {
            var createdUser = await _adminService.PostItem(adminCreateDto);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetDto>>> GetItems()
        {
            var admins = await _adminService.GetList();
            return Ok(admins); // Admin listesini döner
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserGetDto>> GetByEmail(string email)
        {
            var admin = await _adminService.GetByEmail(email);
            if (admin == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döner
            }
            return Ok(admin); // Kullanıcıyı döner
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<bool>> PutItem(string email, UserUpdateDto adminUpdateDto)
        {
            // Mevcut admin bilgilerini email'e göre getir
            var existingAdmin = await _adminService.GetByEmail(email);
            if (existingAdmin == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döner
            }

            var result = await _adminService.PutItem(existingAdmin.Id, adminUpdateDto);
            if (!result)
            {
                return BadRequest("Güncelleme işlemi başarısız oldu.");
            }
            return Ok(true); // Güncelleme başarılıysa true döner
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutItem(int id, UserUpdateDto adminUpdateDto)
        {
            if (id != adminUpdateDto.Id)
            {
                return BadRequest();
            }

            var result = await _adminService.PutItem(id, adminUpdateDto);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteItem(int id)
        {
            var result = await _adminService.DeleteItem(id);
            if (!result)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döner
            }
            return Ok(result); // Silme başarılıysa true döner
        }
    }
}