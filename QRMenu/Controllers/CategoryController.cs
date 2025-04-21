using Microsoft.AspNetCore.Mvc;
using Business.DTOs.Category;
using Business.GenericRepository.ConcManager;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Business.GenericRepository.BaseRep;
using AutoMapper;

namespace QRMenu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger, IMapper mapper)
        {
            _categoryService = categoryService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryGetDto>>> GetList()
        {
            try
            {
                var categoriesDto = await _categoryService.GetAllCategoriesAsync();
                return Ok(categoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryGetDto>> GetItem(int id)
        {
            try
            {
                var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(categoryDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Category with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting category {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryGetDto>> PostItem([FromForm] CategoryCreateDto categoryCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdCategory = await _categoryService.CategoryCreateAsync(categoryCreateDto);
                return CreatedAtAction(nameof(GetItem), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                if (id != categoryUpdateDto.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryUpdateDto);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Category with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Category with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}