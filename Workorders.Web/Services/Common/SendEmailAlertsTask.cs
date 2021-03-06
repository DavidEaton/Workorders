﻿using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Workorders.Web.Services.Tasks;
using Workorders.Web.Helpers;
using Workorders.Data.Enums;

namespace Workorders.Web.Services.Common
{
    public class SendReportEmailTask : ITask
    {

        public void Execute()
        {
            //get current time by zone 
            DateTime currentDateTime = DateTime.UtcNow.ToCentralTime();

            if (currentDateTime.DayOfWeek == DayOfWeek.Monday && currentDateTime.Hour == 3 && currentDateTime.Minute == 0 && currentDateTime.Second > 0)
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
