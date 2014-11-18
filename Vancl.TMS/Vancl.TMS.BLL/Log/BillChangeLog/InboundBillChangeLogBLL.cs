using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    /// <summary>
    /// 入库表单变更日志
    /// </summary>
    internal class InboundBillChangeLogBLL : InboundBaseBillChangeLogBLL
    {
        
        protected override bool BatchGetOutsourcingLogModel(List<BillChangeLogDynamicModel> listdynamicModel, out List<OutsourcingLogModel> listOutsourcingLog)
        {
            if (listdynamicModel == null) throw new ArgumentNullException("listdynamicModel is null");
            listOutsourcingLog = null;
            var tmplistOutsourcingLog = new List<OutsourcingLogModel>();
            listdynamicModel.ForEach(l =>
            {
                var agentUser = l.ExtendedObj.AgentUser;
                if (agentUser != null && agentUser.ID > 0)
                {
                    tmplistOutsourcingLog.Add(new OutsourcingLogModel()
                    {
                        FormCode = l.FormCode,
                        AgentDeptID = agentUser.DeptID,
                        AgentUserID = agentUser.ID,
                        AgentDistributionCode = agentUser.DistributionCode,
                        PrincipalUserID = l.CreateBy,
                        PrincipalDeptID = l.CreateDept,
                        PrincipalDistributionCode = l.CurrentDistributionCode,
                        UpdateBy = l.CreateBy,
                        OperateType = l.OperateType,
                        Note = GetNote(l)               //方便扩展，后续可记录外包自定义日志
                    });
                }
            });
            listOutsourcingLog = tmplistOutsourcingLog;
            return listOutsourcingLog.Count > 0;
        }

        protected override bool GetOutsourcingLogModel(BillChangeLogDynamicModel dynamicModel, out OutsourcingLogModel osLogModel)
        {
            if (dynamicModel == null) throw new ArgumentNullException("dynamicModel is null");
            osLogModel = null;
            var agentUser = dynamicModel.ExtendedObj.AgentUser;
            if (agentUser != null && agentUser.ID > 0)
            {
                OutsourcingLogModel outsourcingLog = new OutsourcingLogModel();
                outsourcingLog.FormCode = dynamicModel.FormCode;
                outsourcingLog.AgentDeptID = agentUser.DeptID;
                outsourcingLog.AgentUserID = agentUser.ID;
                outsourcingLog.AgentDistributionCode = agentUser.DistributionCode;
                outsourcingLog.PrincipalUserID = dynamicModel.CreateBy;
                outsourcingLog.PrincipalDeptID = dynamicModel.CreateDept;
                outsourcingLog.PrincipalDistributionCode = dynamicModel.CurrentDistributionCode;
                outsourcingLog.UpdateBy = dynamicModel.CreateBy;
                outsourcingLog.OperateType = dynamicModel.OperateType;
                outsourcingLog.Note = GetNote(dynamicModel);            //方便扩展，后续可记录外包自定义日志

                osLogModel = outsourcingLog;
                return true;
            }
            return false;
        }

     
    }
}
