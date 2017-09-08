using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DsiWorkorders.Data;
using DsiWorkorders.Data.Enums;
using DsiWorkorders.Web.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.Security.Claims;
using DsiWorkorders.Web.Filters;
using DsiWorkorders.Web.Helpers;
using DsiShifts.Data.Enums;

namespace DsiWorkorders.Web.Controllers
{
    [CustomAuthorize(AccessType = AccessType.Users)]
    public class WorkordersController : Controller
    {
        AppDbContext _db = new AppDbContext(Settings.GetConnectionStringName());

        public JsonResult GetOpen([DataSourceRequest]DataSourceRequest request, bool isOpen)
        {           
            var model = _db.Workorders
                                .OrderBy(w => w.Department.AreaName)
                                .ThenBy(w => w.Department.Name)
                                //.Where(w => w.Closed == null && w.Approved == isOpen).Select(m => new WorkOrdersGridViewModel
                                .Where(w => w.Closed == null && w.Approved != null).Select(m => new WorkOrdersGridViewModel
                                {
                                    //Changed by David since all but ConsumerName can never be null.
                                    //DepartmentAreaName = m.Department != null ? m.Department.AreaName : string.Empty,
                                    //DepartmentName = m.Department != null ? m.Department.Name : string.Empty,
                                    DepartmentAreaName = m.Department.AreaName,
                                    DepartmentName = m.Department.Name,
                                    Reported = m.Reported,
                                    Details = m.Details,
                                    Priority = m.Priority,
                                    ConsumerName = m.Consumer != null ? m.Consumer.Name : string.Empty,
                                    Id = m.Id
                                });


            // Get closed workorders

            // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
            //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.



            return this.Json(model.ToDataSourceResult(request));
        }

        public JsonResult GetClosed([DataSourceRequest]DataSourceRequest request)
        {

            // Get closed workorders

            // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
            //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.
            var model = _db.Workorders
                                    .OrderBy(w => w.Department.AreaName)
                                    .ThenBy(w => w.Department.Name)
                                    .Where(w => w.Closed != null).Select(m => new WorkOrdersGridViewModel
                                    {
                                        DepartmentAreaName = m.Department != null ? m.Department.AreaName : string.Empty,
                                        DepartmentName = m.Department != null ? m.Department.Name : string.Empty,
                                        Reported = m.Reported,
                                        Details = m.Details,
                                        Priority = m.Priority,
                                        ConsumerName = m.Consumer != null ? m.Consumer.Name : string.Empty,
                                        Closed = m.Closed,
                                        Closer = m.Closer,
                                        Id = m.Id
                                    });


            return this.Json(model.ToDataSourceResult(request));
        }

        public JsonResult GetRejected([DataSourceRequest]DataSourceRequest request)
        {

            // Get Rejected workorders

            // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
            //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.
            var model = _db.Workorders
                                    .OrderBy(w => w.Department.AreaName)
                                    .ThenBy(w => w.Department.Name)
                                    .Where(w => w.Rejected != null).Select(m => new WorkOrdersGridViewModel
                                    {
                                        DepartmentAreaName = m.Department != null ? m.Department.AreaName : string.Empty,
                                        DepartmentName = m.Department != null ? m.Department.Name : string.Empty,
                                        Reported = m.Reported,
                                        Details = m.Details,
                                        Priority = m.Priority,
                                        ConsumerName = m.Consumer != null ? m.Consumer.Name : string.Empty,
                                        Rejected = m.Rejected,
                                        Rejector = m.Rejector,
                                        Id = m.Id
                                    });


            return this.Json(model.ToDataSourceResult(request));
        }

