using CoreL.Domain;
using CoreL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order : BaseEntity
    {
        public int TableId { get; set; } // Foreign key
        public Table Table { get; set; } // Navigation property
        public OrderStatus Status { get; set; } // Sipariş durumu
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Sipariş öğeleri
        public DateTime OrderDate { get; set; } // Sipariş tarihi
        public string? CustomerNote { get; set; } // Müşteri notu/mesajı

        public Order()
        {
            OrderDate = DateTime.Now;
            Status = OrderStatus.Preparing;
        }




    }
}
