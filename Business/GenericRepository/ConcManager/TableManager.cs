using AutoMapper;
using Business.DTOs.Table;
using Business.DTOs.TableItem;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using CoreL.Enums;
using CoreL.ServiceClasses;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class TableManager : ITableService
    {
        private readonly TableRepository _tableRepository;
        private readonly OrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly QRMenuDbContext _context;
        private readonly IQRCodeService _qrCodeService;
        private readonly ILogger<TableManager> _logger;

        public TableManager(
            TableRepository tableRepository,
            OrderRepository orderRepository,
            IMapper mapper,
            QRMenuDbContext context,
            IQRCodeService qrCodeService,
            ILogger<TableManager> logger)
        {
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
            _qrCodeService = qrCodeService;
            _logger = logger;
        }

        public async Task<TableGetDto> GetTableByIdAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            return _mapper.Map<TableGetDto>(table);
        }

        public async Task<IEnumerable<TableGetDto>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TableGetDto>>(tables);
        }


        public async Task<TableGetDto> CreateTableAsync(TableCreateDto tableCreateDto)
        {
            try
            {
                var table = new Table
                {
                    Status = tableCreateDto.Status,
                    QRCode = string.Empty, // QRCode başlangıçta boş olabilir
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                // Orders ve TableItems constructor'da initialize ediliyor

                await _tableRepository.AddAsync(table);
                await _context.SaveChangesAsync(); // Explicit SaveChanges

                return _mapper.Map<TableGetDto>(table);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating table: {Message}", ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async Task<TableGetDto> UpdateTableAsync(int id, TableUpdateDto tableUpdateDto)
        {
            var existingTable = await _tableRepository.GetByIdAsync(id);
            if (existingTable == null)
                throw new KeyNotFoundException($"Table with id {id} not found.");

            _mapper.Map(tableUpdateDto, existingTable);
            await _tableRepository.UpdateAsync(existingTable);
            return _mapper.Map<TableGetDto>(existingTable);
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
            {
                return false;
            }

            await _tableRepository.DeleteAsync(id);
            return true;
        }

        public async Task<TableGetDto> AssignOrderToTableAsync(int tableId, int orderId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {tableId} not found.");

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found.");

            if (order.TableId != tableId)
            {
                order.TableId = tableId;
                await _orderRepository.UpdateAsync(order);
            }

            if (!table.Orders.Any(o => o.Id == orderId))
            {
                table.Orders.Add(order);
                await _tableRepository.UpdateAsync(table);
            }

            return _mapper.Map<TableGetDto>(table);
        }

        public async Task<TableGetDto> ClearTableAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {id} not found.");

            foreach (var order in table.Orders.ToList())
            {
                order.Table = null;
                await _orderRepository.UpdateAsync(order);
            }
            table.Orders.Clear();
            table.TableItems.Clear();
            table.Status = TableStatus.Available;

            await _tableRepository.UpdateAsync(table);
            return _mapper.Map<TableGetDto>(table);
        }

        public async Task<TableGetDto> AddItemToTableAsync(int tableId, TableItemCreateDto tableItemCreateDto)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {tableId} not found.");

            var tableItem = _mapper.Map<TableItem>(tableItemCreateDto);
            table.TableItems.Add(tableItem);

            await _tableRepository.UpdateAsync(table);
            return _mapper.Map<TableGetDto>(table);
        }

        public async Task<TableGetDto> RemoveItemFromTableAsync(int tableId, int tableItemId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {tableId} not found.");

            var tableItem = table.TableItems.FirstOrDefault(ti => ti.Id == tableItemId);
            if (tableItem == null)
                throw new KeyNotFoundException($"TableItem with id {tableItemId} not found in table {tableId}.");

            table.TableItems.Remove(tableItem);
            await _tableRepository.UpdateAsync(table);
            return _mapper.Map<TableGetDto>(table);
        }

        public async Task<decimal> GetTableTotalAsync(int tableId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {tableId} not found.");

            return table.TableItems.Sum(ti => ti.MenuItem.Price * ti.Quantity);
        }

        public async Task<byte[]> GenerateTableQRCodeAsync(int tableId, string baseUrl)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {tableId} not found.");

            return _qrCodeService.GenerateQRCode(tableId, baseUrl);
        }

        public async Task<byte[]> GetAllTableQRCodesZipAsync(string baseUrl)
        {
            var tables = await _tableRepository.GetAllAsync();
            if (!tables.Any())
                throw new KeyNotFoundException("No tables found.");

            var qrCodes = new List<(string fileName, byte[] content)>();

            foreach (var table in tables)
            {
                byte[] qrCodeBytes = _qrCodeService.GenerateQRCode(table.Id, baseUrl);
                qrCodes.Add(($"masa-{table.Id}-qr.png", qrCodeBytes));
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    foreach (var qrCode in qrCodes)
                    {
                        var entry = archive.CreateEntry(qrCode.fileName);
                        using (var entryStream = entry.Open())
                        {
                            await entryStream.WriteAsync(qrCode.content, 0, qrCode.content.Length);
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}