using Business.DTOs.MenuItem;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

    }
}
