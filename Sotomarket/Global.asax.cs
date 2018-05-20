using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sotomarket
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("ru-ru");
            //ru-RU
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
        }

        protected void Application_PreRequestHandlerExecute()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ru-RU");
        }
    }
}
