using Business.DTOs.OrderItem;
using Business.DTOs.TableItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Order
{
    public class OrderGetDto
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public ICollection<OrderItemGetDto> OrderItems { get; set; }
        public string? CustomerNote { get; set; }
    }
}
