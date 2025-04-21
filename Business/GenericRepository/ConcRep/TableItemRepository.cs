using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.GenericRepository.ConcRep
{
    public class TableItemRepository : BaseRepository<TableItem>
    {
        private readonly ILogger<TableItemRepository> _logger;

        public TableItemRepository(QRMenuDbContext context, ILogger<TableItemRepository> logger)
            : base(context)
        {
            _logger = logger;
        }

        public async Task<TableItem> GetByIdAsync(int id)
        {
            try
            {
                var tableItem = await _context.TableItems
                    .Include(ti => ti.MenuItem)
                    .Include(ti => ti.Table)
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (tableItem == null)
                {
                    _logger.LogWarning($"TableItem with ID {id} not found");
                    throw new KeyNotFoundException($"TableItem with ID {id} not found");
                }

                return tableItem;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, $"Error retrieving TableItem with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<TableItem>> GetTableItemsByTableIdAsync(int tableId)
        {
            try
            {
                _logger.LogInformation($"Retrieving items for table {tableId}");

                var items = await _context.TableItems
                    .Include(ti => ti.MenuItem)
                    .Include(ti => ti.Table)
                    .Where(ti => ti.TableId == tableId)
                    .ToListAsync();

                if (!items.Any())
                {
                    _logger.LogInformation($"No items found for table {tableId}");
                    return Enumerable.Empty<TableItem>();
                }

                _logger.LogInformation($"Found {items.Count} items for table {tableId}");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving items for table {tableId}");
                throw;
            }
        }

        public async Task<bool> TableExists(int tableId)
        {
            try
            {
                return await _context.Tables
                    .AnyAsync(t => t.Id == tableId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of table {tableId}");
                throw;
            }
        }

        public async Task<bool> MenuItemExists(int menuItemId)
        {
            try
            {
                return await _context.MenuItems
                    .AnyAsync(mi => mi.Id == menuItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of menuItem {menuItemId}");
                throw;
            }
        }

        public async Task<TableItem> AddTableItemAsync(TableItem tableItem)
        {
            try
            {
                _logger.LogInformation($"Adding new item to table {tableItem.TableId}");

                if (!await TableExists(tableItem.TableId))
                {
                    throw new KeyNotFoundException($"Table with ID {tableItem.TableId} not found");
                }

                if (!await MenuItemExists(tableItem.MenuItemId))
                {
                    throw new KeyNotFoundException($"MenuItem with ID {tableItem.MenuItemId} not found");
                }

                await _context.TableItems.AddAsync(tableItem);
                await _context.SaveChangesAsync();

                // Yeni eklenen item'ı ilişkili verileriyle birlikte getir
                return await _context.TableItems
                    .Include(ti => ti.MenuItem)
                    .Include(ti => ti.Table)
                    .SingleAsync(ti => ti.Id == tableItem.Id);
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, $"Error adding item to table {tableItem.TableId}");
                throw;
            }
        }
    }
}