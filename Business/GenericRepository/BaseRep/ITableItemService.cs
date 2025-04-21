using Business.DTOs.TableItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{

    public interface ITableItemService
    {
        // CRUD operations
        Task<TableItemGetDto> GetTableItemByIdAsync(int tableId);
        Task<IEnumerable<TableItemGetDto>> GetAllTableItemsAsync();
        Task<TableItemGetDto> CreateTableItemAsync(int tableId ,TableItemCreateDto tableItemCreateDto);
        Task<TableItemGetDto> UpdateTableItemAsync(int id, TableItemUpdateDto tableItemUpdateDto);
        Task<bool> DeleteTableItemAsync(int tableId , int tableItemId);

        // Additional operations
        Task<IEnumerable<TableItemGetDto>> GetTableItemsByTableIdAsync(int tableId);
        Task<decimal> GetTotalPriceForTableItemAsync(int id);
        Task<TableItemGetDto> UpdateTableItemAsync(int tableId, int itemId, TableItemUpdateDto tableItemUpdateDto);
    }
}
