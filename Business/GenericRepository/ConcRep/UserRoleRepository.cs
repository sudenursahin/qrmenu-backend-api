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
    public class UserRoleRepository : BaseRepository<UserRole>
    {

        private readonly QRMenuDbContext _db;

        public UserRoleRepository(QRMenuDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserRole>> GetItems()
        {
            return await _db.UserRoles.Include(t => t.User)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<UserRole?> GetItem(int id)
        {
            return await _db.UserRoles.Include(t => t.User)
                .Include(u => u.Role)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

    }
}
