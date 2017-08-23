using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DsiWorkorders.Data.Enums;

namespace DsiWorkorders.Data
{
    public class Workorder
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public Priority Priority { get; set; }
        public DateTime Reported { get; set; }
        public string Reporter { get; set; }
        public string Details { get; set; }
        public Nullable<int> ConsumerId { get; set; }
        public string Editor { get; set; }
        public Nullable<System.DateTime> Edited { get; set; }
        public Nullable<System.DateTime> Closed { get; set; }
        public decimal Estimate { get; set; }
        public string Closer { get; set; }
        public string Resolution { get; set; }
        public Nullable<int> PoNumber { get; set; }

        public virtual Consumer Consumer { get; set; }
        public virtual Department Department { get; set; }
        public bool? SupervisorApproved { get; set; }
        public string Supervisor { get; set; }
    }
}
