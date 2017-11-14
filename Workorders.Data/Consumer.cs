using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workorders.Data
{
    public class Consumer
    {
        private ICollection<Workorder> _workorders;

        public Consumer()
        {
            _workorders = new List<Workorder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        //public string AreaName { get; set; }
        public Boolean Active { get; set; }

        //public string ConsumerAreaName
        //{
        //    get
        //    {
        //        return String.Format("{0} ({1})", Name, AreaName);
        //    }
        //}

        public virtual ICollection<Workorder> Workorders
        {
            get { return _workorders; }
            set { _workorders = value; }
        }
    }
}
