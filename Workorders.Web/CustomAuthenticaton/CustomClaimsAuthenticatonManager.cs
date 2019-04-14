using Workorders.Web.Helpers;
using System;
using System.Security.Claims;

namespace Workorders.Web.CustomAuthenticaton
{
    public class CustomClaimsAuthenticatonManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated == true)
            {
                var logMessage = incomingPrincipal.Identity.Name + " logged in " + DateTime.UtcNow + " <br/>";

                //check if user is an CsiViewer
                var isCsiViewer = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.CsiViewersRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsCsiViewer", isCsiViewer.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isCsiViewer = " + isCsiViewer + " " + DateTime.UtcNow + " <br/>";

                //check if user is an CsiEditor
                var isCsiEditor = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.CsiEditorsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsCsiEditor", isCsiEditor.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isCsiEditor = " + isCsiEditor + " " + DateTime.UtcNow + " <br/>";

                //check if user is an CsiAdmin
                var isCsiAdmin = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.CsiAdminsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsCsiAdmin", isCsiAdmin.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isCsiAdmin = " + isCsiAdmin + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiViewer
                var isDsiViewer = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.DsiViewersRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsiViewer", isDsiViewer.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsiViewer = " + isDsiViewer + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiEditor
                var isDsiEditor = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.DsiEditorsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsiEditor", isDsiEditor.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsiEditor = " + isDsiEditor + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsiAdmin
                var isDsiAdmin = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.DsiAdminsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsiAdmin", isDsiAdmin.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsiAdmin = " + isDsiAdmin + " " + DateTime.UtcNow + " <br/>";



                //check if user is an DsnViewer
                var isDsnViewer = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.DsnViewersRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsnViewer", isDsnViewer.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsnViewer = " + isDsnViewer + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsnEditor
                var isDsnEditor = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.DsnEditorsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsnEditor", isDsnEditor.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsnEditor = " + isDsnEditor + " " + DateTime.UtcNow + " <br/>";

                //check if user is an DsnAdmin
                var isDsnAdmin = AzureGraphAPIFunctions.CheckIfUserIsMemberOfGroup(Settings.DsnAdminsRole, incomingPrincipal);

                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim("IsDsnAdmin", isDsnAdmin.ToString()));

                logMessage += incomingPrincipal.Identity.Name + " isDsnAdmin = " + isDsnAdmin + " " + DateTime.UtcNow + " <br/>";

                Logger.LogEvent("Workorders - " + LogLevel.Info.ToString(), logMessage + DateTime.UtcNow);

                if (UserFunctions.IsAllowedAccessToCompany(isCsiViewer,
                                                           isCsiEditor,
                                                           isCsiAdmin,
                                                           isDsiViewer,
                                                           isDsiEditor,
                                                           isDsiAdmin,
                                                           isDsnViewer,
                                                           isDsnEditor,
                                                           isDsnAdmin
                                                           ) == false)
                {
                    CompanyCookie.SelectedCompany = null;
                }

            }

            return incomingPrincipal;
        }
    }
}