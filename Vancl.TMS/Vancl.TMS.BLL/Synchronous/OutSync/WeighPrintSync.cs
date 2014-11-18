using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.IDAL.LMS;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    internal enum SyncDbType
    {
        Sqlserver,
        Oracle,
    }
    internal partial class WeighPrintSync
    {
        IBillInfoDAL BillInfoDAL = ServiceFactory.GetService<IBillInfoDAL>();
        IBillWeighDAL BillWeighDAL = ServiceFactory.GetService<IBillWeighDAL>();
        IWaybillThirdPartyInfoDAL WaybillThirdPartyInfoDAL_SQL = ServiceFactory.GetService<IWaybillThirdPartyInfoDAL>("WaybillThirdPartyInfoDAL_SQL");
        IWaybillThirdPartyInfoDAL WaybillThirdPartyInfoDAL_Oracle = ServiceFactory.GetService<IWaybillThirdPartyInfoDAL>("WaybillThirdPartyInfoDAL_Oracle");

        /// <summary>
        /// 获取数据操作类
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public IWaybillThirdPartyInfoDAL GetWaybillThirdPartyInfoDAL(SyncDbType dbType)
        {
            IWaybillThirdPartyInfoDAL WaybillThirdPartyInfoDAL = dbType == SyncDbType.Sqlserver ? WaybillThirdPartyInfoDAL_SQL : WaybillThirdPartyInfoDAL_Oracle;
            return WaybillThirdPartyInfoDAL;
        }

        /// <summary>
        /// 获取运单总重量
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public decimal GetBillWeight(string formCode)
        {
            var model = BillInfoDAL.GetBillInfoByFormCode(formCode);
            if (model == null) throw new Exception("未找到对应的运单信息");
            return model.Weight;
        }

        /// <summary>
        /// 同步运单重量
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public ResultModel SyncBillWeight(SyncDbType dbType, string formCode, decimal weight)
        {
            IWaybillThirdPartyInfoDAL WaybillThirdPartyInfoDAL = GetWaybillThirdPartyInfoDAL(dbType);
            int count = WaybillThirdPartyInfoDAL.SaveWeight(formCode, weight);
            return ResultModel.Create(count > 0);
        }
        /// <summary>
        /// 同步运单箱信息
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public ResultModel SyncBillPackageInfo(SyncDbType dbType, string formCode)
        {
            IWaybillThirdPartyInfoDAL WaybillThirdPartyInfoDAL = GetWaybillThirdPartyInfoDAL(dbType);
            var list = BillWeighDAL.GetListByFormCode(formCode);
            if (list == null || list.Count == 0) return ResultModel.Create(false, "没有相关称重信息");
            //未同步数据
            var notSyncList = list.Where(x => x.SyncFlag == Enums.SyncStatus.NotYet);
            foreach (var package in notSyncList)
            {
                var pm = WaybillThirdPartyInfoDAL.GetPackageModel(formCode, package.PackageIndex);
                if (pm != null)
                {//存在,更新
                    pm.Weight = package.Weight ?? 0;
                    pm.UpdateBy = package.UpdateBy;
                    pm.UpdateTime = package.UpdateTime;
                    WaybillThirdPartyInfoDAL.UpdateBillPackage(pm);
                }
                else
                {//不存在，添加
                    pm = new Model.LMS.WaybillThirdPartyInfoEntityModel
                    {
                        BoxNo = package.PackageIndex.ToString(),
                        CreateBy = package.CreateBy,
                        UpdateTime = package.UpdateTime,
                        BoxType = "",
                        UpdateBy = package.UpdateBy,
                        Weight = package.Weight ?? 0,
                        CreateTime = package.CreateTime,
                        IsDelete = false,
                        Volume = 0,
                        WaybillNo = long.Parse(formCode)
                    };
                    WaybillThirdPartyInfoDAL.AddBillPackage(pm);
                }
            }

            return ResultModel.Create(true);
        }

        public void UpdateWeighSyncStatus(string formCode, Enums.SyncStatus prevFlag, Enums.SyncStatus nextFlag)
        {
            //更改同步状态
            BillWeighDAL.UpdateSyncStatus(formCode, prevFlag, nextFlag);
        }


    }
}
