using Business.GenericRepository.BaseRep;
using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcRep
{
    public class TableRepository : BaseRepository<Table>
    {
        public TableRepository(QRMenuDbContext context) : base(context) 
        {
            
        }
    }
}
