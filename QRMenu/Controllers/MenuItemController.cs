using Microsoft.AspNetCore.Mvc;
using Business.DTOs.MenuItem;
using Business.GenericRepository.ConcManager;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.GenericRepository.BaseRep;

namespace QRMenu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(IMenuItemService menuItemService, ILogger<MenuItemController> logger)
        {
            _menuItemService = menuItemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemGetDto>>> GetAllMenuItems()
        {
            try
            {
                var menuItems = await _menuItemService.GetAllMenuItemsAsync();
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all menu items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuItemGetDto>> GetMenuItem(int id)
        {
            try
            {
                var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found.");
                }
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting menu item {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MenuItemCreateDto>> CreateMenuItem([FromForm] MenuItemCreateDto menuItemCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdMenuItem = await _menuItemService.CreateMenuItemAsync(menuItemCreateDto);
                return CreatedAtAction(
                    nameof(GetMenuItem),
                    new { id = createdMenuItem.CategoryId },
                    createdMenuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDto menuItemUpdateDto)
        {
            try
            {
                if (id != menuItemUpdateDto.Id)
                {
                    return BadRequest("ID in the URL does not match the ID in the request body.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedMenuItem = await _menuItemService.UpdateMenuItemAsync(id, menuItemUpdateDto);
                if (updatedMenuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found.");
                }
                return Ok(updatedMenuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating menu item {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                var result = await _menuItemService.DeleteMenuItemAsync(id);
                if (!result)
                {
                    return NotFound($"Menu item with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting menu item {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<MenuItemGetDto>>> GetMenuItemsByCategoryId(int categoryId)
        {
            try
            {
                var menuItems = await _menuItemService.GetMenuItemsByCategoryIdAsync(categoryId);
                if (!menuItems.Any())
                {
                    return NotFound($"No menu items found for category with ID {categoryId}.");
                }
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting menu items for category {categoryId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}