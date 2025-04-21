using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcRep
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(QRMenuDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet.Include(c => c.MenuItems).ToListAsync();
        }

        public override async Task<Category> GetByIdAsync(int id)
        {
            return await _dbSet.Include(c => c.MenuItems)
                               .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithMenuItemsAsync()
        {
            return await _dbSet.Include(c => c.MenuItems).ToListAsync();
        }

        public async Task<Category> GetCategoryWithMenuItemsAsync(int id)
        {
            return await _dbSet.Include(c => c.MenuItems)
                               .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}