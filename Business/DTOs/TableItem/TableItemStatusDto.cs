using CoreL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.TableItem
{
    public class TableItemStatusDto
    {
        public int Id { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderStatus Status { get; set; }
    }
}
