using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workorders.Web.ViewModels
{
    public class AboutViewModel
    {
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string CurrentYear { get; set; }
        public string UrlReferrer { get; set; }
        public string CompanyAbbreviation { get; set; }
        public string Users { get; set; }
        public string Editors { get; set; }
        public string Admins { get; set; }
    }
}