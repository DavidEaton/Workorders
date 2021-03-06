﻿using Workorders.Web.Helpers;
using System.Linq;
using System.Web.Mvc;
using Workorders.Web.ViewModels.ReportRecipient;
using Workorders.Data.Enums;
using Workorders.Web.Filters;
using Workorders.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.Collections.Generic;

namespace Workorders.Web.Controllers
{
    //[CustomAuthorize(AccessType = AccessType.Admins)]
    public class ReportRecipientController : Controller
    {
        AppDbContext _db = new AppDbContext(Settings.GetConnectionStringName());
        // GET: ReportRecipients
        [CustomAuthorize(AccessType = AccessType.Admins)]
        public ActionResult Index()
        {
            ReportRecipientGridViewModel model = new ReportRecipientGridViewModel();
            var selectedCompany = CompanyCookie.SelectedCompany;
            model.Companies = UserFunctions.GetCompaniesSelectList(selectedCompany);
            model.SelectedCompany = selectedCompany;
            model.Areas = UserFunctions.GetAreasSelectList(_db);


            return View(model);
        }

        public void TestSendEmails()
        {
            var companies = new List<CompanyEnum>
            {
                CompanyEnum.CSI,
                CompanyEnum.DSI,
                CompanyEnum.DSN
            };

            foreach (var company in companies)
            {
                string companyConnectionString = null;
                companyConnectionString = ConfigurationManager.AppSettings[company.ToString()];
                AppDbContext _db = new AppDbContext(companyConnectionString);
                //get al areas
                var areas = _db.Areas.ToList();

                foreach (var area in areas)
                {

                    string toBeSentEmails = "davidzaeaton@outlook.com";
                    string emailBodyText = ReportEmailMessage.GetReportEmailMessage(company, area.Name, area.Id, _db);

                    //send email 
                    Email.SendEmail(toBeSentEmails, company + " Weekly Maintenance Work Orders Report", emailBodyText, emailBodyText, null, null);
                }
            }

        }


        [HttpPost]
        public JsonResult GetReportRecipient([DataSourceRequest]DataSourceRequest request)
        {
            var model = _db.ReportRecipients.ToList()
                                 .Select(m => new ReportRecipientGridViewModel
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
            var selectedCompany = CompanyCookie.SelectedCompany;

            if (string.IsNullOrEmpty(selectedCompany))
            {
                return RedirectToAction("Index");
            }

            var model = _db.ReportRecipients.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ReportRecipientViewModel();

            viewModel.Id = id;
            viewModel.AreaId = model.AreaId;
            viewModel.Emails = model.Emails;
            viewModel.Areas = UserFunctions.GetAreasSelectList(_db, viewModel.AreaId);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ReportRecipientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var reportrecipient = _db.ReportRecipients.Find(viewModel.Id);

                //update only edit only fields
                reportrecipient.Emails = viewModel.Emails;
                reportrecipient.AreaId = viewModel.AreaId;

                _db.SaveChanges();

                TempData["SuccessMessage"] = "Report Recipient has been updated successfully";

                return RedirectToAction("Index");
            }

            viewModel.Areas = UserFunctions.GetAreasSelectList(_db, viewModel.AreaId);

            return View(viewModel);

        }
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(AccessType = AccessType.Admins)]
        public JsonResult DeleteConfirmed(int id = 0)
        {
            ReportRecipient reportRecipient = _db.ReportRecipients.FirstOrDefault(x => x.Id == id);
            if (reportRecipient == null)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                return Json(new { success = false });
            }

            _db.ReportRecipients.Remove(reportRecipient);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Report Recipient has been deleted successfully.";

            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult Create()
        {
            ReportRecipientViewModel model = new ReportRecipientViewModel();
            model.Areas = UserFunctions.GetAreasSelectList(_db);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReportRecipientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ReportRecipient reportrecipient = new ReportRecipient()
                {
                    AreaId = viewModel.AreaId,
                    Emails = viewModel.Emails,
                };

                _db.ReportRecipients.Add(reportrecipient);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Report Recipient has been created successfully.";

                return RedirectToAction("Index");
            }

            viewModel.Areas = UserFunctions.GetAreasSelectList(_db, viewModel.AreaId);
            return View(viewModel);
        }

        public ActionResult Mobile()
        {
            ReportRecipientGridViewModel model = new ReportRecipientGridViewModel();
            model.Areas = UserFunctions.GetAreasSelectList(_db);
            return View(model);
        }

        //public JsonResult GetMobileGridData([DataSourceRequest]DataSourceRequest request)
        //{
        //    // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
        //    //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.
        //    var model = _db.ReportRecipients.ToList()

        //                            .Select(m => new ReportRecipientGridViewModel
        //                            {
        //                                AreaName = m.Area != null ? m.Area.Name : null,
        //                                Emails = m.Emails,
        //                                Id = m.Id,
        //                                AreaId = m.AreaId

        //                            });

        //    var data = model;
        //    var requestPageSize = request.PageSize;
        //    request.PageSize = 0;

        //    //get total records
        //    var totalRecords = data.ToDataSourceResult(request).Data.AsQueryable().Count();
        //    request.PageSize = requestPageSize;

        //    return this.Json(new { Data = model.ToDataSourceResult(request), TotalRecords = totalRecords });
        //}


        //private SelectList GetMobileFilterAreasSelectList()
        //{
        //    return new SelectList(_db.Areas
        //                       .ToList()
        //                       .Where(d => (d.Active == true))
        //                       .OrderBy(d => d.Name), "Id", "Name");
        //}
    }
}