using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous;

namespace Vancl.TMS.IBLL.Synchronous
{
    /// <summary>
    /// 出库同步接口
    /// </summary>
    public interface IOutboundBLL
    {
        void LMSOutbondDataToFile(OutboundReadParam argument);
        void FileToTMSOrder(OutboundWriteParam argument);
    }


}
