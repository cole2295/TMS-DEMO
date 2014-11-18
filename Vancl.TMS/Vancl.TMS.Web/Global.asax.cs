using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Logging;
using Vancl.TMS.Web.WebControls.Mvc.Authorization;

namespace Vancl.TMS.Controls
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TMSHandleErrorAttribute());
            //filters.Add(new TMSAuthorizeAttribute());
            filters.Add(new XmlAuthorizeAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.DefaultBinder = new DefaultModelBinderEx();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error()
        {
            HttpContext ctx = HttpContext.Current;

            Exception ex = ctx.Server.GetLastError();

            //Log.loggeremail.Error(ex.Message, ex);

            //if (ex != null)
            //{
               
            //}

        }

    }
}