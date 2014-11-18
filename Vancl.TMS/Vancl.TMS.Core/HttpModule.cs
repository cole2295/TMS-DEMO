using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Vancl.TMS.Core.Logging;


namespace Vancl.TMS.Core
{
    public class HttpModule : IHttpModule
    {

        static HttpModule()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }

        //private static bool IsRegisted = false;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "MainFrame", id = UrlParameter.Optional } // 参数默认值
            );

            //IsRegisted = true;
        }

        //protected void Application_Start(object sender, EventArgs e)
        //{
        //    RegisterGlobalFilters(GlobalFilters.Filters); 
        //    if (!IsRegisted)
        //    {
        //        AreaRegistration.RegisterAllAreas();
        //        RegisterRoutes(RouteTable.Routes);
        //    }
        //}

        public void application_Error(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            Exception ex = ctx.Server.GetLastError();

            //ILogBLL<ErrorLogModel> _bll = ServiceFactory.GetService<ILogBLL<ErrorLogModel>>("");
            //ErrorLogModel error = new ErrorLogModel();
            //_bll.Write(error);

            Log.loggeremail.Error(ex.Message,ex);

            if (ex != null)
            {
                ctx.Response.Redirect("home/error");
            }
        }


        #region IHttpModule 成员
        public void Dispose()

        { }


        public void Init(HttpApplication application)
        {
            //application.BeginRequest += new EventHandler(Application_Start);

            //application.EndRequest += new EventHandler(Application_EndRequest);

            application.Error += new EventHandler(application_Error);
        }

        #endregion
    }
}