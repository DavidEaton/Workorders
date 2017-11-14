using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workorders.Data;
using Workorders.Data.Enums;
using Workorders.Web.Helpers;

namespace Workorders.Web.ViewModels
{
  public class WorkorderDetailViewModel
  {
    public int Id { get; set; }

    [Required]
    [Display(Name = "Department", Description = "Select Department.")]
    public int DepartmentId { get; set; }

    [Display(Name = "Area", Description = "Select Department Area.")]
    public int DepartmentAreaId { get; set; }

    [Required]
    public Priority Priority { get; set; }

    [Required]
    [DataType(DataType.Date)]
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
    [MaxLength(255)]
    public string Details { get; set; }

    [Required]
    public string Reporter { get; set; }

    [Display(Name = "Individual", Description = "Select individual resposible for issue.")]
    public Nullable<int> ConsumerId { get; set; }
    

    public SelectList Departments { get; set; }
    public SelectList Priorities { get; set; }
    public SelectList Consumers { get; set; }
    public SelectList Areas { get; set; }

    public string PersonServed { get; set; }

    public Nullable<System.DateTime> Rejected { get; set; }
    public string Rejector { get; set; }
    public string Rejection { get; set; }

    }
}