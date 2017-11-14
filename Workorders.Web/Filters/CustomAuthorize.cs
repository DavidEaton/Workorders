using Workorders.Data.Enums;
using Workorders.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Workorders.Web.Filters
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public AccessType AccessType { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            // Make sure the user is authenticated.
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            //if valid user
            if (UserFunctions.IsValidUser())
            {
                return true;
            }
            else if (UserFunctions.IsValidUser() == false)
            {
                return false;
            }

            //check if user is an admin
            var isAdmin = UserFunctions.IsAdmin();

            if (isAdmin) //if user is admin then allow it access all pages of app
            {
                return true;
            }

            //NOTE: if we are here means user is not an admin

            //if access type is admins
            if (AccessType == AccessType.Admins)
            {
                return false;
            }

            //NOTE: if we are here means user is not an admin and AccessType is not Admins

            //check if user is an editor
            var isEditor = UserFunctions.IsEditor();

            if (isEditor) //if user is editor then allow it access all non-admin pages
            {
                return true;
            }

            //NOTE: if we are here means user is not an admin or an editor and access type is not Admins

            if (AccessType == AccessType.Editors)
            {
                return false;
            }

            //check if user is a viewer
            var isViewer = UserFunctions.IsViewer();

            return isViewer;

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult) //Unauthorized request
            {
                filterContext.Result = new RedirectResult("~/AccessDenied"); //AccessDenied
            }
        }
    }
}