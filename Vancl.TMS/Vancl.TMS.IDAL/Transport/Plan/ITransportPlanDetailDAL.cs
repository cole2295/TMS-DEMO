using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Plan;

namespace Vancl.TMS.IDAL.Transport.Plan
{
    public interface ITransportPlanDetailDAL
    {
        /// <summary>
        /// 新增运输计划明细
        /// </summary>
        /// <param name="model">运输计划明细信息</param>
        /// <returns></returns>
        int Add(TransportPlanDetailModel model);
        /// <summary>
        /// 删除运输计划明细
        /// </summary>
        /// <param name="lstTpid">运输计划id列表</param>
        /// <returns></returns>
        int Delete(List<int> lstTpid);
        /// <summary>
        /// 根据运输计划id取得运输计划详细信息
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        IList<TransportPlanDetailModel> GetByTransportPlanID(int tpid);

        /// <summary>
        /// 取得所有运输计划详细
        /// </summary>
        /// <returns></returns>
        List<TransportPlanDetailLineIDRepairModel> GetValidTransportPlanDetail();

        /// <summary>
        /// 修复运输计划详细中的线路编号
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        int RepairTransportPlanDetailLineID(List<TransportPlanDetailLineIDRepairModel> lstModel);
    }
}
