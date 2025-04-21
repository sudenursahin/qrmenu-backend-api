using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.TableItem
{
    public class TableItemCreateDto
    {
        public int TableId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
