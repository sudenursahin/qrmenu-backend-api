using AutoMapper;
using Business.DTOs.MenuItem;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using CoreL.ServiceClasses;
using Domain;
using Microsoft.Extensions.Logging;

public class MenuItemManager : IMenuItemService
{
    private readonly MenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MenuItemManager> _logger;
    private readonly IFileService _fileService;

    public MenuItemManager(MenuItemRepository menuItemRepository, IMapper mapper,
        ILogger<MenuItemManager> logger, IFileService fileService)
    {
        _menuItemRepository = menuItemRepository;
        _mapper = mapper;
        _logger = logger;
        _fileService = fileService;
    }

    public async Task<MenuItemCreateDto> CreateMenuItemAsync(MenuItemCreateDto menuItemCreateDto)
    {
        try
        {
            var menuItem = _mapper.Map<MenuItem>(menuItemCreateDto);

            // Resim yükleme işlemi
            if (menuItemCreateDto.Image != null)
            {
                menuItem.MenuItemImageUrl = await _fileService.UploadImageAsync(menuItemCreateDto.Image, "menu-items");
                _logger.LogInformation($"Image uploaded for menu item: {menuItem.MenuItemImageUrl}");
            }

            var createdMenuItem = await _menuItemRepository.AddAsync(menuItem);
            _logger.LogInformation($"Menu item created: {createdMenuItem.Id}");
            return _mapper.Map<MenuItemCreateDto>(createdMenuItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating menu item");
            throw;
        }
    }

    public async Task<MenuItemUpdateDto> UpdateMenuItemAsync(int id, MenuItemUpdateDto menuItemUpdateDto)
    {
        try
        {
            var existingMenuItem = await _menuItemRepository.GetByIdAsync(id);
            if (existingMenuItem == null)
            {
                _logger.LogWarning($"Menu item not found for update: {id}");
                throw new KeyNotFoundException($"Menu item with id {id} not found");
            }

            // Resim güncelleme işlemi
            if (menuItemUpdateDto.Image != null)
            {
                // Eski resmi sil
                if (!string.IsNullOrEmpty(existingMenuItem.MenuItemImageUrl))
                {
                    _fileService.DeleteImage(existingMenuItem.MenuItemImageUrl);
                    _logger.LogInformation($"Old image deleted for menu item: {id}");
                }

                // Yeni resmi yükle
                existingMenuItem.MenuItemImageUrl = await _fileService.UploadImageAsync(menuItemUpdateDto.Image, "menu-items");
                _logger.LogInformation($"New image uploaded for menu item: {id}");
            }

            _mapper.Map(menuItemUpdateDto, existingMenuItem);
            var updatedMenuItem = await _menuItemRepository.UpdateAsync(existingMenuItem);
            _logger.LogInformation($"Menu item updated: {updatedMenuItem.Id}");
            return _mapper.Map<MenuItemUpdateDto>(updatedMenuItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating menu item: {id}");
            throw;
        }
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        try
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem != null && !string.IsNullOrEmpty(menuItem.MenuItemImageUrl))
            {
                _fileService.DeleteImage(menuItem.MenuItemImageUrl);
                _logger.LogInformation($"Image deleted for menu item: {id}");
            }

            var result = await _menuItemRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation($"Menu item deleted: {id}");
            }
            else
            {
                _logger.LogWarning($"Menu item not found for deletion: {id}");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting menu item: {id}");
            throw;
        }
    }

    // Diğer metodlar aynı kalabilir
    public async Task<IEnumerable<MenuItemGetDto>> GetAllMenuItemsAsync()
    {
        try
        {
            var menuItems = await _menuItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MenuItemGetDto>>(menuItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all menu items");
            throw;
        }
    }

    public async Task<MenuItemGetDto> GetMenuItemByIdAsync(int id)
    {
        try
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                _logger.LogWarning($"Menu item not found: {id}");
                throw new KeyNotFoundException($"Menu item with id {id} not found");
            }
            return _mapper.Map<MenuItemGetDto>(menuItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting menu item: {id}");
            throw;
        }
    }

    public async Task<bool> MenuItemExistsAsync(int id)
    {
        try
        {
            return await _menuItemRepository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while checking if menu item exists: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<MenuItemGetDto>> GetMenuItemsByCategoryIdAsync(int categoryId)
    {
        var menuItems = await _menuItemRepository.GetMenuItemsByCategoryIdAsync(categoryId);
        return _mapper.Map<IEnumerable<MenuItemGetDto>>(menuItems);
    }
}