using AutoMapper;
using Business.DTOs.Category;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using CoreL.ServiceClasses;
using Domain;
using Microsoft.Extensions.Logging;

public class CategoryManager : ICategoryService
{
    private readonly CategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryManager> _logger;
    private readonly IFileService _fileService;

    public CategoryManager(
        CategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<CategoryManager> logger,
        IFileService fileService)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _fileService = fileService;
    }

    public async Task<CategoryGetDto> CategoryCreateAsync(CategoryCreateDto categoryCreateDto)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryCreateDto);

            if (categoryCreateDto.Image != null)
            {
                category.CategoryImageUrl = await _fileService.UploadImageAsync(categoryCreateDto.Image, "categories");
                _logger.LogInformation($"Image uploaded for category: {category.CategoryImageUrl}");
            }

            await _categoryRepository.AddAsync(category);
            _logger.LogInformation($"Category created: {category.Id}");

            return _mapper.Map<CategoryGetDto>(category);  // CategoryGetDto döndür
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating category");
            throw;
        }
    }

    public async Task<CategoryGetDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category not found for update: {id}");
                throw new KeyNotFoundException($"Category with id {id} not found");
            }

            // Resim güncelleme işlemi
            if (categoryUpdateDto.Image != null)
            {
                // Eski resmi sil
                if (!string.IsNullOrEmpty(category.CategoryImageUrl))
                {
                    _fileService.DeleteImage(category.CategoryImageUrl);
                    _logger.LogInformation($"Old image deleted for category: {id}");
                }

                // Yeni resmi yükle
                category.CategoryImageUrl = await _fileService.UploadImageAsync(categoryUpdateDto.Image, "categories");
                _logger.LogInformation($"New image uploaded for category: {id}");
            }

            _mapper.Map(categoryUpdateDto, category);
            await _categoryRepository.UpdateAsync(category);
            _logger.LogInformation($"Category updated: {id}");

            return _mapper.Map<CategoryGetDto>(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating category: {id}");
            throw;
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category not found for deletion: {id}");
                throw new KeyNotFoundException($"Category with id {id} not found");
            }

            // Resmi sil
            if (!string.IsNullOrEmpty(category.CategoryImageUrl))
            {
                _fileService.DeleteImage(category.CategoryImageUrl);
                _logger.LogInformation($"Image deleted for category: {id}");
            }

            var result = await _categoryRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation($"Category deleted: {id}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting category: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<CategoryGetDto>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryGetDto>>(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all categories");
            throw;
        }
    }

    public async Task<CategoryGetDto> GetCategoryByIdAsync(int id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category not found: {id}");
                throw new KeyNotFoundException($"Category with id {id} not found");
            }
            return _mapper.Map<CategoryGetDto>(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting category: {id}");
            throw;
        }
    }
}