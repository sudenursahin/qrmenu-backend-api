using Business.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryGetDto>> GetAllCategoriesAsync();
        Task<CategoryGetDto> GetCategoryByIdAsync(int id);
        Task<CategoryGetDto> CategoryCreateAsync(CategoryCreateDto categoryDto);
        Task DeleteCategoryAsync(int id);
        Task<CategoryGetDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto);

    }


}
