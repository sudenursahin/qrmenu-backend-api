using AutoMapper;
using Business.DTOs.TableItem;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{

    public class TableItemManager : ITableItemService
    {
        private readonly TableItemRepository _tableItemRepository;
        private readonly MenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public TableItemManager(TableItemRepository tableItemRepository, IMapper mapper, MenuItemRepository menuItemRepository)
        {
            _tableItemRepository = tableItemRepository;
            _mapper = mapper;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<TableItemGetDto> GetTableItemByIdAsync(int tableId)
        {
            var tableItem = await _tableItemRepository.GetByIdAsync(tableId);
            return _mapper.Map<TableItemGetDto>(tableItem);
        }

        public async Task<IEnumerable<TableItemGetDto>> GetAllTableItemsAsync()
        {
            var tableItems = await _tableItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TableItemGetDto>>(tableItems);
        }

        public async Task<TableItemGetDto> CreateTableItemAsync(int tableId , TableItemCreateDto tableItemCreateDto)
        {
            var tableItem = _mapper.Map<TableItem>(tableItemCreateDto);
            await _tableItemRepository.AddAsync(tableItem);
            return _mapper.Map<TableItemGetDto>(tableItem);
        }

        public async Task<TableItemGetDto> UpdateTableItemAsync(int tableId, TableItemUpdateDto tableItemUpdateDto)
        {
            var existingTableItem = await _tableItemRepository.GetByIdAsync(tableId);
            if (existingTableItem == null)
                throw new ArgumentException("TableItem not found");

            _mapper.Map(tableItemUpdateDto, existingTableItem);
            await _tableItemRepository.UpdateAsync(existingTableItem);
            return _mapper.Map<TableItemGetDto>(existingTableItem);
        }



        public async Task<bool> DeleteItem(int id)
        {

            var tableItem = await _tableItemRepository.GetByIdAsync(id);
            if (tableItem == null)
            {
                return false;
            }

            var result = await _tableItemRepository.DeleteAsync(id);

            return true;

        }

        public async Task<IEnumerable<TableItemGetDto>> GetTableItemsByTableIdAsync(int tableId)
        {
            var tableItems = await _tableItemRepository.GetTableItemsByTableIdAsync(tableId);

            if (tableItems == null)
            {
                return new List<TableItemGetDto>();
            }

            return _mapper.Map<IEnumerable<TableItemGetDto>>(tableItems);
        }

        public async Task<TableItemGetDto> UpdateTableItemAsync(int tableId, int tableItemId, TableItemUpdateDto tableItemUpdateDto)
        {
            // First verify the table item exists and belongs to the specified table
            var existingTableItem = await _tableItemRepository.GetByIdAsync(tableItemId);
            if (existingTableItem == null || existingTableItem.TableId != tableId)
                throw new ArgumentException("TableItem not found or does not belong to the specified table");

            // Verify the menu item exists
            var menuItem = await _menuItemRepository.GetByIdAsync(tableItemUpdateDto.MenuItemId);
            if (menuItem == null)
                throw new ArgumentException("MenuItem not found");

            // Update only specific properties
            existingTableItem.MenuItemId = tableItemUpdateDto.MenuItemId;
            existingTableItem.Quantity = tableItemUpdateDto.Quantity;

            await _tableItemRepository.UpdateAsync(existingTableItem);
            return _mapper.Map<TableItemGetDto>(existingTableItem);
        }

        public async Task<decimal> GetTotalPriceForTableItemAsync(int id)
        {
            var tableItem = await _tableItemRepository.GetByIdAsync(id);
            if (tableItem == null)
                throw new ArgumentException("TableItem not found");

            // Assuming there's a Price property in the TableItem entity
            return tableItem.Quantity * tableItem.MenuItem.Price;
        }

        public async Task<bool> DeleteTableItemAsync(int tableId, int tableItemId)
        {
            // Önce item'ı bul ve table'a ait olduğunu doğrula
            var tableItem = await _tableItemRepository.GetByIdAsync(tableItemId);

            if (tableItem == null || tableItem.TableId != tableId)
            {
                throw new KeyNotFoundException("Table item not found or does not belong to the specified table");
            }

            await _tableItemRepository.DeleteAsync(tableItemId);
            await _tableItemRepository.SaveChangesAsync();

            return true;
        }


        public Task<TableItemGetDto> UpdateTableItemQuantityAsync(int tableId, TableItemUpdateDto tableItemUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
