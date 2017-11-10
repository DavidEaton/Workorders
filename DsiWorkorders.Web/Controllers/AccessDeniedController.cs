using Workorders.Web.Helpers;
using Workorders.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Workorders.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        // GET: AccessDenied
        public ActionResult Index()
        {
            var model = new WorkOrdersGridViewModel();
            return View(model);
        }
    }
}