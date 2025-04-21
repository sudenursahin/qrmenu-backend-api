using CoreL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Category : BaseEntity
    {

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryImageUrl { get; set; } = string.Empty;
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
