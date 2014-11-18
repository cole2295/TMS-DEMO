using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.WCFService
{
    [DataContract]
    public class NeedDoTMSModel
    {
        /// <summary>
        /// 是否需要走TMS
        /// </summary>
        [DataMember]
        public bool IsNeedDoTMS { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
