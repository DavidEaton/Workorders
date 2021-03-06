﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workorders.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workorders.Data
{
    public class Workorder
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public Priority Priority { get; set; }
        public DateTime Reported { get; set; }
        public string Reporter { get; set; }
        public string Details { get; set; }
        public int? ConsumerId { get; set; }
        public string Editor { get; set; }
        public DateTime? Edited { get; set; }
        public DateTime? Closed { get; set; }
        public decimal Estimate { get; set; }
        public string Closer { get; set; }
        public string Resolution { get; set; }
        public int? PoNumber { get; set; }
        public bool Open { get; set; }

        public virtual Consumer Consumer { get; set; }
        public virtual Department Department { get; set; }       

        public DateTime? Approved { get; set; }
        public string Approver { get; set; }

        public DateTime? Rejected { get; set; }
        public string Rejector { get; set; }
        public string Rejection { get; set; }
        public string PersonServed { get; set; }
    }
}
