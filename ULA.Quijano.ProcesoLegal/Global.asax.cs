using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ULA.Quijano.ProcesoLegal
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //logger.Trace("trace log message");
            //logger.Debug("debug log message");
            //logger.Info("info log message");
            //logger.Warn("warn log message");
            //logger.Error("error log message");
            //logger.Fatal("fatal log message");


        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Logger logger = LogManager.GetLogger("Globalasax");

        //    Exception exception = Server.GetLastError();

        //    Server.ClearError();

        //    logger.Fatal("[MvcApplication / Application_Error] " + exception.ToString());

        //    HttpContext.Current.RewritePath("Home/Error");
        //}
    }


}