using Business.GenericRepository.BaseRep;
using Business.GenericRepository.IntRep;
using CoreL.Enums;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.GenericRepository.ConcRep
{
    public class OrderRepository : BaseRepository<Order>
    {
        private QRMenuDbContext _db;
        public OrderRepository(QRMenuDbContext context) : base(context) 
        {
            _db = context;
            
        }
        public IQueryable<Order> Table => _dbSet;

        public IQueryable<Order> Orders => _context.Orders;
        public  async Task<IEnumerable<Order>> GetAll(int page, int size)
        {
            return await _dbSet
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public  async Task<IEnumerable<Order>> GetAllItems()
        {
            return await _db.Orders
               .Include(o => o.OrderItems)
               .ToListAsync();
        }


        public async Task<IEnumerable<Order>> GetOrdersByTableIdAsync(int tableId)
        {
            return await _dbSet
                .Where(o => o.TableId == tableId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();
        }
    }
}