using CoreL.Domain;
using CoreL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Table : BaseEntity
    {
        public ICollection<Order> Orders { get; set; } // A table can have more than 1 order.
        public ICollection<TableItem> TableItems { get; set; }
        public TableStatus Status { get; set; }
        public string QRCode { get; set; }
        public Table()
        {
            Orders = new List<Order>();
            TableItems = new List<TableItem>();
        }

    }
}
