using CoreL.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * Price;
    }
}