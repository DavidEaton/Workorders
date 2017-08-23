using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foolproof;
using DsiWorkorders.Data;
using DsiWorkorders.Data.Enums;
using DsiWorkorders.Web.Helpers;

namespace DsiWorkorders.Web.ViewModels
{
    public class WorkorderEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department", Description = "Select Department.")]
        public int DepartmentId { get; set; }

        [Display(Name = "Area", Description = "Select Department Area.")]
        [Required]
        public int DepartmentAreaId { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
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

        [Required]
        [MaxLength(255)]
        public string Details { get; set; }

        [Required]
        public string Reporter { get; set; }

        [Display(Name = "Individual", Description = "Select individual resposible for issue.")]
        public Nullable<int> ConsumerId { get; set; }

        public string Editor { get; set; }
        public Nullable<System.DateTime> Edited { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true)]
        public decimal Estimate { get; set; }

        [RequiredIfNotEmpty("Closed", ErrorMessage = "Please enter {0} to Close workorder")]
        public string Closer { get; set; }

        [RequiredIfNotEmpty("Closed", ErrorMessage = "Please enter {0} to Close workorder")]
        public string Resolution { get; set; }

        [Display(Name = "PO Number")]
        public Nullable<int> PoNumber { get; set; }


        public virtual Consumer Consumer { get; set; }
        public virtual Department Department { get; set; }

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

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Due
        {
            get { return Reported.AddDays(Priority.GetDays()); }
        }


        [Display(Name = "Days Open")]
        public int DaysOpen
        {
            get
            {
                return Open ? (DateTime.Today - Reported).Days : (Closed.Value - Reported).Days;
            }
        }

        public Boolean Overdue
        {
            get { return Open ? (Due < DateTime.Today) : (Due < Closed); }
        }

        [Display(Name = "Days Overdue")]
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
        public bool SupervisorApproved { get; set; }

        public SelectList Departments { get; set; }
        public SelectList Priorities { get; set; }
        public SelectList Consumers { get; set; }
        public SelectList Areas { get; set; }

    }
}