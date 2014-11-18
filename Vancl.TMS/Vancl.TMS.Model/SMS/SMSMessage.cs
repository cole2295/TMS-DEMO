using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.SMS
{
    /// <summary>
    /// 短信消息
    /// </summary>
    [Serializable]
    public class SMSMessage
    {

        /// <summary>
        /// 单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public String Content { get; set; }


    }
}
