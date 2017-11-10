using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Workorders.Web.Helpers
{
    public class GraphConfiguration
    {
        private static string tenantIdClaimType = ConfigurationManager.AppSettings["TenantIdClaimType"];
        private static string graphResourceUrl = ConfigurationManager.AppSettings["GraphUrl"];
        private static string graphApiVersion = ConfigurationManager.AppSettings["GraphApiVersion"];
        private static string graphApiClientId = ConfigurationManager.AppSettings["GraphApiClientId"];
        private static string graphApiKey = ConfigurationManager.AppSettings["GraphApiKey"];

        public static string GraphApiVersion
        {
            get { return graphApiVersion; }
            set { graphApiVersion = value; }
        }

        public static string GraphResourceUrl
        {
            get { return graphResourceUrl; }
            set { graphResourceUrl = value; }
        }

        public static string TenantIdClaimType
        {
            get { return tenantIdClaimType; }
        }

        public static string GraphApiClientId
        {
            get { return graphApiClientId; }
            set { graphApiClientId = value; }
        }

        public static string GraphApiKey
        {
            get { return graphApiKey; }
            set { graphApiKey = value; }
        }

    }
}