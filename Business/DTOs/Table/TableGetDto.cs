using Business.DTOs.Order;
using Business.DTOs.TableItem;
using CoreL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Table
{
    public class TableGetDto
    {
        public int Id { get; set; }
        public TableStatus Status { get; set; }
        public string QRCode { get; set; }
    }
}
