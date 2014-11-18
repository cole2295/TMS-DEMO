using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Vancl.WuliuSys.ClientLib
{
    [ServiceContract(Namespace = "http://www.wuliusys.com")]
    public interface IPrintService
    {
        /// <summary>
        /// 是否可打印
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool CanPrint();

        /// <summary>
        /// 打印数据
        /// </summary>
        /// <param name="url">下载数据地址</param>
        /// <returns></returns>
        [OperationContract]
        PrintModel Print(string url);
    }
}
