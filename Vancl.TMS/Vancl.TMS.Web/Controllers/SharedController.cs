using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Vancl.TMS.IBLL.History;
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
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.IO;

namespace Vancl.TMS.Web.Controllers
{
    public class SharedController : Controller
    {
        IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        IFileProcessingBLL _fileProcessingBLL = ServiceFactory.GetService<IFileProcessingBLL>("FileProcessingBLL");
        /// <summary>
        /// 取得城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCitiesHasAuthority()
        {
            return Json(_expressCompanyBLL.GetCitiesHasAuthority(true), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据市ID取得站点
        /// </summary>
        /// <param name="cityID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStationsHasAuthorityByCityID(string cityID)
        {
            return Json(_expressCompanyBLL.GetStationsHasAuthorityByCityID(cityID, true), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得省
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllPronvice()
        {
            return Json(_expressCompanyBLL.GetAllProvince(true), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据省ID取得市
        /// </summary>
        /// <param name="provinceID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCitiesByProviceID  (string provinceID)
        {
            return Json(_expressCompanyBLL.GetCitiesByProviceID(provinceID, true), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetHelpFile(string type, string sysname, string menuname)
        {
            ResultModel r = new ResultModel();
            r.IsSuccess = false;
            r.Message = "无可下载文件,请至PMS配置";
            if (type == "help" && !string.IsNullOrEmpty(sysname) && !string.IsNullOrEmpty(menuname))
            {
                try
                {
                    string ftpFileName=_fileProcessingBLL.GetHelpFile(sysname, menuname);
                    if (!string.IsNullOrEmpty(ftpFileName))
                    {
                        string defaultRemotePath = ftpFileName.Substring(0, ftpFileName.LastIndexOf('/')+1);
                        string remoteFileName = ftpFileName.Substring(ftpFileName.LastIndexOf('/')+1,
                                                                        ftpFileName.Length -
                                                                        ftpFileName.LastIndexOf('/')-1);
                        FtpHelper ftpHelper = new FtpHelper(FtpAction.DownLoad, "", remoteFileName, defaultRemotePath, "rfd", "rufengda@FK*()");
                        ftpHelper.Action();
                        Stream fFileStream = ftpHelper.FileStream;
                        long fFileSize = fFileStream.Length;

                        Response.ContentType = "application/octet-stream";

                        String agent = (String)Request.Headers.Get("USER-AGENT");
                        if (agent != null && agent.IndexOf("MSIE") == -1)
                        {// FF      
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + remoteFileName);
                        }
                        else
                        { // IE      
                            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(Path.GetFileName(remoteFileName), System.Text.Encoding.UTF8) + "\"");
                        }  
                        
                        Response.AddHeader("Content-Length", fFileSize.ToString());
                        byte[] fFileBuffer = new byte[fFileSize];
                        fFileStream.Read(fFileBuffer, 0, (int)fFileSize);

                        fFileStream.Flush();
                        fFileStream.Close();
                        Response.BinaryWrite(fFileBuffer);
                        Response.Flush();
                        Response.End();
                        return new EmptyResult();
                    }
                    else
                    {
                        r.IsSuccess = false;
                        r.Message = "无可下载文件,请至PMS配置";
                    }
                }
                catch(Exception e)
                {

                    r.IsSuccess = false;
                    r.Message = e.Message;
                }
            }
            return View("DownLoadFileFaultDialog", r);
            //return Json(new { done = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
