using Workorders.Web.Services.Tasks;
using Workorders.Web.ModelBinder;
using Workorders.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Workorders.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IdentityConfig.ConfigureIdentity();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //custom model binder for currency formated Estimate 
            ModelBinders.Binders.Add(typeof(WorkorderEditViewModel), new CurrencyFormatedEstimateBinder());

            //init tasks in background
            TaskManager.Instance.Initialize();
            TaskManager.Instance.Start();
        }

        private void WSFederationAuthenticationModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
            if (!String.IsNullOrEmpty(IdentityConfig.Realm))
            {
                e.SignInRequestMessage.Realm = IdentityConfig.Realm;
            }
        }
    }
}
