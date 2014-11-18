using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Sorting;
using System.Data;
using Vancl.TMS.Util.ClsExtender;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.IBLL.BaseInfo;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    public abstract class Lms2TmsStrategy : BaseBLL
    {
        /// <summary>
        /// LMS数据层
        /// </summary>
        private ILms2TmsSyncLMSDAL _lmsDAL = null;
        protected ILms2TmsSyncLMSDAL LmsDAL
        {
            get
            {
                if (_lmsDAL == null)
                {
                    _lmsDAL = ServiceFactory.GetService<ILms2TmsSyncLMSDAL>("Lms2TmsSyncLMSDAL");
                }
                return _lmsDAL;
            }
        }
        /// <summary>
        /// TMS数据层
        /// </summary>
        private ILms2TmsSyncTMSDAL _tmsDAL = null;
        protected ILms2TmsSyncTMSDAL TmsDAL
        {
            get
            {
                if (_tmsDAL == null)
                {
                    _tmsDAL = ServiceFactory.GetService<ILms2TmsSyncTMSDAL>("Lms2TmsSyncTMSDAL");
                }
                return _tmsDAL;
            }
        }

        /// <summary>
        /// 主表业务层
        /// </summary>
        private IBillBLL _billBLL = null;
        protected IBillBLL BillBLL
        {
            get
            {
                if (_billBLL == null)
                {
                    _billBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
                }
                return _billBLL;
            }
        }


        private List<Enums.BillStatus> _statusForBillSyncService = null;
        /// <summary>
        /// 订单入口服务中需要同步数据的状态
        /// </summary>
        protected List<Enums.BillStatus> StatusForBillSyncService
        {
            get
            {
                if (_statusForBillSyncService == null)
                {
                    _statusForBillSyncService = new List<Enums.BillStatus>();
                    _statusForBillSyncService.Add(Enums.BillStatus.GisAssigned);
                    _statusForBillSyncService.Add(Enums.BillStatus.HaveBeenSorting);
                    _statusForBillSyncService.Add(Enums.BillStatus.ValidWaitingAssign);
                    _statusForBillSyncService.Add(Enums.BillStatus.WaitingInbound);
                }
                return _statusForBillSyncService;
            }
        }

        /// <summary>
        /// 执行同步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DoSync(LmsWaybillStatusChangeLogModel model)
        {
            bool isOk = ImportBaseDataToTms(model);
            ResultModel rm = null;
            if (isOk)
            {
                rm = Sync(model);
            }
            else
            {
                rm = ErrorResult("同步基础数据失败");
            }
            return rm;
        }

        /// <summary>
        /// 具体的同步过程，需要子类实现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract ResultModel Sync(LmsWaybillStatusChangeLogModel model);

        /// <summary>
        /// 判断存在并同步基础数据到tms
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ImportBaseDataToTms(LmsWaybillStatusChangeLogModel model)
        {
            if (model.OperateType == Enums.Lms2TmsOperateType.BillSyncService
                && !StatusForBillSyncService.Contains(model.Status))
            {
                return true;
            }
            bool isBillExists = TmsDAL.IsBillExists(model.WaybillNo.ToString());
            bool isBillInfoExists = true;
            if (!isBillExists)
            {
                isBillInfoExists = TmsDAL.IsBillInfoExists(model.WaybillNo.ToString());
            }
            BillModel billModel = null;
            BillInfoModel billInfoModel = null;
            if (!isBillInfoExists || !isBillExists)
            {
                BuildBillDataModel(model, out billModel, out billInfoModel);
                if (billModel == null && billInfoModel == null)
                {
                    return false;
                }
            }
            bool isOk = true;
            //先插入明细表
            if (!isBillInfoExists && billInfoModel != null)
            {
                isOk = TmsDAL.AddBillInfo(billInfoModel) == 1;
            }
            //插入主表
            if (!isBillExists && billModel != null && isOk)
            {
                isOk = TmsDAL.AddBill(billModel) == 1;
            }
            return isOk;
        }

        /// <summary>
        /// 生成基础数据对象
        /// </summary>
        /// <param name="model"></param>
        /// <param name="billModel"></param>
        /// <param name="billInfoModel"></param>
        private void BuildBillDataModel(LmsWaybillStatusChangeLogModel model, out BillModel billModel, out BillInfoModel billInfoModel)
        {
            billModel = null;
            billInfoModel = null;
            DataTable dt = LmsDAL.GetLmsWayBillData(model);
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            DataRow dr = dt.Rows[0];
            billModel = new BillModel();
            billModel.BillType = dr.GetValue<Enums.BillType>("WaybillType");
            billModel.CreateBy = dr.GetValue<int>("CreatBy");
            billModel.CreateDept = dr.GetValue<int>("CreatStation");
            billModel.CreateTime = dr.GetValue<DateTime>("CreatTime");
            billModel.CurrentDistributionCode = dr.GetValue<string>("CurrentDistributionCode");
            billModel.CustomerOrder = String.IsNullOrWhiteSpace(model.CustomerOrder) ? " " : model.CustomerOrder;
            billModel.DeliverCode = dr.GetValue<string>("DeliverCode");
            if (String.IsNullOrWhiteSpace(billModel.DeliverCode))
            {
                billModel.DeliverCode = " ";            //为空字符串或者null，默认空格符
            }
            billModel.DeliverStationID = model.DeliverStationID;
            billModel.DistributionCode = dr.GetValue<string>("DistributionCode");
            billModel.FormCode = model.WaybillNo.ToString();
            billModel.MerchantID = model.MerchantID;
            billModel.ReturnStatus = dr.GetValue<Enums.ReturnStatus?>("BackStatus");
            billModel.Source = dr.GetValue<Enums.BillSource>("Sources");
            billModel.Status = model.Status;
            billModel.UpdateBy = model.CreateBy;
            billModel.UpdateDept = model.CreateStation;
            billModel.UpdateTime = model.CreateTime;
            billModel.WarehouseID = dr.GetValue<string>("WarehouseID");
            billInfoModel = new BillInfoModel();
            billInfoModel.BillGoodsType = string.IsNullOrWhiteSpace(dr.GetValue<string>("WaybillProperty")) ? Enums.BillGoodsType.Normal : dr.GetValue<Enums.BillGoodsType>("WaybillProperty");
            billInfoModel.CustomerBoxNo = dr.GetValue<string>("WayBillBoxNo");
            billInfoModel.CustomerWeight = dr.GetValue<decimal>("MerchantWeight");
            billInfoModel.FormCode = model.WaybillNo.ToString();
            billInfoModel.InsuredAmount = dr.GetValue<decimal>("ProtectedPrice");
            billInfoModel.PackageCount = dr.GetValue<int>("PackageCount");
            billInfoModel.PackageMode = dr.GetValue<string>("WaybillBoxModel");
            try
            {
                billInfoModel.PayType = EnumHelper.GetValue<Enums.PayType>(dr.GetValue<string>("AcceptType"));
            }
            catch
            {
                billInfoModel.PayType = Enums.PayType.Cash;
            }
            billInfoModel.ReceivableAmount = dr.GetValue<decimal>("NeedAmount");
            billInfoModel.Tips = dr.GetValue<string>("Tips");
            billInfoModel.TotalAmount = dr.GetValue<decimal>("Amount");
            billInfoModel.Weight = dr.GetValue<decimal>("WayBillInfoWeight");
        }

        /// <summary>
        /// 更新操作的返回值
        /// </summary>
        /// <param name="affectCount">影响行数</param>
        /// <param name="message">存入日志表的内容</param>
        /// <returns></returns>
        protected override ResultModel UpdateResult(int affectCount, string message = "")
        {
            ResultModel rm = new ResultModel();
            rm.IsSuccess = affectCount > 0;
            rm.Message = message;
            return rm;
        }
    }
}