        public JsonResult GetMobileGridData([DataSourceRequest]DataSourceRequest request, string workOrderType, string workOrderDueType)
        {
            // JavaScriptSerializer class used by the Json method cannot serialize object graphs which contain circular references (refer to each other). 
            //The best solution is to use View Model objects and avoid the serializing the properties which create the circular reference.
            var model = _db.Workorders.ToList()
                                    .OrderBy(w => w.Department.AreaName)
                                    .ThenBy(w => w.Department.Name)
                                    .Select(m => new WorkOrdersGridViewModel
                                    {
                                        DepartmentAreaName = m.Department.AreaName,
                                        DepartmentName = m.Department.Name,
                                        Reported = m.Reported,
                                        Details = m.Details,
                                        Priority = m.Priority,
                                        ConsumerName = m.Consumer != null ? m.Consumer.Name : string.Empty,
                                        Closed = m.Closed,
                                        Closer = m.Closer,
                                        Id = m.Id,
                                        Resolution = m.Resolution,
                                        Estimate = m.Estimate,
                                        //Approved = m.Approved ?? false
                                        Approved=m.Approved
                                    });

            //filter by workorder type
            if (workOrderType != null)
            {
                if (workOrderType.Equals("Closed"))
                {
                    model = model.Where(x => x.Closed != null);
                }
                else if (workOrderType.Equals("Open"))
                {
                    //model = model.Where(x => x.Closed == null && x.Approved == true);
                    model = model.Where(x => x.Closed == null && x.Approved != null);
                }
                else if (workOrderType.Equals("Awaiting Approval"))
                {
                    //model = model.Where(x => x.Closed == null && x.Approved == false);
                    model = model.Where(x => x.Closed == null && x.Approved == null && x.Rejected == null);
                }
            }

            //filter by due (due or overdue)
            if (workOrderDueType != null)
            {
                if (workOrderDueType.Equals("Overdue"))
                {
                    model = model.Where(x => x.Overdue);
                }
                else if (workOrderDueType.Equals("Due"))
                {
                    model = model.Where(x => x.Overdue == false);
                }
            }

            var data = model;
            var requestPageSize = request.PageSize;
            request.PageSize = 0;

            //get total records
            var totalRecords = data.ToDataSourceResult(request).Data.AsQueryable().Count();
            request.PageSize = requestPageSize;

            return this.Json(new { Data = model.ToDataSourceResult(request), TotalRecords = totalRecords });
        }

        public ActionResult Index()
        {
            var selectedCompany = CompanyCookie.SelectedCompany;

            WorkOrdersGridViewModel model = new WorkOrdersGridViewModel();

            //fill dropdowns data. required for mobile view
            model.Departments = GetMobileFilterDepartmentsSelectList();
            model.Areas = GetMobileFilterAreasSelectList();
            model.Closers = GetMobileFilterClosersSelectList();
            model.Priorities = GetPrioritiesSelectList(null);

            model.Companies = Helpers.UserFunctions.GetCompaniesSelectList(selectedCompany);
            model.SelectedCompany = selectedCompany;
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult SelectCompany(WorkOrdersGridViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                CompanyCookie.SelectedCompany = model.SelectedCompany;
            }

            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                return RedirectToAction("Index");

            }
        }

        public ActionResult Mobile()
        {
            WorkOrdersGridViewModel model = new WorkOrdersGridViewModel();

            //fill dropdowns data. required for mobile view
            model.Departments = GetMobileFilterDepartmentsSelectList();
            model.Areas = GetMobileFilterAreasSelectList();
            model.Closers = GetMobileFilterClosersSelectList();
            model.Priorities = GetPrioritiesSelectList(null);

            return View(model);
        }

        public ActionResult LoadWorkordersGridPartial(string type, bool isApproved)
        {
            ViewBag.isApproved = isApproved;
            ViewBag.WorkorderType = type;
            return PartialView("_WorkorderPartial");
        }

        public JsonResult GetFilteredOpenIssues([DataSourceRequest]DataSourceRequest request)
        {
            //var model = _db.Workorders.Where(x => x.Closed == null && x.Approved == true).Select(m => new WorkOrdersGridViewModel
            var model = _db.Workorders.Where(x => x.Closed == null).Select(m => new WorkOrdersGridViewModel
            {
                DepartmentAreaName = m.Department != null ? m.Department.AreaName : string.Empty,
                DepartmentName = m.Department != null ? m.Department.Name : string.Empty,
                Reported = m.Reported,
                Details = m.Details,
                Priority = m.Priority,
                ConsumerName = m.Consumer != null ? m.Consumer.Name : string.Empty,
                Id = m.Id

            });
            var totalOpenIssues = model.ToDataSourceResult(request).Data.AsQueryable().Count();

            return Json(new { issues = totalOpenIssues });
        }

        public ActionResult GetOpenIssuesCount()
        {
            return Content(_db.Workorders.Count(x => x.Closed == null).ToString());
            // return Content(_db.Workorders.Count(x => x.Closed == null && x.Approved == true).ToString());
        }

