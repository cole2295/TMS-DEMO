using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Vancl.TMS.Web.Models;
using System.Web.Security;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Web.WebControls.Mvc.Authorization;
using RFD.SSO.WebClient;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.Security;

namespace Vancl.TMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserContextBLL UserContextService = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");
        private IVerificationCode VerificationCodeService = ServiceFactory.GetService<IVerificationCode>("VerificationCode");
        private IDistributionBLL DistributionService = ServiceFactory.GetService<IDistributionBLL>("DistributionBLL");
        //    IUserContextBLL UserContextSerivce = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");

        /// <summary>
        /// 验证码名字
        /// </summary>
        private const string VerificationCodeID = "VerificationCodeID";

        

        public ActionResult Index()
        {
            ViewBag.IsSsoLogin = LoginType.IsSsoLogin;
            if (LoginType.IsSsoLogin)
            {
                SsoClient ssoClient = new SsoClient();
                var navi = ssoClient.GetNavigationHtml();
                ViewBag.Navi = new MvcHtmlString(navi);
            }
            UserModel curUser = UserContext.AgentUser;
            if (curUser == null)
            {
                curUser = UserContext.CurrentUser;
            }
            ViewBag.DistributionName = DistributionService.GetDistributionNameByCode(curUser.DistributionCode);
            //添加外包内容列表
            List<OutSourcingModel> list = UserContextService.GetAgentRelations(curUser.ID);
            ViewData["OutSourcingList"] = list;
            ViewData["CurrentUser"] = curUser;
            //清除代理人
            QuitOutsourcing();
            return View("Index");
        }

        /// <summary>
        /// 选择外包公司
        /// </summary>
        /// <param name="principalUserID"></param>
        [HttpGet]
        public void OutsourcingChange(int principalUserID)
        {
            //TODO:先验证该用户是否为登录人所能够代理的用户
            int loginUserID = 0;
            if (UserContext.AgentUser != null)
            {
                loginUserID = UserContext.AgentUser.ID;
            }
            else
            {
                loginUserID = UserContext.CurrentUser.ID;
            }
            //验证没过，不允许切换(防止在界面修改js代码进行攻击)
            if (!UserContextService.IsSetOutsourcingRelation(principalUserID, loginUserID))
            {
                throw new Exception("请不要进行非法攻击");
            }
            //把当前登录人放到代理人里面，如果代理类不为空则不存
            if (UserContext.AgentUser == null)
            {
                UserContext.SetAgentCookie(UserContext.CurrentUser);
            }
            UserModel principaUser = UserContextService.GetUserInfo(principalUserID);
            UserContext.SetAuthCookie(principaUser, false);
        }

        /// <summary>
        /// 退出外包模式
        /// </summary>
        [HttpGet]
        public void QuitOutsourcing()
        {
            //把代理类放回当前登录人
            if (UserContext.AgentUser == null)
            {
                return;
            }
            //参数createPersistentCookie默认为false
            UserContext.SetAuthCookie(UserContext.AgentUser, false);
            UserContext.SetAgentCookie(null);
        }

        public ActionResult SsoLogin()
        {
            LoginType.IsSsoLogin = true;

            SsoClient ssoClinet = new SsoClient();
            ProcessLogin processLogin = new ProcessLogin();

            if (!ssoClinet.Login(ssoClinet.QueryUidFromUrl(),
                ssoClinet.QueryTokenFromUrl(),
                processLogin))
            {
                ssoClinet.RedirectToLoginPage();
                return new EmptyResult();
            }

            //Response.Redirect(string.Format("index.aspx?ssotoken={0}&ssouid={1}",
            //       Request.QueryString["ssotoken"],
            //       Request.QueryString["ssouid"]), true);


            return Redirect(FormsAuthentication.DefaultUrl);
        }

        [AllowAnoumous] 
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }

            return View();
        }

        [AllowAnoumous]
        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                LoginType.IsSsoLogin = false;

                HttpCookie cookie = Request.Cookies[VerificationCodeID];
                //if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value) || !VerificationCodeService.VerifyCode(cookie.Value, model.VerificationCode, true))
                if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value) || MD5.Encrypt(model.VerificationCode.ToLower()) != cookie.Value)
                {
                    ModelState.AddModelError("VerificationCode", "验证码不正确。");
                }
                else
                {
                    bool loginResult = false;
                    try
                    {
                        loginResult = Membership.ValidateUser(model.UserName, model.Password);
                        if (loginResult)
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Set(cookie);
                            //var user = UserContextService.GetUserInfo(model.UserName);
                            var user = new UserModel()
                            {
                                UserName="zengwei",
                                UserCode="zengwei",
                                RoleID=1,
                                DeptID=1,
                                DeptName="company",
                                DistributionCode="shunfeng",
                                ID=1
                            };
                            UserContext.SetAuthCookie(user, model.RememberMe);
                            //  FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                // FormsAuthentication.
                                return Redirect(FormsAuthentication.DefaultUrl);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("UserName", "提供的用户名或密码不正确。");
                        }
                    }
                    catch (Exception ex)
                    {
                        loginResult = false;
                        ModelState.AddModelError("Exception", ex.Message);
                    }
                }
            }
            GenerateVerificationCode();
            return View(model);
        }

        [AllowAnoumous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //清空代理人cookie
            UserContext.SetAgentCookie(null);
            if (LoginType.IsSsoLogin)
            {
                SsoClient ssoClinet = new SsoClient();
                ssoClinet.RedirectToLoginPage();
                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [AllowAnoumous]
        public ActionResult GoLogin()
        {
            return View();
        }

        [AllowAnoumous]
        public ActionResult CheckVerificationCode(string VerificationCode)
        {
            bool result = false;
            HttpCookie cookie = Request.Cookies[VerificationCodeID];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                result = false;
            }
            else
            {
                //result = VerificationCodeService.VerifyCode(cookie.Value, VerificationCode, false);
                result = MD5.Encrypt(VerificationCode.ToLower()) == cookie.Value;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnoumous]
        public ActionResult GenerateVerificationCode()
        {
            var verificationcode = Authcode.Generate();
            //string id = VerificationCodeService.SetVerificationCode(verificationcode.Code);
            string id = MD5.Encrypt(verificationcode.Code.ToLower());
            if (string.IsNullOrWhiteSpace(id)) return new EmptyResult();

            HttpCookie cookie = new HttpCookie(VerificationCodeID, id);
            Response.Cookies.Add(cookie);
            MemoryStream ms = new MemoryStream();
            verificationcode.Bitmap.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
            return File(ms, "image/jpeg");
        }

        public ActionResult Welcome()
        {
            return View();
        }


        [AllowAnoumous]
        public ActionResult Test()
        {
            return View();
        //    return DateTime.Now.ToString();
        }


        [AllowAnoumous]
        public ActionResult Except(string msg)
        {
            throw new Exception(msg);
        }

        [AllowAnoumous]
        public string SetCookie(string id, string value)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = ".ASPXAUTH";
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }
            HttpCookie cookie = new HttpCookie(id, value);
            Response.Cookies.Add(cookie);

            return id + "-" + value;
        }


        public class ProcessLogin : IProcessLoginInfo
        {
            private IUserContextBLL UserContextService = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");

            public void ProcessLoginInfo(SsoResponse ssoResponse)
            {
                var userid = ssoResponse.EmployeeID.ToString();
                var userCode = ssoResponse.EmployeeCode;
                var userName = ssoResponse.EmployeeName;
                var expressId = ssoResponse.StationID.ToString();
                var expressName = ssoResponse.Companyname;
                var distributionCode = ssoResponse.DistributionCode;
                var sysManager = ssoResponse.SysManager.ToString();
                //      string sid = userMappingSv.UpdateUserMapping(ssoResponse.EmployeeCode);

                var user = UserContextService.GetUserInfo(userCode);
                UserContext.SetAuthCookie(user, false);
            }
        }
    }
}
