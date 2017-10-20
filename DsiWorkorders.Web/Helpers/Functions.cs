using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DsiWorkorders.Data.Enums;
using System.Web.Mvc;
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

        public static bool IsAdminOrEditor()
        {
            return IsEditor() || IsAdmin();
        }

        //User is valid for Dsi
        public static bool IsValidUser()
        {
            return IsViewer() || IsEditor() || IsAdmin();
        }

        // If user is Viewer
        public static bool IsViewer()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsViewer" && bool.Parse(x.Value));
        }

        // If user is Editor
        public static bool IsEditor()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsEditor" && bool.Parse(x.Value));
        }

        // If user is Admin
        public static bool IsAdmin()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            return identity.Claims.Any(x => x.Type == "IsAdmin" && bool.Parse(x.Value));
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