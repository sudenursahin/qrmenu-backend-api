using Business.DTOs.MenuItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemGetDto>> GetAllMenuItemsAsync();
        Task<MenuItemGetDto> GetMenuItemByIdAsync(int id);
        Task<MenuItemCreateDto> CreateMenuItemAsync(MenuItemCreateDto menuItemCreateDto);
        Task<MenuItemUpdateDto> UpdateMenuItemAsync(int id, MenuItemUpdateDto menuItemUpdateDto);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<bool> MenuItemExistsAsync(int id);
        Task<IEnumerable<MenuItemGetDto>> GetMenuItemsByCategoryIdAsync(int categoryId); // Add this line
    }
}