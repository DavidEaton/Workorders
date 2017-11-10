using System;
using System.Collections.Generic;
using System.Linq;

namespace Workorders.Web.Services.Tasks
{
  public class ScheduleTask
  {
    public string Name { get; set; }
    public int Seconds { get; set; }
    public string Type { get; set; }
    public bool Enabled { get; set; }
    public bool StopOnError { get; set; }
    public DateTime Send { get; set; }
  }
}