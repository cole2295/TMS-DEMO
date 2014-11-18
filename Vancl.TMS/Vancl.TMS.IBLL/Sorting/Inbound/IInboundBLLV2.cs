using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;

namespace Vancl.TMS.IBLL.Sorting.Inbound
{
    public interface IInboundBLLV2
    {
        InboundResultModelV2 SimpleInboundV2(InboundSimpleArgModelV2 argument);

        /// <summary>
        /// 取得当前操作入库数量(新)
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        int GetInboundCountV2( SortCenterUserModel userModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        int GetFormCodeInboundCount(string FormCode);
        /// <summary>
        /// 获得订单详细信息
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillInfoModel GetBillInfoByFormCode(string FormCode);

        /// <summary>
        /// 重新称重
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        InboundResultModelV2 ReWeight(InboundSimpleArgModelV2 argument);
 
    }
}
