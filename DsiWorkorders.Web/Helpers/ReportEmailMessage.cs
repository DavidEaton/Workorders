using System;
using System.Collections.Generic;
using System.Linq;
using DsiWorkorders.Web.ViewModels;
using System.Configuration;

namespace DsiWorkorders.Web.Helpers
{
    public class ReportEmailMessage
    {
        public static string GetReportEmailMessage(string areaName, int areaId, AppDbContext _db)
        {

            var today = System.DateTime.UtcNow.ToCentralTime();
            var yesterday = today.AddDays(-1);
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek).Date;
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);

            var openedWorkOrderModels = new List<WorkOrdersGridViewModel>();
            var closedWorkOrderModels = new List<WorkOrdersGridViewModel>();
            //Add Awaiting Approval workorders

            var openedWorkOrders = _db.Workorders.Where(x => x.Reported >= lastWeekStart && x.Reported <= lastWeekEnd && x.Department.AreaID == areaId).ToList();

            foreach (var workorder in openedWorkOrders)
            {
                var model = new WorkOrdersGridViewModel();
                model.DepartmentName = workorder.Department != null ? workorder.Department.Name : null;
                model.DepartmentAreaName = workorder.Department != null ? workorder.Department.AreaName : null;
                model.Reported = workorder.Reported;
                model.Priority = workorder.Priority;
                model.Details = workorder.Details;
                model.Closed = workorder.Closed;
                model.Id = workorder.Id;
                openedWorkOrderModels.Add(model);
            }

            var message = @"<h2>" + ConfigurationManager.AppSettings["CompanyAbbr"] + @" Weekly Maintenance Workorders Report</h2>
                                    <h2>" + areaName + @"</h2>
                                    <h5>Report created " + @DateTime.UtcNow.ToCentralTime() + @"</h5><br />
                                      <h4 ><i>Workorders Opened over the past week</i> </h4> ";

            if (openedWorkOrderModels.Any() == false)
            {
                message += "No Workorders were opened for the specified reporting period.";
            }
            else
            {
                message += @" <table style=""border:1px"">
                                       <tr style=""text-align:left"">
                                            <th style=""border-bottom:1px; border-right: 1px;"">Department</th> 
                                            <th style=""border-bottom:1px; border-right: 1px;"">Reported </th>   
                                             <th style=""border-bottom:1px; border-right: 1px;"">Details</th>
                                            <th style=""border-bottom:1px; border-right: 1px;"">Priorty</th>
                                            <th style=""border-bottom:1px; border-right: 1px;"">Due</th>
                                            <th style=""border-bottom:1px; border-right: 1px;"">Days Overdue</th>
                                           <th style=""border-bottom:1px"">Days Open </th>
                                        </tr>";

                foreach (var workorder in openedWorkOrderModels)
                {
                    message += GetWorkorderEmailRow(workorder);
                }

                message += @"</table>";
            }


            message += @"<h4><i>Workorders closed over the past week</i> </h4>";
            var closedWorkOrders = _db.Workorders.Where(x => x.Closed != null && x.Closed >= lastWeekStart && x.Closed <= lastWeekEnd && x.Department.AreaID == areaId).ToList();
            foreach (var workorder in closedWorkOrders)
            {
                var model = new WorkOrdersGridViewModel();
                model.DepartmentName = workorder.Department != null ? workorder.Department.Name : null;
                model.DepartmentAreaName = workorder.Department != null ? workorder.Department.AreaName : null;
                model.Reported = workorder.Reported;
                model.Priority = workorder.Priority;
                model.Details = workorder.Details;
                model.Closed = workorder.Closed;
                model.Id = workorder.Id;
                closedWorkOrderModels.Add(model);
            }

            if (closedWorkOrderModels.Any() == false)
            {
                message += "No Workorders were closed during the specified reporting period.";
            }
            else
            {
                message += @" <table style=""border:1px"">
                                       <tr style=""text-align:left"">
                                            <th style=""border-bottom:1px; border-right: 1px;"">Department</th> 
                                            <th style=""border-bottom:1px; border-right: 1px;"">Reported </th>   
                                             <th style=""border-bottom:1px; border-right: 1px;"">Details</th>
                                            <th style=""border-bottom:1px; border-right: 1px;"">Priorty</th>
                                            <th style=""border-bottom:1px; border-right: 1px;"">Due</th>
                                            <th style=""border-bottom:1px; border-right: 1px;"">Days Overdue</th>
                                           <th style=""border-bottom:1px"">Days Open </th>
                                        </tr>";

                foreach (var workorder in closedWorkOrderModels)
                {
                    message += GetWorkorderEmailRow(workorder);
                }

                message += @"</table>";
            }

            return message;
        }


        public static string GetWorkorderEmailRow(WorkOrdersGridViewModel workorder)
        {
            return @"<tr style=""text-align:left"">
                        <td style=""border-right: 1px;""> " + workorder.DepartmentName + "&nbsp" + @" </td>
                        <td style=""border-right: 1px;""> " + workorder.Reported.ToShortDateString() + "&nbsp" + @"  </td>
                        <td style=""border-right: 1px;""> " + workorder.Details + "&nbsp" + @"  </td>
                        <td style=""border-right: 1px;""> " + workorder.Priority + "&nbsp" + @"  </td>
                         <td style=""border-right: 1px;""> " + workorder.Due.ToShortDateString() + "&nbsp" + @"  </td>
                        <td style=""border-right: 1px;""> " + workorder.DaysOverdue + "&nbsp" + @"  </td>
                        <td> " + workorder.DaysOpen + "&nbsp" + @" </td>
                </tr >";

        }
    }
}