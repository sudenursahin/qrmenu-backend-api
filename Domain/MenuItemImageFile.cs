using CoreL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MenuItemImageFile : BaseEntity
    {

        public bool Showcase { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage { get; set; }

    }
}







