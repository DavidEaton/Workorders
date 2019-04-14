using System.Configuration;

namespace Workorders.Web.Helpers
{
    public class Settings
    {
        public static string GetConnectionStringName()
        {
            var company = CompanyCookie.SelectedCompany;

            if (string.IsNullOrEmpty(company) == false)
            {
                if (ConfigurationManager.AppSettings[company] != null)
                {
                    return ConfigurationManager.AppSettings[company];
                }
            }

            return "AppConnection";
        }

        public static string CsiViewersRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["CsiViewers"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["CsiViewers"].ToString();
                }
                return "CsiViewers";
            }
        }

        public static string CsiEditorsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["CsiEditors"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["CsiEditors"].ToString();
                }
                return "CsiEditors";
            }
        }

        public static string CsiAdminsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["CsiAdmins"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["CsiAdmins"].ToString();
                }
                return "CsiAdmins";
            }
        }

        public static string DsiViewersRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["DsiViewers"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["DsiViewers"].ToString();
                }
                return "DsiViewers";
            }
        }

        public static string DsiEditorsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["DsiEditors"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["DsiEditors"].ToString();
                }
                return "DsiEditors";
            }
        }

        public static string DsiAdminsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["DsiAdmins"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["DsiAdmins"].ToString();
                }
                return "DsiAdmins";
            }
        }

        public static string DsnViewersRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["DsnViewers"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["DsnViewers"].ToString();
                }
                return "DsnViewers";
            }
        }

        public static string DsnEditorsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["DsnEditors"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["DsnEditors"].ToString();
                }
                return "DsnEditors";
            }
        }

        public static string DsnAdminsRole
        {
            get
            {
                if (ConfigurationManager.AppSettings["DsnAdmins"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["DsnAdmins"].ToString();
                }
                return "DsnAdmins";
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