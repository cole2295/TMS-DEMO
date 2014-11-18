using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class AudioSettingModel
    {
        public string SucAudioFile { get; set;}

        public string FailAudioFile { get; set;}

        //public List<AudioSettingModel> Models { get; set; }

        public List<string> sucAudioFiles  = new List<string>();

        public List<string> failAudioFiles = new List<string>();
    }
}