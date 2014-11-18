using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Vancl.TMS.Web.Areas.BaseInfo.Models;

namespace Vancl.TMS.Web.Areas.BaseInfo.Controllers
{
    public class AudioSettingController : Controller
    {
        //
        // GET: /BaseInfo/AudioSetting/
        private AudioSettingModel  model = new AudioSettingModel();

        private void Init ()
        {
            string succPath = Server.MapPath("~/Content/media/Done/");
            string failPath = Server.MapPath("~/Content/media/error/");
            DirectoryInfo  succdir = new DirectoryInfo(succPath);
            DirectoryInfo  faildir = new DirectoryInfo(failPath);
            foreach (var filename in succdir.GetFiles())
            {
                model.sucAudioFiles.Add(filename.Name);
            }
            foreach (var filename in faildir.GetFiles())
            {
                model.failAudioFiles.Add(filename.Name);
            }
            ViewBag.SearchTitle = "声音自定义设置";
        }

        public ActionResult Index(string fileName)
        {
            Init();
            return View(model);
        }

    }
}
