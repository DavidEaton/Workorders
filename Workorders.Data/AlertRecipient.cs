﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workorders.Data
{
    public class AlertRecipient
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
        public string Emails { get; set; }
    }
}
