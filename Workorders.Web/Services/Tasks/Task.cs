using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using Workorders.Web.Helpers;

namespace Workorders.Web.Services.Tasks
{
  /// <summary>
  /// Task
  /// </summary>
  public partial class Task
  {
    /// <summary>
    /// Ctor for Task
    /// </summary>
    private Task()
    {
      Enabled = true;
    }

    /// <summary>
    /// Ctor for Task
    /// </summary>
    /// <param name="task">Task </param>
    public Task(ScheduleTask task)
    {
      Type = task.Type;
      Enabled =  task.Enabled;
      StopOnError = task.StopOnError;
      Name = task.Name;
      Send = task.Send;
    }

    private ITask CreateTask()
    {
      ITask task = null;
      if (Enabled)
      {
        var type2 = System.Type.GetType(Type);
        if (type2 != null)
        {
          object instance = Activator.CreateInstance(type2);
          task = instance as ITask;
        }
      }
      return task;
    }

    /// <summary>
    /// Executes the task
    /// </summary>
    public void Execute()
    {
      IsRunning = true;

      try
      {
        var task = CreateTask();
        if (task != null)
        {
          //execute task
          task.Execute();
        }
      }
      catch (Exception exc)
      {
        Enabled = !StopOnError;

        string whoToContact = ConfigurationManager.AppSettings["TechnicalSupportEmails"];

        MailMessage mailMessage = new MailMessage();

        var subject = string.Format("Error while running the '{0}' schedule task.", Name);

        StringBuilder msg = new StringBuilder();
        msg.AppendLine();
        msg.AppendLine("Task Name: " + Name);
        msg.AppendLine();
        msg.AppendLine("Task Type: " + Type);
        msg.AppendLine();
        msg.AppendLine("Error: " + exc.Message);
        msg.AppendLine();
        msg.AppendLine("Stack Trace :" + exc.StackTrace);

        Email.SendEmail(whoToContact, subject, msg.ToString(), msg.ToString(), null, null);
      }

      IsRunning = false;
    }

    /// <summary>
    /// A value indicating whether a task is running
    /// </summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// A value indicating type of the task
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// A value indicating whether to stop task on error
    /// </summary>
    public bool StopOnError { get; private set; }

    /// <summary>
    /// Get the task name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// A value indicating whether the task is enabled
    /// </summary>
    public bool Enabled { get; private set; }

    public DateTime? Send { get; set; }
  }
}