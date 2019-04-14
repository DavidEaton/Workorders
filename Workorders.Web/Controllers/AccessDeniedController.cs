using Workorders.Web.ViewModels;
using System.Web.Mvc;
using Workorders.Web.Helpers;

namespace Workorders.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        // GET: AccessDenied
        public ActionResult Index()
        {
            //read selected company from cookie
            var selectedCompany = CompanyCookie.SelectedCompany;

            var model = new WorkOrdersGridViewModel
            {
                Companies = UserFunctions.GetCompaniesSelectList(selectedCompany),
                SelectedCompany = selectedCompany
            };

            return View(model);
        }
    }
}