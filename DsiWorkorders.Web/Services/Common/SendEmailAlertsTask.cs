using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using DsiWorkorders.Web.Services.Tasks;
using System.Data.Entity;
using DsiWorkorders.Web;
using DsiWorkorders.Web.Helpers;
using DsiShifts.Data.Enums;
using DsiWorkorders.Data;

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
                var companies = new List<CompanyEnum>();
                companies.Add(CompanyEnum.CSI);
                companies.Add(CompanyEnum.DSI);

                foreach (var company in companies)
                {
                    string companyConnectionString = null;

                    if (ConfigurationManager.AppSettings[company.ToString()] != null)
                    {
                        companyConnectionString = ConfigurationManager.AppSettings[company.ToString()];
                    }

                    AppDbContext _db = new AppDbContext(companyConnectionString);

                    //get al areas
                    var areas = _db.Areas.ToList();

                    foreach (var area in areas)
                    {
                        //get reportrecpients according area
                        var reportRecipients = _db.ReportRecipients.Where(x => x.AreaId == area.Id).ToList();
                        if (reportRecipients.Any())
                        {
                            var toBeSentEmails = string.Join(";", _db.ReportRecipients.Where(x => x.AreaId == area.Id).Select(x => x.Emails).ToList());
                            string emailBodyText = ReportEmailMessage.GetReportEmailMessage(company, area.Name, area.Id, _db);

                            //send email 
                            if (emailBodyText != null)
                            {
                                Email.SendEmail(toBeSentEmails, company + " Weekly Maintenance Work Orders Report", emailBodyText, emailBodyText, null, null);
                            }
                        }
                    }
                }
            }
        }
    }
}