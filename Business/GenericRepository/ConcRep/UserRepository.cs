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

    public class UserRepository :BaseRepository<User>
    {
        private QRMenuDbContext _db;
        public UserRepository(QRMenuDbContext db) : base(db)
        {
            _db = db;
        }

        public IQueryable<User> GetItemsAsQueryable()
        {
            return _db.User.AsQueryable();
        }


    }
}
