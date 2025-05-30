﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class QRToken
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsActive { get; set; }
        public Table Table { get; set; }
    }
}
