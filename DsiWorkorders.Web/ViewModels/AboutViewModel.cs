using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DsiWorkorders.Web.ViewModels
{
    public class AboutViewModel
    {
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string CurrentYear { get; set; }
        public string UrlReferrer { get; set; }
    }
}