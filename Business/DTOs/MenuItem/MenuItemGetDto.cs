using Business.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.MenuItem
{
    public class MenuItemGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int CategoryId { get; set; }
        public CategoryGetDto Category { get; set; }
        public string MenuItemImageUrl { get; set; } = string.Empty;
    }
}
