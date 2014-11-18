using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class DistributionBLL :BaseBLL, IDistributionBLL
    {
        IDistributionDAL _distributionDao = ServiceFactory.GetService<IDistributionDAL>("DistributionDAL");

        #region IDistributionBLL 成员

        public Model.BaseInfo.Distribution GetModel(string DistributionCode)
        {
            return _distributionDao.GetModel(DistributionCode);
        }

        public string GetDistributionNameByCode(string distributionCode)
        {
            return _distributionDao.GetDistributionNameByCode(distributionCode);
        }

        #endregion

        #region IDistributionBLL 成员

        /// <summary>
        /// 取得配送商前一个状态
        /// </summary>
        /// <param name="DistributionCode">配送商Code</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public Model.Common.Enums.BillStatus? GetDistributionPreBillStatus(string DistributionCode, Model.Common.Enums.BillStatus Status)
        {
            if (String.IsNullOrWhiteSpace(DistributionCode)) throw new ArgumentNullException("DistributionCode is null or empty");
            return _distributionDao.GetDistributionPreBillStatus(DistributionCode, Status);
        }

        #endregion

        #region IDistributionBLL 成员

        /// <summary>
        /// 取得出库的后置条件
        /// </summary>
        /// <param name="currentDistributionCode">当前操作的配送商</param>
        /// <param name="targetDistributionCode">出库转到的目的地配送商</param>
        /// <param name="outboundType">出库类型</param>
        /// <returns></returns>
        public OutboundAfterConditionModel GetOutboundAfterConditionModel(string currentDistributionCode, string targetDistributionCode, Enums.SortCenterOperateType outboundType)
        {
            if (String.IsNullOrWhiteSpace(currentDistributionCode)) throw new ArgumentNullException("currentDistributionCode is null or empty.");
            if (String.IsNullOrWhiteSpace(targetDistributionCode)) throw new ArgumentNullException("targetDistributionCode is null or empty.");
            //默认为当前配送商
            OutboundAfterConditionModel condition = new OutboundAfterConditionModel() { CurrentDistributionCode = currentDistributionCode };
            if (outboundType == Enums.SortCenterOperateType.SecondSorting)
            {
                condition.AfterStatus = Enums.BillStatus.WaitingInbound;
                return condition;
            }
            if (outboundType == Enums.SortCenterOperateType.DistributionSorting)
            {
                //目的配送商存在运输中心，则在装车时修改CurrentDistributionCode
                if (_distributionDao.ExistsTrafficCenter(targetDistributionCode))
                {
                    condition.AfterStatus = Enums.BillStatus.Outbounded;
                    return condition;
                }
                condition.CurrentDistributionCode = targetDistributionCode;
                Enums.BillStatus? preStatus = null;
                //存在分拣中心，则进行分拣相关操作
                if (_distributionDao.ExistsSortCenter(targetDistributionCode))
                {
                    preStatus = _distributionDao.GetDistributionPreBillStatus(targetDistributionCode, Enums.BillStatus.HaveBeenSorting);
                    if (preStatus == null)
                    {
                        throw new Exception(String.Format("配送商:{0}配置未配置分拣前置状态", targetDistributionCode));
                    }
                    condition.AfterStatus = preStatus.Value;
                    return condition;
                }
                //进行入站相关操作
                preStatus = _distributionDao.GetDistributionPreBillStatus(targetDistributionCode, Enums.BillStatus.InStation);
                if (preStatus == null)
                {
                    throw new Exception(String.Format("配送商:{0}配置未配置入站前置状态", targetDistributionCode));
                }
                condition.AfterStatus = preStatus.Value;
                return condition;         
            }

            condition.AfterStatus = Enums.BillStatus.Outbounded;
            return condition;
        }

        #endregion

        public IList<Distribution> GetDistributionList()
        {
            return _distributionDao.GetDistributionList();
        }
    }
}
