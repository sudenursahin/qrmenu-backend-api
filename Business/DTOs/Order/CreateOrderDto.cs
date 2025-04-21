using Business.DTOs.TableItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Order
{
    public class CreateOrderDto
    {
        public int TableId { get; set; }
        public string? CustomerNote { get; set; }

    }
}
