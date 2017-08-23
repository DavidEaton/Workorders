using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DsiWorkorders.Data.Enums;
using System.Web.Mvc;
using DsiShifts.Data.Enums;
using DsiWorkorders.Web.Services.Tasks;

namespace DsiWorkorders.Web.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// get days for priortity
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static int GetDays(this Priority priority)
        {
            switch (priority)
            {
                case Priority.High:
                    return int.Parse(WebConfigurationManager.AppSettings["PriorityHigh"]);
                case Priority.Normal:
                    return int.Parse(WebConfigurationManager.AppSettings["PriorityNormal"]);
                case Priority.Low:
                    return int.Parse(WebConfigurationManager.AppSettings["PriorityLow"]);
                default:
                    return int.Parse(WebConfigurationManager.AppSettings["PriorityLow"]);
            }
        }

    }

    public class UserFunctions
    {

        // User is Valid
        public static bool IsValidUser()
        {
            return IsCSIValidUser() || IsDSIValidUser();
        }

        public static bool IsAdminOrEditor()
        {
            return IsEditor() || IsAdmin();
        }

        // User is Valid for Csi
        public static bool IsCSIValidUser()
        {
            return IsCsiViewer() || IsCsiEditor() || IsCsiAdmin();
        }

        //User is valid for Dsi
        public static bool IsDSIValidUser()
        {
            return IsDsiViewer() || IsDsiEditor() || IsDsiAdmin();
        }
        // If user is Csi Viewer
        public static bool IsCsiViewer()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsCsiViewer" && bool.Parse(x.Value));
        }

        // If user is Csi Editor
        public static bool IsCsiEditor()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsCsiEditor" && bool.Parse(x.Value));
        }

        // If user is Csi Admin
        public static bool IsCsiAdmin()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsCsiAdmin" && bool.Parse(x.Value));
        }

        // If user is Dsi Viewer
        public static bool IsDsiViewer()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsDsiViewer" && bool.Parse(x.Value));
        }

        // If user is Dsi Editor
        public static bool IsDsiEditor()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsDsiEditor" && bool.Parse(x.Value));
        }

        // If user is Dsi Admin
        public static bool IsDsiAdmin()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsDsiAdmin" && bool.Parse(x.Value));
        }

        // If user is Viewer Admin Editor of Selected Company than allow 
        public static bool IsAllowedAccessToCompany(bool isCsiViewer, bool isCsiEditor, bool isCsiAdmin, bool isDsiViewer, bool isDsiEditor, bool isDsiAdmin)
        {
            var selectedCompany = CompanyCookie.SelectedCompany;

            if (selectedCompany == CompanyEnum.CSI.ToString() && isCsiViewer)
            {
                return true;
            }
            else if (selectedCompany == CompanyEnum.CSI.ToString() && isCsiEditor)
            {
                return true;
            }
            else if (selectedCompany == CompanyEnum.CSI.ToString() && isCsiAdmin)
            {
                return true;
            }
            else if (selectedCompany == CompanyEnum.DSI.ToString() && isDsiViewer)
            {
                return true;
            }
            else if (selectedCompany == CompanyEnum.DSI.ToString() && isDsiEditor)
            {
                return true;
            }
            else if (selectedCompany == CompanyEnum.DSI.ToString() && isDsiAdmin)
            {
                return true;
            }

            return false;
        }

        public static bool IsAdmin()
        {
            var selectedCompany = CompanyCookie.SelectedCompany;
            if (selectedCompany == CompanyEnum.CSI.ToString())
            {
                return IsCsiAdmin();
            }
            else if (selectedCompany == CompanyEnum.DSI.ToString())
            {
                return IsDsiAdmin();
            }

            return false;
        }

        public static bool IsEditor()
        {
            var selectedCompany = CompanyCookie.SelectedCompany;
            if (selectedCompany == CompanyEnum.CSI.ToString())
            {
                return IsCsiEditor();
            }
            else if (selectedCompany == CompanyEnum.DSI.ToString())
            {
                return IsDsiEditor();
            }

            return false;
        }

        public static bool IsViewer()
        {
            var selectedCompany = CompanyCookie.SelectedCompany;
            if (selectedCompany == CompanyEnum.CSI.ToString())
            {
                return IsCsiViewer();
            }
            else if (selectedCompany == CompanyEnum.DSI.ToString())
            {
                return IsDsiViewer();
            }

            return false;
        }

        public static SelectList GetCompaniesSelectList(string company)
        {
            //create a list of strings that will have logged in users Roles

            var list = new List<string>();

            if (Helpers.UserFunctions.IsCSIValidUser())
            {

                list.Add(CompanyEnum.CSI.ToString());
            }

            if (Helpers.UserFunctions.IsDSIValidUser())
            {
                list.Add(CompanyEnum.DSI.ToString());
            }

            var companies = list.Select(x => new { ID = x, Name = x });

            return new SelectList(companies, "Name", "Name", company);
        }

        public static SelectList GetAreasSelectList(AppDbContext _db, int? areaId = null)
        {
            var areas = _db.Areas.Where(x => x.Active).ToList();

            return new SelectList(areas, "Id", "Name", areaId);

        }

        /// <summary>
        /// get all active schedule
        /// </summary>
        /// <returns></returns>
        public static List<ScheduleTask> GetReportTasks()
        {
            using (var db = new AppDbContext())
            {
                var alertSchedules = new List<ScheduleTask>();

                alertSchedules.Add(new ScheduleTask
                {
                    Type = "DsiWorkorders.Web.Services.Common.SendReportEmailTask, DsiWorkorders.Web",
                    Name = "Send Workorder Report",
                    Seconds = 60, //every five minute
                    Enabled = true,
                    StopOnError = true,
                });

                return alertSchedules;
            }
        }
    }

}