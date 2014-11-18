using System.Web.Mvc;

namespace Vancl.TMS.Web.Areas.LadingBill
{
    public class LadingBillAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LadingBill";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "LadingBill_default",
                "LadingBill/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
