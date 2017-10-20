using DsiWorkorders.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DsiWorkorders.Web.CustomAuthenticaton
{
    public class CustomClaimsAuthenticatonManager : ClaimsAuthenticationManager
    {
        string company = ConfigurationManager.AppSettings["CompanyAbbr"];

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated == true)
            {
                var logMessage = incomingPrincipal.Identity.Name + " logged in " + DateTime.UtcNow + " <br/>";

                //check if user is an Viewer
                var isViewer = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.ViewersRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsViewer", isViewer.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isViewer = " + isViewer + " " + DateTime.UtcNow + " <br/>";

                //check if user is an Editor
                var isEditor = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.EditorsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsEditor", isEditor.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isEditor = " + isEditor + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiAdmin
                var isAdmin = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.AdminsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsAdmin", isAdmin.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isAdmin = " + isAdmin + " " + DateTime.UtcNow + " <br/>";

                Logger.LogEvent(company + " Workorders " + LogLevel.Info.ToString(), logMessage + DateTime.UtcNow);

            }

            return incomingPrincipal;
        }
    }
}