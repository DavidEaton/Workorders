using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workorders.Data
{
    public class DepartmentSupervisorEmail
    {
        [Key]
        public int DepartmentId { get; set; }
        public string SupervisorName { get; set; }
        public string EmailAddress { get; set; }
    }
}
