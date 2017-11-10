using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Workorders.Web.Helpers
{
    public class Settings
    {

        public static string ViewersRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["Viewers"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["Viewers"].ToString();
                }
                return "Viewers";
            }
        }

        public static string EditorsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["Editors"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["Editors"].ToString();
                }
                return "Editors";
            }
        }

        public static string AdminsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["Admins"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["Admins"].ToString();
                }
                return "Admins";
            }
        }

        public static string HelpVideoLinkUsers
        {
            get
            {
                if (ConfigurationManager.AppSettings["HelpVideoLinkUsers"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["HelpVideoLinkUsers"].ToString();
                }
                return "HelpVideoLinkUsers";
            }
        }

        public static string HelpVideoLinkEditors
        {
            get
            {
                if (ConfigurationManager.AppSettings["HelpVideoLinkEditors"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["HelpVideoLinkEditors"].ToString();
                }
                return "HelpVideoLinkEditors";
            }
        }

        public static string HelpVideoLinkMobile
        {
            get
            {
                if (ConfigurationManager.AppSettings["HelpVideoLinkMobile"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["HelpVideoLinkMobile"].ToString();
                }
                return "HelpVideoLinkMobile";
            }
        }

        public static string CompanyName
        {
          get
          {
            if (ConfigurationManager.AppSettings["CompanyName"]
                != null)
            {
              return ConfigurationManager.
                  AppSettings["CompanyName"].ToString();
            }
            return "Alpha Information Systems, Inc.";
          }
        }

        public static string CompanyAbbr
        {
            get
            {
                if (ConfigurationManager.AppSettings["CompanyAbbr"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["CompanyAbbr"].ToString();
                }
                return "AISI";
            }
        }

        public static string ApplicationName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ApplicationName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ApplicationName"].ToString();
                }
                return "Workorders";
            }
        }

        public static bool IsProduction
        {
          get
          {
            if (ConfigurationManager.AppSettings["IsProduction"]
                != null)
            {
              return bool.Parse(ConfigurationManager.AppSettings["IsProduction"].ToString());
            }
            return false;
          }
        }

        public static string ApplicationDescription
        {
            get
            {
                if (ConfigurationManager.AppSettings["ApplicationDescription"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ApplicationDescription"].ToString();
                }
                return "Manage Service Location Maintenance issues, aka Workorders.";
            }
        }
    
    }
}