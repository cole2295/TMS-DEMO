using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.IBLL;
using System.Reflection;

namespace Vancl.TMS.Test.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        private ITest<CarriersModel> testService = ServiceFactory.GetService<ITest<CarriersModel>>();
        private ITest<TestModel> testExtService = ServiceFactory.GetService<ITest<TestModel>>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(Assembly.GetAssembly(Type.GetType("Vancl.TMS.BLL.TestBllExt")).FullName);
            //Response.Write(typeof(IMyTest<CarriersModel, TestModel>).FullName);

            Response.Write("<font color=red>test:</font>" + testService.OutputString(new CarriersModel()) + "<br><font color=red>testExt:</font>" + testExtService.OutputString(new TestModel()));
        }
    }
}