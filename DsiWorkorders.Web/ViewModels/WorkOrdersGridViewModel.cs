using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DsiWorkorders.Data.Enums;
using DsiWorkorders.Web.Helpers;

namespace DsiWorkorders.Web.ViewModels
{
    public class WorkOrdersGridViewModel
    {
        public int Id { get; set; }
        public string DepartmentAreaName { get; set; }
        public string DepartmentName { get; set; }
        private DateTime reported;
        public DateTime Reported
        {
            get { return this.reported; }
            set
            {
                this.reported = new DateTime(value.Ticks, DateTimeKind.Utc);
            }
        }

        private DateTime? closed;
        public DateTime? Closed
        {
            get { return this.closed; }
            set
            {
                this.closed = value.HasValue ? new DateTime(value.Value.Ticks, DateTimeKind.Utc) : (DateTime?)null;
            }
        }

        public string Details { get; set; }
        public Priority Priority { get; set; }
        public string ConsumerName { get; set; }
        public string Closer { get; set; }
        public string Resolution { get; set; }
        public decimal? Estimate { get; set; }

        public Boolean Open
        {
            get
            {
                if (Closed == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public DateTime Due
        {
            get { return Reported.AddDays(Priority.GetDays()); }
        }

        public Boolean Overdue
        {
            get { return Open ? (Due < DateTime.Today) : (Due < Closed); }
        }

        public int DaysOpen
        {
            get { return Open ? (DateTime.Today - Reported).Days : (Closed.Value - Reported).Days; }
        }

        public int DaysOverdue
        {
            get
            {
                if (Overdue)
                {
                    return Open ? (DateTime.Today - Due).Days : (Closed.Value - Due).Days;
                }

                return 0;
            }
        }

        //public bool Approved { get; set; }
        public Nullable<System.DateTime> Approved { get; set; }

        //required for mobile grid filter dropdowns
        public SelectList Departments { get; set; }
        public SelectList Areas { get; set; }
        public SelectList Priorities { get; set; }
        public SelectList Closers { get; set; }
        public string SelectedCompany { get; set; }
        public SelectList Companies { get; set; }

        public DateTime? Rejected { get; set; }
        public string Rejector { get; set; }
    }

}