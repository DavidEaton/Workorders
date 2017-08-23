using DsiWorkorders.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DsiWorkorders.Web.CustomAuthenticaton
{
    public class CustomClaimsAuthenticatonManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated == true)
            {
                var logMessage = incomingPrincipal.Identity.Name + " logged in " + DateTime.UtcNow + " <br/>";

                //check if user is an CsiViewer
                var isCsiViewer = Helpers.AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Helpers.Settings.CsiViewersRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsCsiViewer", isCsiViewer.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isCsiViewer = " + isCsiViewer + " " + DateTime.UtcNow + " <br/>";

                //check if user is an CsiEditor
                var isCsiEditor = Helpers.AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Helpers.Settings.CsiEditorsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsCsiEditor", isCsiEditor.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isCsiEditor = " + isCsiEditor + " " + DateTime.UtcNow + " <br/>";

                //check if user is an CsiAdmin
                var isCsiAdmin = Helpers.AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Helpers.Settings.CsiAdminsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsCsiAdmin", isCsiAdmin.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isCsiAdmin = " + isCsiAdmin + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiViewer
                var isDsiViewer = Helpers.AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Helpers.Settings.DsiViewersRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsiViewer", isDsiViewer.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsiViewer = " + isDsiViewer + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiEditor
                var isDsiEditor = Helpers.AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Helpers.Settings.DsiEditorsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsiEditor", isDsiEditor.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsiEditor = " + isDsiEditor + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiAdmin
                var isDsiAdmin = Helpers.AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Helpers.Settings.DsiAdminsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsiAdmin", isDsiAdmin.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsiAdmin = " + isDsiAdmin + " " + DateTime.UtcNow + " <br/>";

                Logger.LogEvent("Workorders DSI/CSI - " + LogLevel.Info.ToString(), logMessage + DateTime.UtcNow);

                if (UserFunctions.IsAllowedAccessToCompany(isCsiViewer, isCsiEditor, isCsiAdmin, isDsiViewer, isDsiEditor, isDsiAdmin) == false)
                {
                    CompanyCookie.SelectedCompany = null;
                }

            }

            return incomingPrincipal;
        }
    }
}