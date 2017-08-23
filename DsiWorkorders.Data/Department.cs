using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsiWorkorders.Data
{
    public class Department
    {
        private ICollection<Workorder> _workorders;

        public Department()
        {
            _workorders = new List<Workorder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public int AreaID { get; set; }
        public Area Area { get; set; }
        public Nullable<bool> Active { get; set; }
        public string AreaName { get; set; }
        public string ServiceType { get; set; }

        public string DepartmentFullName
        {
            get
            {
                return String.Format("{0} ({1})", Name, AreaName);
            }
        }


        public virtual ICollection<Workorder> Workorders
        {
            get { return _workorders; }
            set { _workorders = value; }
        }
    }
}
