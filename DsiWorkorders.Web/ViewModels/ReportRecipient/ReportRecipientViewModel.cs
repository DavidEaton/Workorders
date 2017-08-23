using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DsiWorkorders.Web.ViewModels.ReportRecipient
{
    public class ReportRecipientViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Area")]
        public int AreaId { get; set; }

        [Required]
        [RegularExpression(@"^[\W]*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4}[\W]*;{1}[\W]*)*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4})[\W]*$", ErrorMessage = "Invalid email(s)")]
        public string Emails { get; set; }
        public SelectList Areas { get; set; }

    }
}