using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.OrderItem
{
    public class OrderItemUpdateDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
