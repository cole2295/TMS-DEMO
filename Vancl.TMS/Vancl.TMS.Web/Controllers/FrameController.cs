using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Util.Drawing;
using System.Collections;
using System.IO;
using System.Drawing.Imaging;
using Vancl.TMS.Web.Models;
using System.Web.Security;
using Vancl.TMS.Web.Common;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Web.WebControls.Mvc.Authorization;
using RFD.SSO.WebClient;
using System.Data;
using System.Configuration;

namespace Vancl.TMS.Web.Controllers
{
    public class FrameController : Controller
    {
        private IUserContextBLL UserContextService = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");
        private IVerificationCode VerificationCodeService = ServiceFactory.GetService<IVerificationCode>("VerificationCode");
        private IPermissionBLL PermissionService = ServiceFactory.GetService<IPermissionBLL>("PermissionBLL");
        //    IUserContextBLL UserContextSerivce = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Menu()
        {
            var MenuList = UserContextService.GetUserMenuByUserName(UserContext.CurrentUser.UserCode);
            return View(MenuList);
        }

        public ActionResult LeftLine()
        {
            return View();
        }

        public ActionResult TopLine()
        {
            return View();
        }

        public ActionResult TabPage()
        {
            return View();
        }

        public ActionResult Notice()
        {
            var notice = PermissionService.GetNotice();
            if (notice == null) notice = new PmsNotice();
            return View(notice);
        }


        private string JSList = string.Empty;

    }
}