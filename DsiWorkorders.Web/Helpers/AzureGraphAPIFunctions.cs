using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.WebPages;
using System.Globalization;

namespace Workorders.Web.Helpers
{
    public class AzureGraphAPIFunctions
    {

        public static AuthenticationResult authenticationResult;

        /// <summary>
        ///     Async task to acquire token for Application.
        /// </summary>
        /// <returns>Async Token for application.</returns>
        public static async Task<string> AcquireTokenAsync(string tenantId)
        {
            AuthenticationContext authContext = new AuthenticationContext(String.Format(CultureInfo.InvariantCulture, "https://login.windows.net/{0}", tenantId));


            if (authenticationResult == null || authenticationResult.AccessToken == null ||
            authenticationResult.AccessToken.IsEmpty() || authenticationResult.ExpiresOn < DateTime.UtcNow)
            {
                ClientCredential credential = new ClientCredential(GraphConfiguration.GraphApiClientId, GraphConfiguration.GraphApiKey);
                authenticationResult = await authContext.AcquireTokenAsync(GraphConfiguration.GraphResourceUrl, credential);
            }

            return authenticationResult.AccessToken;
        }

        /// <summary>
        ///Get Active Directory Client for Application.
        /// </summary>
        /// <returns>ActiveDirectoryClient for Application.</returns>
        public static ActiveDirectoryClient GetActiveDirectoryClient(string tenantId)
        {
            Uri baseServiceUri = new Uri(GraphConfiguration.GraphResourceUrl);
            ActiveDirectoryClient activeDirectoryClient =
                new ActiveDirectoryClient(new Uri(baseServiceUri, tenantId),
                    async () => await AcquireTokenAsync(tenantId));
            return activeDirectoryClient;
        }

        /// <summary>
        /// method will check if logged in user is member of AD group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static bool CheckIfUserIsMemberOfGroup(string groupName, ClaimsPrincipal incomingPrincipal)
        {
            var username = incomingPrincipal.Identity.Name;
            var tenantId = incomingPrincipal.FindFirst(GraphConfiguration.TenantIdClaimType).Value;

            //create Azure Graph Api client to make api calls
            var client = GetActiveDirectoryClient(tenantId);

            IGroup group = null;
            try
            {
                //get group by groupName
                group = client.Groups.Where(x => x.DisplayName == groupName).ExecuteSingleAsync().Result;
                if (group == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogEvent("Workorders DSI - " + LogLevel.Error.ToString(), username + " : Exception while getting Group: " + groupName + DateTime.UtcNow, ex);

                return false;
            }

            //get logged in user details from azure
            var loggedInUser = client.Users.Where(x => x.UserPrincipalName == username).ExecuteSingleAsync().Result;

            if (loggedInUser == null)
            {
                return false;
            }

            var result = AsyncHelper.RunSync<bool?>(() => client.IsMemberOfAsync(group.ObjectId, loggedInUser.ObjectId));

            return result ?? false;
        }
    }
}