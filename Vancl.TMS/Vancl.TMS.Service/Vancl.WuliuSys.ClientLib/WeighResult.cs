using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Vancl.WuliuSys.ClientLib
{
    /// <summary>
    /// 方法返回结果信息类
    /// </summary>
    [Serializable]
    [DataContract]
    public class WeighResult
    {
        /// <summary>
        /// 重量
        /// </summary>
        [DataMember]
        public decimal Weight { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        [DataMember]
        public string Message
        {
            get
            {
                if (Exception != null) return Exception.Message;
                return "";
            }
            set { }
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        //   [DataMember]
        public Exception Exception { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>  
        [DataMember]
        public bool IsSucces
        {
            get
            {
                return Weight > 0;
            }
            set { }
        }


    }

}
