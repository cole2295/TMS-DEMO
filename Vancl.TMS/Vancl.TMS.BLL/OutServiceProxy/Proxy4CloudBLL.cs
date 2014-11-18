using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.OutServiceProxy;
using Vancl.TMS.Model.OutServiceProxy;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.OutServiceProxy
{
    public class Proxy4CloudBLL : BaseBLL, IProxy4Cloud
    {
        #region IProxy4Cloud 成员

        public ResultModel AddAgingMonitoringLog(AgingMonitoringLogProxyModel proxyModel)
        {
            ResultModel result = new ResultModel();
            try
            {
                using (AgingMonitoringService.DistributionOperateLogClient proxy = new AgingMonitoringService.DistributionOperateLogClient())
                {
                    if (proxy.AddMonitorLog(new AgingMonitoringService.DistributionOperateLog()
                            {
                                WaybillNo = long.Parse(proxyModel.FormCode),
                                WaybillType = ((int)proxyModel.BillType).ToString(),
                                Area = proxyModel.OperateArea,
                                City = proxyModel.OperateCity,
                                CurrentDistributionCode = proxyModel.CurrentDistributionCode,
                                DistributionCode = proxyModel.DistributionCode,
                                MerchantId = proxyModel.MerchantID,
                                OperateStation = proxyModel.OperateDept,
                                OperateTime = proxyModel.OperateTime,
                                OperateUser = proxyModel.Operator,
                                Province = proxyModel.OperateProvince,
                                Status = ((int)proxyModel.Status).ToString(),
                                TransPortType = proxyModel.TransportType.HasValue ? (int)proxyModel.TransportType.Value : (int?)null ,
                                TransPortStationId = proxyModel.TransportStationID                          
                            })
                        )
                    {
                        return result.Succeed("新增加时效记录成功");
                    }
                    else
                    {
                        return result.Failed("新增加时效记录失败");
                    }
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("新增加时效记录异常:{0}", ex.Message));
            }
        }

        #endregion
    }
}
