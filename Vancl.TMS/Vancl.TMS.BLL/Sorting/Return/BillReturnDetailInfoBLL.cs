using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.IDAL.Sorting.Return;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data;

namespace Vancl.TMS.BLL.Sorting.Return
{
    /// <summary>
    /// 退货单信息业务实现
    /// </summary>
    public class BillReturnDetailInfoBLL : SortCenterBLL, IBillReturnDetailInfoBLL
    {
        IBillReturnDetailInfoDAL BillReturnDetailInfoDAL = ServiceFactory.GetService<IBillReturnDetailInfoDAL>();
        IBillReturnDAL BillReturnDAL = ServiceFactory.GetService<IBillReturnDAL>();
        IBillDAL BillDAL = ServiceFactory.GetService<IBillDAL>();
        IWaybillDAL waybillDAL = ServiceFactory.GetService<IWaybillDAL>("LMSWaybillDAL_SQL");
        IBillReturnBoxInfoDAL BillReturnBoxInfoDAL = ServiceFactory.GetService<IBillReturnBoxInfoDAL>();
        /// <summary>
        /// 添加一条扫描单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDetail(BillReturnDetailInfoModel model)
        {
            if (model != null)
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    if (BillReturnDetailInfoDAL.Add(model) > 0)
                    {
                        var billmodel = BillDAL.GetBillByFormCode(model.FormCode);
                        if (billmodel != null)
                        {
                            var logModel = new BillChangeLogDynamicModel
                            {
                                CreateBy = UserContext.CurrentUser.ID,
                                CreateDept = UserContext.CurrentUser.DeptID,
                                CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                                CurrentSatus = Enums.BillStatus.ReturnInBound,
                                ReturnStatus = Enums.ReturnStatus.ReturnInbounded,
                                DeliverStationID = billmodel.DeliverStationID,
                                FormCode = model.FormCode,
                                OperateType = Enums.TmsOperateType.ReturnInbound,
                                PreStatus = billmodel.Status,
                            };
                            logModel.ExtendedObj.CreateDeptName = UserContext.CurrentUser.DeptName;
                            if (BillDAL.UpdateBillReturnStatus(model.FormCode, Enums.ReturnStatus.ReturnInbounded) > 0)
                            {
                                logModel.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                            }
                            WriteBillChangeLog(logModel);
                        }
                    }
                    scope.Complete();
                    return 1;
                }
            }
            return -1;

