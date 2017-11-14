using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workorders.Data;
using Workorders.Data.Enums;

namespace Workorders.Web.ViewModels
{
    public class WorkorderCreateViewModel
    {
        [Display(Name = 
            "Department", Description = "Select Department.")]
        [Required]                  public int DepartmentId { get; set; }

        [Display(Name = "Area", Description = "Select Department Area.")]
        [Required]
        public int DepartmentAreaId { get; set; }

        [Required]                  public Priority Priority { get; set; }
        [DataType(DataType.Date)]
        [Required]
        private DateTime reported;
        public DateTime Reported
        {
            get { return this.reported; }
            set
            {
                this.reported = new DateTime(value.Ticks, DateTimeKind.Utc);
            }
        }

        [Required]
        [MaxLength(255)]            public string Details { get; set; }
                                    public string Reporter { get; set; }
        [Display(Name = 
            "Result of Person Served", Description = "Select person resposible for issue.")]
                                    public Nullable<int> ConsumerId { get; set; }

                                   public SelectList Departments { get; set; }
                                   public SelectList Consumers { get; set; }
                                   public SelectList Priorities { get; set; }
                                   public SelectList Areas { get; set; }


                                    public WorkorderCreateViewModel()
                                    {
                                        Reported = DateTime.Today;
                                        Priority = Priority.Normal;
                                        //Reporter = User.Identity.Name;
                                    }

        public string PersonServed { get; set; }
        public bool ResultOfPersonServed { get; set; }

    }
}