using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Vancl.WuliuSys.ClientLib
{
    [ServiceContract(Namespace = "http://www.wuliusys.com")]
    public interface IWeightService
    {
        /// <summary>
        /// 是否可称重
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool CanWeigh();

        /// <summary>
        /// 获取重量
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        WeighResult GetWeight();
        
        /// <summary>
        /// 开始称重
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        WeighResult StartWeigh();

        /// <summary>
        /// 停止称重
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool StopWeigh();
    }


}