        public ActionResult Resolved()
        {
            var model = _db.Workorders.ToList()
              .OrderBy(w => w.Department.AreaName)
              .ThenBy(w => w.Department.Name)
              .Where(w => w.Closed != null)
              .Select(m => new WorkOrdersGridViewModel
              {
                  DepartmentAreaName = m.Department != null ? m.Department.AreaName : string.Empty,
                  DepartmentName = m.Department != null ? m.Department.Name : string.Empty,
                  Reported = m.Reported,
                  Details = m.Details,
                  Priority = m.Priority,
                  ConsumerName = m.Consumer != null ? m.Consumer.Name : string.Empty,
                  Id = m.Id

              });

            return View(model);
        }

        // GET: /Workorders/Edit/1
        [HttpGet]
        [CustomAuthorize(AccessType = AccessType.Editors)]
        public ActionResult Edit(int id)
        {
            var selectedCompany = CompanyCookie.SelectedCompany;

            if (string.IsNullOrEmpty(selectedCompany))
            {
                return RedirectToAction("Index");
            }
            var model = _db.Workorders.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var viewModel = new WorkorderEditViewModel();

            viewModel.Id = id;
            viewModel.DepartmentId = model.DepartmentId;
            viewModel.DepartmentAreaId = model.Department.AreaID;
            viewModel.Priority = model.Priority;
            viewModel.Reported = model.Reported;
            viewModel.Details = model.Details;
            viewModel.Reporter = model.Reporter;
            viewModel.ConsumerId = model.ConsumerId;
            viewModel.Estimate = model.Estimate;
            viewModel.PoNumber = model.PoNumber;
            viewModel.Closed = model.Closed;
            viewModel.Closer = model.Closer;
            viewModel.Resolution = model.Resolution;
            //viewModel.Approved = model.Approved ?? false;
            viewModel.Approved = model.Approved;
            viewModel.PersonServed = model.PersonServed;
            //fill dropdowns data
            viewModel.Departments = GetDepartmentsSelectList(viewModel.DepartmentId);
            viewModel.Consumers = GetConsumersSelectList(viewModel.ConsumerId);
            viewModel.Priorities = GetPrioritiesSelectList(viewModel.Priority);
            viewModel.Areas = GetAreasSelectList(viewModel.DepartmentAreaId);

            return View(viewModel);

        }

        [HttpPost]
        [CustomAuthorize(AccessType = AccessType.Editors)]
        public ActionResult Edit(WorkorderEditViewModel viewModel)
        {
            var workorder = _db.Workorders.Find(viewModel.Id);

            if (ModelState.IsValid)
            {

                //validate issue closed date 
                if (viewModel.Closed.HasValue && (viewModel.Closed.Value.Date < viewModel.Reported.Date || viewModel.Closed.Value.Date > DateTime.Today.Date))
                {
                    ModelState.AddModelError("Closed", "Invalid Closed date.");
                }
                else
                {
                    //update only edit only fields
                    workorder.DepartmentId = viewModel.DepartmentId;
                    workorder.Priority = viewModel.Priority;
                    workorder.Details = viewModel.Details;
                    workorder.ConsumerId = viewModel.ConsumerId;
                    workorder.Estimate = viewModel.Estimate;
                    workorder.PoNumber = viewModel.PoNumber;
                    workorder.Closed = viewModel.Closed;
                    workorder.Closer = viewModel.Closer;
                    workorder.Resolution = viewModel.Resolution;
                    workorder.PersonServed = viewModel.PersonServed;

                    _db.Entry(workorder).State = EntityState.Modified;

                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Workorder has been updated successfully";

                    return RedirectToAction("Index");
                }
            }

            //fill dropdowns data
            viewModel.Departments = GetDepartmentsSelectList(viewModel.DepartmentId);
            viewModel.Consumers = GetConsumersSelectList(viewModel.ConsumerId);
            viewModel.Priorities = GetPrioritiesSelectList(viewModel.Priority);

            //add disabled fields to view model
            viewModel.Reported = workorder.Reported;
            viewModel.Reporter = workorder.Reporter;

            return View(viewModel);

        }
        //
        // GET: /Workorders/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new WorkorderCreateViewModel();
                      
            model.Reporter =  User.Identity.Name;
            //fill dropdowns data
            model.Departments = GetDepartmentsSelectList(null);
            model.Consumers = GetConsumersSelectList(null);
            model.Priorities = GetPrioritiesSelectList(null);
            model.Areas = GetAreasSelectList();

