using Business.DTOs.Category;
using Business.DTOs.MenuItem;
using Business.DTOs.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.TableItem
{
    public class TableItemGetDto
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int MenuItemId { get; set; }
        public MenuItemGetDto MenuItem { get; set; }
        public decimal MenuItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
