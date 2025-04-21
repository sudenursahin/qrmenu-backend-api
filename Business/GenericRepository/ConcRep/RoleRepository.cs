using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcRep
{
    public class RoleRepository : BaseRepository<Role>
    {
        private readonly QRMenuDbContext _db;

        public RoleRepository(QRMenuDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Role>> GetItems()
        {
            return await _db.Roles.Include(u => u.UserRoles)
                .ToListAsync();
        }

        public async Task<Role?> GetItem(int id)
        {
            return await _db.Roles.Include(u => u.UserRoles).SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByIdAsync(int roleId)
        {
            return await _db.Set<Role>().FindAsync(roleId);
        }

    }
}
