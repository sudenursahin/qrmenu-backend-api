using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcRep
{
    public class MenuItemRepository : BaseRepository<MenuItem>
    {
        public MenuItemRepository(QRMenuDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _dbSet.Include(m => m.Category).ToListAsync();
        }

        public override async Task<MenuItem> GetByIdAsync(int id)
        {
            return await _dbSet.Include(m => m.Category)
                               .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryIdAsync(int categoryId)
        {
            return await _dbSet.Where(m => m.CategoryId == categoryId).ToListAsync();
        }


    }
}