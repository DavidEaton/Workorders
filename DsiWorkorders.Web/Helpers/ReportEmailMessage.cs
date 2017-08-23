using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DsiShifts.Data.Enums;
using DsiWorkorders.Data.Enums;
using DsiWorkorders.Web.ViewModels;
using DsiWorkorders.Data;

namespace DsiWorkorders.Web.Helpers
{
    public class ReportEmailMessage
    {
        public static string GetReportEmailMessage(CompanyEnum company, string areaName, int areaId, AppDbContext _db)
        {

            var today = System.DateTime.UtcNow.ToCentralTime();
            var yesterday = today.AddDays(-1);
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek).Date;
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);

            var openedWorkOrderModels = new List<WorkOrdersGridViewModel>();
            var closedWorkOrderModels = new List<WorkOrdersGridViewModel>();

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

            var message = @"<h2>" + company.ToString() + @" Weekly Maintenance Work Orders Report</h2>
                                    <h2>" + areaName + @"</h2>
                                    <h5>Report created " + @DateTime.UtcNow.ToCentralTime() + @"</h5><br />
                                      <h4 ><i>Workorders Opened over the past week</i> </h4> ";

            if (openedWorkOrderModels.Any() == false)
            {
                message += "No Workorders were opened for the specified reporting period.";
            }
            else
            {
                message += @" <table style=""border:1px solid #000"">
                                       <tr style=""text-align:left"">
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Departments</th> 
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Reported </th>   
                                             <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Details</th>
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Priorty</th>
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Due</th>
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Days Overdue</th>
                                           <th style=""border-bottom:1px solid #000"">Days Open </th>
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
                message += @" <table style=""border:1px solid #000"">
                                       <tr style=""text-align:left"">
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Departments</th> 
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Reported </th>   
                                             <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Details</th>
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Priorty</th>
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Due</th>
                                            <th style=""border-bottom:1px solid #000; border-right: 1px solid #000;"">Days Overdue</th>
                                           <th style=""border-bottom:1px solid #000"">Days Open </th>
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
                        <td style=""border-right: 1px solid #000;""> " + workorder.DepartmentName + @" </td>
                        <td style=""border-right: 1px solid #000;""> " + workorder.Reported.ToShortDateString() + @"  </td>
                        <td style=""border-right: 1px solid #000;""> " + workorder.Details + @"  </td>
                        <td style=""border-right: 1px solid #000;""> " + workorder.Priority + @"  </td>
                         <td style=""border-right: 1px solid #000;""> " + workorder.Due.ToShortDateString() + @"  </td>
                        <td style=""border-right: 1px solid #000;""> " + workorder.DaysOverdue + @"  </td>
                        <td> " + workorder.DaysOpen + @" </td>
                </tr >";

        }
    }
}