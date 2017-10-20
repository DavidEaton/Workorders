using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace DsiWorkorders.Web.Helpers
{
    public class Logger
    {

        public static void LogEvent(LogLevel loglevel, string customMessage, Exception ex = null)
        {
            LogEvent(loglevel.ToString().ToUpper(), customMessage, ex);
        }

        public static void LogEvent(string emailSubject, string customMessage, Exception ex = null)
        {
            if (bool.Parse(ConfigurationManager.AppSettings["IsLoggerEnabled"]) == false)
            {
                return;
            }

            if (ex != null)
            {
                //cleaning the ex to get a more condenced version of it
                Exception realerror = ex;
                //while (realerror.InnerException != null)
                //  realerror = realerror.InnerException;

                customMessage += "<br/>" + realerror.ToString();

            }


            //get configs from web.config
            var toEmails = ConfigurationManager.AppSettings["TechnicalSupportEmails"];

            //replace <br/> tag with \n to get the Plain Text Version
            var customMessageTextOnly = customMessage;

            //send email
            Email.SendEmail(toEmails, emailSubject, customMessage, customMessageTextOnly, null, null);
        }
    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}