            //return BillReturnDetailInfoDAL.Add(model);
        }
        /// <summary>
        /// 根据系统运单号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetListByFormCodes(string FormCodes)
        {
            List<BillReturnDetailInfoModel> lists = new List<BillReturnDetailInfoModel>();
            lists = BillReturnDetailInfoDAL.GetListByFormCodes(FormCodes);
            return lists;
        }

        /// <summary>
        /// 根据箱号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetListByBoxNo(string boxNo)
        {
            List<BillReturnDetailInfoModel> lists = new List<BillReturnDetailInfoModel>();
            lists = BillReturnDetailInfoDAL.GetListByBoxNo(boxNo,UserContext.CurrentUser.DeptName);
            return lists;
        }
        /// <summary>
        /// 根据箱号获取已经扫描的单号信息并更新ReturnStatus
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> ScanBoxNo(string boxNo,string createDept,string selectStationName)
        {
            List<BillReturnDetailInfoModel> lists = new List<BillReturnDetailInfoModel>();
            lists = BillReturnDetailInfoDAL.GetListByBoxNo(boxNo, createDept);
            DataTable dt = BillReturnBoxInfoDAL.GetCreateDept(boxNo);
            if (dt!=null && dt.Rows.Count>0 )
            {
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    if (dt.Rows[i]["CreateDept"].ToString() == createDept)
                        return lists;
                }
            }
            lists = BillReturnDetailInfoDAL.GetListByBoxNo(boxNo);
            if (lists != null && lists.Count > 0)
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    try
                    {
                        foreach(var item in lists)
                        {
                            BillReturnModel model = BillReturnDAL.GetBillByFormCode(item.FormCode);

                            BillReturnDetailInfoModel billDetail = new BillReturnDetailInfoModel() 
                            {
                            FormCode=model.FormCode,
                            BoxNo=boxNo,
                            CustomerOrder=model.CustomerOrder,
                            CreateDept=createDept,
                            ReturnTo=selectStationName,
                            CreateBy = UserContext.CurrentUser.ID,
                            Weight=item.Weight,
                            IsDeleted=false
                            };
                            this.AddDetail(billDetail);
                            //var logModel = new BillChangeLogDynamicModel
                            //{
                            //    CreateBy = UserContext.CurrentUser.ID,
                            //    CreateDept = UserContext.CurrentUser.DeptID,
                            //    CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                            //    CurrentSatus = Enums.BillStatus.ReturnInBound,
                            //    ReturnStatus = model.ReturnStatus,
                            //    DeliverStationID = model.DeliverStationID,
                            //    FormCode = model.FormCode,
                            //    OperateType = Enums.TmsOperateType.ReturnInbound,
                            //    PreStatus = model.Status,
                            //};
                            //logModel.ExtendedObj.CreateDeptName = UserContext.CurrentUser.DeptName;
                            //if (BillDAL.UpdateBillReturnStatus(model.FormCode, Enums.ReturnStatus.ReturnInbounded) > 0)
                            //{
                            //    BillReturnDetailInfoDAL.UpdateSyncedStatus(model.FormCode);
                            //    logModel.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                            //}
                            //WriteBillChangeLog(logModel);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("扫描箱号并更新箱中单号的逆向状态失败", ex);
                    }
                }
                lists = BillReturnDetailInfoDAL.GetListByBoxNo(boxNo, createDept);
            }
            return lists;

        }
        /// <summary>
        /// 获取箱号信息用于退货交接表打印
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<ReturnBillSortingDetailModel> GetDetailListByBoxNo(string boxNo)
        {
            List<ReturnBillSortingDetailModel> lists = new List<ReturnBillSortingDetailModel>();
            //var amounts = BillReturnDetailInfoDAL.GetDetailListByBoxNo(boxNo);
            using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
            {
                var amounts = client.GetWaybillBackDetailPrintModel(boxNo);
                if (amounts != null && amounts.Length > 0)
                {
                    int i = 0;
                    foreach(var item in amounts)
                    {
                        i++;
                        ReturnBillSortingDetailModel model = new ReturnBillSortingDetailModel()
                        {
                            rNow=i,
                            BoxNo = item.BoxNo,
                            CreateDept = item.CreateDep,
                            CreateTime = item.CreateTime,
                            CustomerOrder = item.OrderNo,
                            FormCode = item.WaybillNo,
                            Weight = item.Weight,
                            formType=item.WaybillTypeName,
                            NeedAmount=item.NeedAmount,
                            ReturnTo=item.ReturnTo
                        };
                        lists.Add(model);
                    }
                }
            }
            return lists;
        }
        /// <summary>
        /// 判断运单是否已经退货分拣称重
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public bool IsReturn(string FormCode)
        {
            return BillReturnDetailInfoDAL.isExist(FormCode);
        }
        /// <summary>
        /// 判断运单是否已经退货分拣称重至分拣中心
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        public bool IsReturn(string FormCode, string returnTo, out string returnBoxNo)
        {
            return BillReturnDetailInfoDAL.IsReturn(FormCode, returnTo, out returnBoxNo);
        }
        /// <summary>
        /// 获取已经退货装箱的箱号
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        public string GetBoxNO(string formCode, string returnTo)
        {
            if (string.IsNullOrEmpty(formCode)) throw new ArgumentNullException();
            if (string.IsNullOrEmpty(returnTo)) throw new ArgumentNullException();
            return BillReturnDetailInfoDAL.GetBoxNO(formCode, returnTo);
        }
        /// <summary>
        /// 删除运单
        /// </summary>
        /// <param name="FormCodeLists"></param>
        /// <returns></returns>
        public int Delete(string FormCodeLists)
        {
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                string[] Formcodes = FormCodeLists.Split(',');
                if(Formcodes.Length>0)
                {
                    foreach (var item in Formcodes)
                    {
                        var billmodel = BillDAL.GetBillByFormCode(item);
                        if (billmodel != null)
                        {
                            var logModel = new BillChangeLogDynamicModel
                            {
                                CreateBy = UserContext.CurrentUser.ID,
                                CreateDept = UserContext.CurrentUser.DeptID,
                                CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                                CurrentSatus = Enums.BillStatus.ReturnOnStation,
                                ReturnStatus = Enums.ReturnStatus.ReturnInTransit,
                                DeliverStationID = billmodel.DeliverStationID,
                                FormCode = item,
                                OperateType = Enums.TmsOperateType.ReturnInbound,
                                PreStatus = billmodel.Status,
                            };
                            logModel.ExtendedObj.CreateDeptName = UserContext.CurrentUser.DeptName;
                            if (BillDAL.UpdateBillReturnStatus(item, Enums.ReturnStatus.ReturnInTransit) > 0)
                            {
                                logModel.ReturnStatus = Enums.ReturnStatus.ReturnInTransit;
                                BillReturnDetailInfoDAL.UpdateSyncedStatus(item);
                            }
                            WriteBillChangeLog(logModel);
                        }
                    }
                }
                scope.Complete();
                return BillReturnDetailInfoDAL.Delete(FormCodeLists);
            }
        }

        /// <summary>
        /// 查看箱中运单是否返货到同一商家或配送商
        /// </summary>
        /// <param name="returnBoxNo"></param>
        /// <returns></returns>
        public BillReturnCountModel GetReturnToCount(string returnBoxNo)
        {
            if (string.IsNullOrEmpty(returnBoxNo)) throw new ArgumentNullException();
            return BillReturnDetailInfoDAL.GetReturnToCount(returnBoxNo);
        }

        /// <summary>
        /// 查询退货出库后的剩余订单
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetBillAfterOutBound(string FormCodes, string CreateDept)
        {
            if (string.IsNullOrEmpty(FormCodes)) throw new ArgumentNullException();
            return BillReturnDetailInfoDAL.GetBillAfterOutBound(FormCodes, CreateDept);
        }


    }
}
