using Business.DTOs.Table;
using Business.DTOs.TableItem;
using CoreL.Enums;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
   
        public interface ITableService
        {
            // CRUD operations
            Task<TableGetDto> GetTableByIdAsync(int id);
            Task<IEnumerable<TableGetDto>> GetAllTablesAsync();
            Task<TableGetDto> CreateTableAsync(TableCreateDto tableCreateDto);
            Task<TableGetDto> UpdateTableAsync(int id, TableUpdateDto tableUpdateDto);
            Task<bool> DeleteTableAsync(int id);

            // Additional operations
            Task<TableGetDto> AssignOrderToTableAsync(int tableId, int orderId);
            Task<TableGetDto> ClearTableAsync(int id);
            Task<TableGetDto> AddItemToTableAsync(int tableId, TableItemCreateDto tableItemCreateDto);
            Task<TableGetDto> RemoveItemFromTableAsync(int tableId, int tableItemId);
            Task<decimal> GetTableTotalAsync(int tableId);

            // QR Code operations
            Task<byte[]> GenerateTableQRCodeAsync(int tableId, string baseUrl);
            Task<byte[]> GetAllTableQRCodesZipAsync(string baseUrl);
        }

    
}