            return View(model);
        }
        // POST: /Workorders/Create

        [HttpPost]
        public ActionResult Create(WorkorderCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string reporter = string.IsNullOrEmpty(User.Identity.Name) == false ? User.Identity.Name : "Guest";

                string supervisorName = null, supervisorEmail = null; string alertRecipientEmails = null;

                //get department
                var department = _db.Departments.FirstOrDefault(x => x.Id == viewModel.DepartmentId);

                //get department supervisor detail by department id
                var departmentSupervisorEmail = _db.DepartmentSupervisorEmails.FirstOrDefault(x => x.DepartmentId == viewModel.DepartmentId);

                //get all alert receipients
                var alertrecipients = _db.AlertRecipients.Where(x => x.AreaId == viewModel.DepartmentAreaId).ToList();
                alertRecipientEmails = string.Join(";", alertrecipients.Select(x => x.Emails).Distinct());

                if (departmentSupervisorEmail != null)
                {
                    supervisorName = departmentSupervisorEmail.SupervisorName;
                    supervisorEmail = departmentSupervisorEmail.EmailAddress;
                    //supervisorName = "David Eaton";
                    //supervisorEmail = "davidzaeaton@outlook.com";
                }

                Workorder workorder = new Workorder()
                {
                    DepartmentId = viewModel.DepartmentId,
                    Priority = viewModel.Priority,
                    Reporter = reporter,
                    Reported = viewModel.Reported,
                    Details = viewModel.Details,
                    //ConsumerId = viewModel.ConsumerId,
                    //Approver = supervisorName,
                    //Approved = DateTime.Now,
                    PersonServed = viewModel.PersonServed
                };

                _db.Workorders.Add(workorder);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Workorder has been created successfully and is awaiting supervisor approval.";

                //if we don't have supervisor email
                if (string.IsNullOrEmpty(supervisorEmail) && string.IsNullOrEmpty(alertRecipientEmails))
                {
                    //use default email from web.config
                    supervisorEmail = ConfigurationManager.AppSettings["DepartmentSupervisorEmailDefault"];
                }

                //if still  supervisor email is null, then don't send email
                if (string.IsNullOrEmpty(supervisorEmail) == false && string.IsNullOrEmpty(alertRecipientEmails) == false)
                {
                    var message = "New Workorder # " + workorder.Id + " created for " + department.DepartmentFullName;
                    message += "<br />Priority : " + workorder.Priority.ToString() + "<br />";
                    message += "Detail : " + workorder.Details + "<br/>";

                    Helpers.Email.SendEmail(supervisorEmail, "New Workorder for " + department.DepartmentFullName, message, message, null, null, alertRecipientEmails);
                }

                return RedirectToAction("Index");
            }

            //fill dropdowns data
            viewModel.Departments = GetDepartmentsSelectList(viewModel.DepartmentId);
            viewModel.Consumers = GetConsumersSelectList(viewModel.ConsumerId);
            viewModel.Priorities = GetPrioritiesSelectList(viewModel.Priority);


            return View(viewModel);
        }


        // GET: /Workorders/Detail/1
        [HttpGet]
        public ActionResult Detail(int id)
        {

            var model = _db.Workorders.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var viewModel = new WorkorderDetailViewModel();

            viewModel.Id = id;
            viewModel.DepartmentId = model.DepartmentId;
            viewModel.DepartmentAreaId = model.Department.AreaID;
            viewModel.Priority = model.Priority;
            viewModel.Reported = model.Reported;
            viewModel.Details = model.Details;
            viewModel.Reporter = model.Reporter;
            viewModel.ConsumerId = model.ConsumerId;
            viewModel.PersonServed = model.PersonServed;
            //fill dropdowns data
            viewModel.Departments = GetDepartmentsSelectList(viewModel.DepartmentId);
            viewModel.Consumers = GetConsumersSelectList(viewModel.ConsumerId);
            viewModel.Priorities = GetPrioritiesSelectList(viewModel.Priority);
            viewModel.Areas = GetAreasSelectList(viewModel.DepartmentAreaId);

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(AccessType = AccessType.Admins)]
        public JsonResult DeleteConfirmed(int id = 0)
        {
            Workorder workorder = _db.Workorders.FirstOrDefault(x => x.Id == id);
            if (workorder == null)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                return Json(new { success = false });
            }

            _db.Workorders.Remove(workorder);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Workorder has been deleted successfully.";

            return Json(new { success = true });
        }

        [HttpPost]
        [CustomAuthorize(AccessType = AccessType.Editors)]
        public JsonResult Approve(int id)
        {
            Workorder workorder = _db.Workorders.FirstOrDefault(x => x.Id == id);
            if (workorder == null)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                return Json(new { success = false });
            }

            workorder.Approved = DateTime.Now;
            workorder.Approver = User.Identity.Name;
            workorder.Rejected = null;
            workorder.Rejector = null;

            _db.SaveChanges();

            TempData["SuccessMessage"] = "Workorder has been Approved and moved to Open Workorders.";

            return Json(new { success = true });
        }

        [HttpPost]
        [CustomAuthorize(AccessType = AccessType.Editors)]
        public JsonResult Reject(int id, string reason)
        {
            Workorder workorder = _db.Workorders.FirstOrDefault(x => x.Id == id);
            if (workorder == null)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                return Json(new { success = false });
            }

            workorder.Rejection = reason;
            workorder.Rejected = DateTime.Now;
            workorder.Rejector = User.Identity.Name;
            workorder.Approved =null;
            workorder.Approver = null;

            _db.SaveChanges();

            TempData["SuccessMessage"] = "Workorder has been Rejected.";

            return Json(new { success = true });
        }


        public ActionResult GetAreaDropdownItems()
        {
            var areas = _db.Areas.Select(x => x.Name).OrderBy(x => x).ToList();
            return Json(areas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAreaDepartmnentList(int? id)
        {
            var departments = _db.Departments.Where(d => (d.Active == true) && d.AreaID == id).OrderBy(d => d.Name)
                               .Select(x => new { Text = x.Name, Value = x.Id }).ToList();

            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOpenDepartmentDropdownItems()
        {
            var departments = _db.Workorders.Where(w => w.Closed == null && w.Department.Active == true).Select(w => w.Department.Name).OrderBy(x => x).Distinct();

            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        private SelectList GetAreasSelectList(int? areadId = null)
        {
            return new SelectList(_db.Areas
                               .ToList()
                               .Where(d => (d.Active == true))
                               .OrderBy(d => d.Name), "Id", "Name", areadId);
        }

        public ActionResult GetClosedDepartmentDropdownItems()
        {
            var departments = _db.Workorders.Where(w => w.Closed != null && w.Department.Active == true).Select(w => w.Department.Name).OrderBy(x => x).Distinct();

            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPriorityDropdownItems()
        {
            var priorities = Enum.GetNames(typeof(Priority));

            return Json(priorities, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetConsumerDropdownItems()
        {
            var consumers = _db.Consumers.Where(w => w.Active).Select(w => w.Name).OrderBy(x => x);

            return Json(consumers, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }

        public Action Help()
        {
            return null;
        }

        public ActionResult About()
        {
            var applicationName = Settings.ApplicationName;
            var applicationDescription = Settings.ApplicationDescription;
            var currentYear = DateTime.Now.Year.ToString();

            var model = new AboutViewModel();
            model.ApplicationName = applicationName;
            model.ApplicationDescription = applicationDescription;
            model.CurrentYear = currentYear;
            model.UrlReferrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index");
            return View(model);
        }


        #region Helpers
        private SelectList GetDepartmentsSelectList(int? departmentId)
        {
            return new SelectList(_db.Departments
                               .ToList()
                               .Where(d => (d.Active == true))
                               .OrderBy(d => d.Name), "Id", "DepartmentFullName", departmentId);
        }

        private SelectList GetConsumersSelectList(int? consumerId)
        {
            return new SelectList(_db.Consumers.ToList()
                                        .OrderBy(c => c.Name), "Id", "Name", consumerId);
        }       

        private SelectList GetPrioritiesSelectList(Priority? priority)
        {
            IEnumerable typeOfPriority =
                from s in Enum.GetNames(typeof(Priority))
                select new { ID = s, Name = s };

            return new SelectList(typeOfPriority, "Id", "Name", priority);
        }

        #region Mobile Grid

        private SelectList GetMobileFilterDepartmentsSelectList()
        {
            return new SelectList(_db.Departments
                               .ToList()
                               .Where(d => (d.Active == true))
                               .OrderBy(d => d.Name), "Id", "Name");
        }

        private SelectList GetMobileFilterAreasSelectList()
        {
            return new SelectList(_db.Areas
                               .ToList()
                               .Where(d => (d.Active == true))
                               .OrderBy(d => d.Name), "Id", "Name");
        }
        private SelectList GetMobileFilterClosersSelectList()
        {
            return new SelectList(_db.Workorders.Select(x => new { Closer = x.Closer })
                                .Where(x => !string.IsNullOrEmpty(x.Closer)).Distinct()
                               .ToList()
                               .OrderBy(d => d.Closer), "Closer", "Closer");
        }

        //send the report

        #endregion

        #endregion
    }
}
