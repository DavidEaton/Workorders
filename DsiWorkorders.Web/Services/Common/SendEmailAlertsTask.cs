using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using DsiWorkorders.Web.Services.Tasks;
using DsiWorkorders.Web.Helpers;

namespace DsiWorkorders.Web.Services.Common
{
    public class SendReportEmailTask : ITask
    {

        public void Execute()
        {
            //get current time by zone 
            DateTime currentDateTime = DateTime.UtcNow.ToCentralTime();

            if (currentDateTime.DayOfWeek == DayOfWeek.Monday && currentDateTime.Hour == 3 && currentDateTime.Minute == 0 && currentDateTime.Second > 0)
            {
                SendAreaReport();
            }
        }

        private static void SendAreaReport()
        {
            AppDbContext _db = new AppDbContext();

            //get all areas
            var areas = _db.Areas.ToList();

            foreach (var area in areas)
            {
                //get area reportrecpients
                var reportRecipients = _db.ReportRecipients.Where(x => x.AreaId == area.Id).ToList();
                if (reportRecipients.Any())
                {
                    var toBeSentEmails = string.Join(";", _db.ReportRecipients.Where(x => x.AreaId == area.Id).Select(x => x.Emails).ToList());
                    string emailBodyText = ReportEmailMessage.GetReportEmailMessage(area.Name, area.Id, _db);

                    //send email 
                    if (emailBodyText != null)
                    {
                        Email.SendEmail(toBeSentEmails, ConfigurationManager.AppSettings["CompanyAbbr"] + " Weekly Maintenance Workorders Report", emailBodyText, emailBodyText, null, null);
                    }
                }
            }
        }
    }
    }
