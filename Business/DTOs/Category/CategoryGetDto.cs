using Business.DTOs.MenuItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Category
{
    public class CategoryGetDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryImageUrl { get; set; }
        public string? Description { get; set; }
        public List<MenuItemGetDto>? MenuItems { get; set; }
    }
}
