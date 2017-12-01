using Workorders.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Workorders.Web.ViewModels.AlertRecipient;
using Workorders.Web.Filters;
using Workorders.Data.Enums;
using Workorders.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Workorders.Web.Controllers
{

    //[CustomAuthorize(AccessType = AccessType.Admins)]
    public class AlertRecipientController : Controller
    {
        AppDbContext _db = new AppDbContext();
        // GET: AlertRecipient
        public ActionResult Index()
        {
            AlertRecipientGridViewModel model = new AlertRecipientGridViewModel();
            model.Areas = UserFunctions.GetAreasSelectList(_db);

            return View(model);

        }

        [HttpPost]
        public JsonResult GetAlertRecipient([DataSourceRequest]DataSourceRequest request)
        {
            // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
            //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.
            var model = _db.AlertRecipients.ToList()
                                 .Select(m => new AlertRecipientGridViewModel
                                 {
                                     AreaName = m.Area != null ? m.Area.Name : null,
                                     Emails = m.Emails,
                                     Id = m.Id,
                                 });

            return this.Json(model.ToDataSourceResult(request));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _db.AlertRecipients.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AlertRecipientViewModel();
            viewModel.Id = id;
            viewModel.AreaId = model.AreaId;
            viewModel.Emails = model.Emails;
            viewModel.Areas = UserFunctions.GetAreasSelectList(_db, viewModel.AreaId);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(AlertRecipientViewModel viewModel)
        {
            var alertrecipient = _db.AlertRecipients.Find(viewModel.Id);

            if (ModelState.IsValid)
            {
                //update only edit only fields
                alertrecipient.AreaId = viewModel.AreaId;
                alertrecipient.Emails = viewModel.Emails;

                _db.SaveChanges();
                TempData["SuccessMessage"] = "Alert Recipient has been updated successfully";

                return RedirectToAction("Index");
            }

            viewModel.Areas = UserFunctions.GetAreasSelectList(_db, viewModel.AreaId);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        //[CustomAuthorize(AccessType = AccessType.Admins)]
        public JsonResult DeleteConfirmed(int id = 0)
        {
            AlertRecipient alertRecipient = _db.AlertRecipients.FirstOrDefault(x => x.Id == id);
            if (alertRecipient == null)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                return Json(new { success = false });
            }

            _db.AlertRecipients.Remove(alertRecipient);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Alert Recipient has been deleted successfully.";

            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult Create()
        {
            AlertRecipientViewModel model = new AlertRecipientViewModel();
            model.Areas = UserFunctions.GetAreasSelectList(_db);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(AlertRecipientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AlertRecipient alertrecipient = new AlertRecipient()
                {
                    AreaId = viewModel.AreaId,
                    Emails = viewModel.Emails,
                };

                _db.AlertRecipients.Add(alertrecipient);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Alert Recipient has been created successfully ";
                return RedirectToAction("Index");
            }

            viewModel.Areas = UserFunctions.GetAreasSelectList(_db, viewModel.AreaId);

            return View(viewModel);
        }
        public ActionResult Mobile()
        {
            AlertRecipientGridViewModel model = new AlertRecipientGridViewModel();
            model.Areas = UserFunctions.GetAreasSelectList(_db);
            return View(model);
        }


        public JsonResult GetMobileGridData([DataSourceRequest]DataSourceRequest request)
        {
            // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
            //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.
            var model = _db.AlertRecipients.ToList()

                                    .Select(m => new AlertRecipientGridViewModel
                                    {
                                        AreaName = m.Area != null ? m.Area.Name : null,
                                        Emails = m.Emails,
                                        Id = m.Id,
                                        AreaId = m.AreaId

                                    });

            var data = model;
            var requestPageSize = request.PageSize;
            request.PageSize = 0;

            //get total records
            var totalRecords = data.ToDataSourceResult(request).Data.AsQueryable().Count();
            request.PageSize = requestPageSize;

            return this.Json(new { Data = model.ToDataSourceResult(request), TotalRecords = totalRecords });
        }


        private SelectList GetMobileFilterAreasSelectList()
        {
            return new SelectList(_db.Areas
                               .ToList()
                               .Where(d => (d.Active == true))
                               .OrderBy(d => d.Name), "Id", "Name");
        }


    }
}