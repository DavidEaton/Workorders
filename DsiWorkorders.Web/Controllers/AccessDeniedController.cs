﻿using DsiWorkorders.Web.Helpers;
using DsiWorkorders.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DsiWorkorders.Web.Controllers
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