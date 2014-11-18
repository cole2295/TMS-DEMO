using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using System.ServiceModel;

namespace Vancl.TMS.IBLL.Delivery.DataInteraction.Entrance
{
    /// <summary>
    /// TMS数据交互数据入口接口
    /// </summary>
    public interface IDeliveryDataEntranceBLL
    {

        /// <summary>
        /// 数据入口
        /// </summary>
        /// <param name="entranceModel">数据入口对象</param>
        /// <returns></returns>
        ResultModel DataEntrance(TMSEntranceModel entranceModel);

    }

}
