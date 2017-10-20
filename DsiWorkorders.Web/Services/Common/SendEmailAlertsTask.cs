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

                    AppDbContext _db = new AppDbContext();

                    //get al areas
                    var areas = _db.Areas.ToList();

                    foreach (var area in areas)
                    {
                        //get reportrecpients according area
                        var reportRecipients = _db.ReportRecipients.Where(x => x.AreaId == area.Id).ToList();
                        if (reportRecipients.Any())
                        {
                            var toBeSentEmails = string.Join(";", _db.ReportRecipients.Where(x => x.AreaId == area.Id).Select(x => x.Emails).ToList());
                            string emailBodyText = ReportEmailMessage.GetReportEmailMessage(area.Name, area.Id, _db);

                            //send email 
                            if (emailBodyText != null)
                            {
                                Email.SendEmail(toBeSentEmails, ConfigurationManager.AppSettings["CompanyName"] + " Weekly Maintenance Work Orders Report", emailBodyText, emailBodyText, null, null);
                            }
                        }
                    }
                }
            }
        }
    }
