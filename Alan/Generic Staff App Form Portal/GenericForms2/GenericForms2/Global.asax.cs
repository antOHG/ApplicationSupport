using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using GenericForms2;
using System.Diagnostics;

namespace GenericForms2
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Tracing.Source.TraceInformation(Utility.FormatErrMsg("Application is starting up"));
        }

        void Application_End(object sender, EventArgs e)
        {
            
            DataAccess.Instance.Dispose();
            Tracing.Source.TraceInformation(Utility.FormatErrMsg("Application is ending"));
            Tracing.Source.Close();
            
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            DataAccess.Instance.Dispose();
            Tracing.Source.Close();
        }
    }
}
