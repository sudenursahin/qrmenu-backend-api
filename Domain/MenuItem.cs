using CoreL.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MenuItem : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string MenuItemImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        public int CategoryId { get; set; }

        // Collection'ları initialize edelim
        public virtual ICollection<TableItem> TableItems { get; set; } = new List<TableItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
