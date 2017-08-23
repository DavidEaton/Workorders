using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsiWorkorders.Data
{
    public class Area
    {
        private ICollection<Department> _departments;

        public Area()
        {
            _departments = new List<Department>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Department> Departments
        {
            get { return _departments; }
            set { _departments = value; }
        }
    }
}
