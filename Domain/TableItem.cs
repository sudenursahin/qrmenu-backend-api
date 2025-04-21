using CoreL.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class TableItem : BaseEntity
    {
        public int TableId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public virtual Table Table { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * (MenuItem?.Price ?? 0);
    }
}