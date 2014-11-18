using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    /// <summary>
    /// 分拣装箱逻辑层
    /// </summary>
    public class SortingPackingBLL : SortCenterBLL, ISortingPackingBLL
    {
        ISortingPackingDAL _sortingPackingDAL = ServiceFactory.GetService<ISortingPackingDAL>("SortingPackingDAL");
        IExpressCompanyDAL _expressCompanyDal = ServiceFactory.GetService<IExpressCompanyDAL>("ExpressCompanyDAL");
        IFormula<String, InboundPackingNoContextModel> _packingNoGenerator = FormulasFactory.GetFormula<IFormula<String, InboundPackingNoContextModel>>("PackingNoGenerateFormula");
        IBillDAL _billDAL = ServiceFactory.GetService<IBillDAL>("BillDAL");

        #region ISortingPackingBLL 成员

        /// <summary>
        /// 根据箱号取得箱中所有订单
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        public List<SortingPackingBillModel> GetPackingBillsByBoxNo(string boxNo)
        {
            return _sortingPackingDAL.GetPackingBillsByBoxNo(boxNo);
        }

        /// <summary>
        /// 根据单号取得箱中的所有订单
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        public List<SortingPackingBillModel> GetPackingBillsByFormCode(string formCode)
        {
            return _sortingPackingDAL.GetPackingBillsByFormCode(formCode);
        }

        /// <summary>
        /// 取得箱子对象
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        public SortingPackingBoxModel GetPackingBox(string boxNo)
        {
            return _sortingPackingDAL.GetPackingBox(boxNo);
        }

        /// <summary>
        /// 该运单是否在当前操作站点已经装箱
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <param name="expressID">操作站点</param>
        /// <returns></returns>
        public bool IsBillAlreadyPacked(string formCode, int expressID)
        {
            return _sortingPackingDAL.IsBillAlreadyPacked(formCode, expressID);
        }

        /// <summary>
        /// 取得分拣装箱订单
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        public SortingPackingBillModel GetSortingPackingBill(string formCode)
        {
            if (String.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException("formcode is null or empty.");
            var result = _sortingPackingDAL.GetSortingPackingBill(formCode);
            if (result != null)
            {
                if (result.DepartureID < 1)
                {
                    result.DepartureID = UserContext.CurrentUser.DeptID;
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 称重装箱
        /// </summary>
        /// <param name="inboundData"></param>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="lstFormCode"></param>
        /// <param name="arrivalID"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public ResultModel AddInboundBox(ViewInboundValidateModel inboundData, string boxNo, decimal weight, List<string> lstFormCode, int arrivalID, bool isUpdate)
        {
            if (isUpdate)
            {
                return UpdateInboundBox(inboundData, boxNo, weight, lstFormCode, arrivalID);
            }
            else
            {
                return AddNewInboundBox(inboundData, boxNo, weight, lstFormCode, arrivalID);
            }
        }

        public ResultModel AddInboundBoxV2(ViewInboundValidateModel inboundData, string boxNo, decimal weight, string formCode, bool isNewbox, ViewPackingBoxToModel vpBox)
        {
            var lstFormCode = new List<string> { formCode };
            List<SortingPackingCheckModel> lstCheckModel = _sortingPackingDAL.GetPackingCheckModel(lstFormCode, inboundData.OpUser.ExpressId.Value);

            var boxForms = _sortingPackingDAL.GetFormCodesByBoxNo(boxNo);
            int boxCount = 0;
            if (boxForms == null)
            {
                boxCount += 1;
            }
            else
            {
                boxCount = boxForms.Count + 1;
            }

            //取得Inbox
            InboundPackingEntityModel boxModel = BuildPackingBox(boxNo, weight, boxCount, vpBox.ArrivalId, lstCheckModel[0], inboundData.OpUser.UserId);

            try
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    if (isNewbox)
                    {
                        _sortingPackingDAL.AddInboundPacking(boxModel);
                    }
                    else
                    {
                        _sortingPackingDAL.UpdateInboundPacking(boxModel);
                    }


                    //todo inboundData.OpUser.UserId
                    _sortingPackingDAL.BatchAddInboundPackingDetail(boxModel.BoxNo, new List<string> { formCode }, 1);

                    _billDAL.UpdateBillStatus(formCode, Enums.BillStatus.Inbounded);

                    var listLogModel = new List<BillChangeLogDynamicModel>();
                    var logItem = new BillChangeLogDynamicModel()
                    {
                        CreateBy = inboundData.OpUser.UserId,
                        CreateDept = inboundData.OpUser.ExpressId.Value,
                        CurrentDistributionCode = inboundData.OpUser.DistributionCode,
                        CurrentSatus = Enums.BillStatus.Inbounded,
                        DeliverStationID = lstCheckModel[0].ArrivalID,
                        FormCode = lstCheckModel[0].FormCode,
                        OperateType = Enums.TmsOperateType.Packing,
                        PreStatus = lstCheckModel[0].Status
                    };
                    logItem.ExtendedObj.BoxNo = boxModel.BoxNo;
                    logItem.ExtendedObj.IsAdd = true;
                    listLogModel.Add(logItem);

                    WriteBillChangeLog_Batch(listLogModel);
                    scope.Complete();
                }
                return InfoResult("装箱操作成功!");
            }
            catch (Exception ex)
            {
                return ErrorResult(String.Format("装箱操作失败! {0} ", ex.Message));
            }
        }

        /// <summary>
        /// 获得目的站
        /// </summary>
        /// <param name="opuserExpressId"></param>
        /// <param name="billArrivalId"></param>
        /// <returns></returns>
        private int GetArrivalId(int opuserExpressId, int billArrivalId)
        {
            //todo del
            int arrivalId = 0;

            //var scenter = _expressCompanyDal.GetSortingCenterByStation(billArrivalId);
            //if (scenter.HasValue)
            //{
            //    arrivalId = opuserExpressId == scenter.Value ? billArrivalId : scenter.Value;
            //}
            //else
            //{
            //    arrivalId = billArrivalId;
            //}

            return arrivalId;
        }

        #endregion

        /// <summary>
        /// 更新装箱
        /// </summary>
        /// <param name="inboundData"></param>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="lstFormCode"></param>
        /// <param name="arrivalID"></param>
        /// <returns></returns>
        private ResultModel UpdateInboundBox(ViewInboundValidateModel inboundData, string boxNo, decimal weight, List<string> lstFormCode, int arrivalID)
        {
            string message = "";
            List<SortingPackingCheckModel> lstCheckModel = null;
            InboundPackingEntityModel boxModel = null;
            //判断箱子是否已出库
            message = CheckBoxAlreadyOutbound(boxNo);
            if (!string.IsNullOrEmpty(message))
            {
                return ErrorResult(message);
            }
            if (lstFormCode != null && lstFormCode.Count > 0)
            {
                //判断是否走城际
                if (!IsNeedTMSTransfer(inboundData.OpUser.ExpressId.Value, arrivalID))
                {
                    throw new Exception("出发地和目的地不走城际运输!");
                }
                lstCheckModel = _sortingPackingDAL.GetPackingCheckModel(lstFormCode, inboundData.OpUser.ExpressId.Value);
                //判断运单号是否存在
                message = CheckFormCodeExists(lstCheckModel, lstFormCode);
                if (!string.IsNullOrEmpty(message))
                {
                    return ErrorResult(message);
                }
                //判断状态是否符合
                message = CheckWaybillStatusValid(lstCheckModel, lstFormCode);
                if (!string.IsNullOrEmpty(message))
                {
                    return ErrorResult(message);
                }
                //判断运单是否属于该分拣中心
                message = CheckWaybillBelongs(lstCheckModel, lstFormCode, inboundData);
                if (!string.IsNullOrEmpty(message))
                {
                    return ErrorResult(message);
                }
            }
            //取得Inbox
            if (lstFormCode != null && lstFormCode.Count > 0)
            {
                boxModel = BuildPackingBox(boxNo, weight, lstFormCode.Count, arrivalID, lstCheckModel[0], inboundData.OpUser.UserId);
            }
            //取得原始装箱订单号
            var lstPastFormCode = _sortingPackingDAL.GetBillModelByBoxNo(boxNo);
            if (lstPastFormCode == null || lstPastFormCode.Count == 0)
            {
                return ErrorResult("该箱不存在或已被删除！");
            }
            //删除的订单号
            List<string> lstDeleteFormCode = null;
            //增加的订单号
            List<string> lstAddFormCode = null;
            if (boxModel == null)
            {
                lstDeleteFormCode = new List<string>(lstPastFormCode.Count);
                lstPastFormCode.ForEach(p =>
                {
                    lstDeleteFormCode.Add(p.FormCode);
                });
            }
            else
            {
                lstDeleteFormCode = new List<string>();
                lstAddFormCode = new List<string>();
                lstFormCode.ForEach(l =>
                {
                    lstAddFormCode.Add(l);
                });
                lstPastFormCode.ForEach(l =>
                {
                    if (lstFormCode.Exists(m => m == l.FormCode))
                    {
                        lstAddFormCode.Remove(l.FormCode);
                    }
                    else
                    {
                        lstDeleteFormCode.Add(l.FormCode);
                    }
                });
            }
            try
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    //删除解除装箱的数据
                    if (lstDeleteFormCode != null && lstDeleteFormCode.Count > 0)
                    {
                        _sortingPackingDAL.BatchDeleteInboundPackingDetail(boxNo, lstDeleteFormCode);
                    }
                    if (boxModel == null)
                    {
                        _sortingPackingDAL.DeleteInboundPacking(boxNo);
                    }
                    //增加新数据
                    if (boxModel != null)
                    {
                        _sortingPackingDAL.UpdateInboundPacking(boxModel);
                        if (lstAddFormCode != null && lstAddFormCode.Count > 0)
                        {
                            _sortingPackingDAL.BatchAddInboundPackingDetail(boxNo, lstAddFormCode, inboundData.OpUser.UserId);
                        }
                    }
                    //写操作日志
                    var listLogModel = new List<BillChangeLogDynamicModel>();
                    //新录入数据
                    if (lstCheckModel != null && lstCheckModel.Count > 0)
                    {
                        foreach (var item in lstCheckModel)
                        {
                            if (lstAddFormCode != null && lstAddFormCode.Exists(p => p.Equals(item.FormCode)))
                            {
                                var logItem = new BillChangeLogDynamicModel()
                                {
                                    CreateBy = inboundData.OpUser.UserId,
                                    CreateDept = inboundData.OpUser.ExpressId.Value,
                                    CurrentDistributionCode = inboundData.OpUser.DistributionCode,
                                    CurrentSatus = item.Status,
                                    DeliverStationID = item.ArrivalID,
                                    FormCode = item.FormCode,
                                    OperateType = Enums.TmsOperateType.Packing,
                                    PreStatus = item.Status
                                };
                                logItem.ExtendedObj.BoxNo = boxNo;
                                logItem.ExtendedObj.IsAdd = true;
                                listLogModel.Add(logItem);
                            }
                        }
                    }
                    if (lstPastFormCode != null && lstPastFormCode.Count > 0)
                    {
                        foreach (var item in lstPastFormCode)
                        {
                            if (lstDeleteFormCode != null && lstDeleteFormCode.Exists(p => p.Equals(item.FormCode)))
                            {
                                var logItem = new BillChangeLogDynamicModel()
                                {
                                    CreateBy = inboundData.OpUser.UserId,
                                    CreateDept = inboundData.OpUser.ExpressId.Value,
                                    CurrentDistributionCode = inboundData.OpUser.DistributionCode,
                                    CurrentSatus = item.Status,
                                    DeliverStationID = item.DeliverStationID,
                                    FormCode = item.FormCode,
                                    OperateType = Enums.TmsOperateType.Packing,
                                    PreStatus = item.Status
                                };
                                logItem.ExtendedObj.BoxNo = boxNo;
                                logItem.ExtendedObj.IsAdd = false;
                                listLogModel.Add(logItem);
                            }
                        }
                    }
                    if (listLogModel != null && listLogModel.Count > 0)
                    {
                        WriteBillChangeLog_Batch(listLogModel);
                    }
                    scope.Complete();
                }
                return InfoResult(boxModel == null ? "解除箱子全部关联成功!" : "重装箱操作成功!");
            }
            catch (Exception ex)
            {
                return ErrorResult(boxModel == null ? String.Format("解除箱子全部关联失败!{0}", ex.Message) : String.Format("重装箱操作失败!{0}", ex.Message));
            }
        }

        /// <summary>
        /// 添加装箱
        /// </summary>
        /// <param name="inboundData"></param>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="lstFormCode"></param>
        /// <param name="arrivalID"></param>
        /// <returns></returns>
        private ResultModel AddNewInboundBox(ViewInboundValidateModel inboundData, string boxNo, decimal weight, List<string> lstFormCode, int arrivalID)
        {
            string message = "";
            if (!IsNeedTMSTransfer(inboundData.OpUser.ExpressId.Value, arrivalID))
            {
                throw new Exception("出发地和目的地不走城际运输!");
            }
            List<SortingPackingCheckModel> lstCheckModel = _sortingPackingDAL.GetPackingCheckModel(lstFormCode, inboundData.OpUser.ExpressId.Value);
            //判断运单号是否存在
            message = CheckFormCodeExists(lstCheckModel, lstFormCode);
            if (!string.IsNullOrEmpty(message))
            {
                return ErrorResult(message);
            }
            //判断状态是否符合
            message = CheckWaybillStatusValid(lstCheckModel, lstFormCode);
            if (!string.IsNullOrEmpty(message))
            {
                return ErrorResult(message);
            }
            //判断是否已经装箱
            message = CheckWaybillAlreadyInbox(lstCheckModel, lstFormCode);
            if (!string.IsNullOrEmpty(message))
            {
                return ErrorResult(message);
            }
            //判断运单是否属于该分拣中心
            message = CheckWaybillBelongs(lstCheckModel, lstFormCode, inboundData);
            if (!string.IsNullOrEmpty(message))
            {
                return ErrorResult(message);
            }
            //取得Inbox
            InboundPackingEntityModel boxModel = BuildPackingBox(boxNo, weight, lstFormCode.Count, arrivalID, lstCheckModel[0], inboundData.OpUser.UserId);
            try
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    _sortingPackingDAL.AddInboundPacking(boxModel);
                    _sortingPackingDAL.BatchAddInboundPackingDetail(boxNo, lstFormCode, inboundData.OpUser.UserId);
                    var listLogModel = new List<BillChangeLogDynamicModel>(lstCheckModel.Count);
                    foreach (var item in lstCheckModel)
                    {
                        var logItem = new BillChangeLogDynamicModel()
                        {
                            CreateBy = inboundData.OpUser.UserId,
                            CreateDept = inboundData.OpUser.ExpressId.Value,
                            CurrentDistributionCode = inboundData.OpUser.DistributionCode,
                            CurrentSatus = item.Status,
                            DeliverStationID = item.ArrivalID,
                            FormCode = item.FormCode,
                            OperateType = Enums.TmsOperateType.Packing,
                            PreStatus = item.Status
                        };
                        logItem.ExtendedObj.BoxNo = boxNo;
                        logItem.ExtendedObj.IsAdd = true;
                        listLogModel.Add(logItem);
                    }
                    WriteBillChangeLog_Batch(listLogModel);
                    scope.Complete();
                }
                return InfoResult("装箱操作成功!");
            }
            catch (Exception ex)
            {
                return ErrorResult(String.Format("装箱操作失败! {0} ", ex.Message));
            }
        }

        /// <summary>
        /// 创建表单改变日志对象
        /// </summary>
        /// <param name="inboundData"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        private BillChangeLogDynamicModel BuildBillChangeLogDynamicModel(ViewInboundValidateModel inboundData, string boxNo)
        {
            BillChangeLogDynamicModel logModel = new BillChangeLogDynamicModel();
            logModel.CurrentDistributionCode = inboundData.OpUser.DistributionCode;
            logModel.OperateType = Enums.TmsOperateType.Packing;
            logModel.ExtendedObj.BoxNo = boxNo;
            return logModel;
        }

        /// <summary>
        /// 检查该箱是否已出库
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        private string CheckBoxAlreadyOutbound(string boxNo)
        {
            Enums.BoxOutBoundedStatus status = _sortingPackingDAL.GetBoxOutBoundedStatus(boxNo);
            if (status == Enums.BoxOutBoundedStatus.NotExists)
            {
                return string.Format("箱号:{0}不存在或已经被删除,请重新生成箱号进行装箱!", boxNo);
            }
            if (status == Enums.BoxOutBoundedStatus.Outbounded)
            {
                return string.Format("箱号:{0}已经出库!", boxNo);
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查是否已装箱
        /// </summary>
        /// <param name="lstCheckModel"></param>
        /// <param name="lstWaybillNo"></param>
        /// <returns></returns>
        private string CheckWaybillAlreadyInbox(List<SortingPackingCheckModel> lstCheckModel, List<string> lstFormCode)
        {
            //判断是否已经装箱
            StringBuilder sbAlreadyPackedFormCode = new StringBuilder();
            lstCheckModel.ForEach(m =>
            {
                if (m.IsPacked)
                {
                    sbAlreadyPackedFormCode.Append(m.FormCode).Append(",");
                }
            });
            if (sbAlreadyPackedFormCode.Length > 0)
            {
                sbAlreadyPackedFormCode.Remove(sbAlreadyPackedFormCode.Length - 1, 1);
                return string.Format("运单号:{0}已经装箱!", sbAlreadyPackedFormCode.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查是否属于该分拣中心
        /// </summary>
        /// <param name="lstCheckModel"></param>
        /// <param name="lstWaybillNo"></param>
        /// <param name="inboundData"></param>
        /// <returns></returns>
        private string CheckWaybillBelongs(List<SortingPackingCheckModel> lstCheckModel, List<string> lstFormCode, ViewInboundValidateModel inboundData)
        {
            StringBuilder sbNotBelongWaybillNo = new StringBuilder();
            lstCheckModel.ForEach(m =>
            {
                if (m.DepartureID != inboundData.OpUser.ExpressId)
                {
                    sbNotBelongWaybillNo.Append(m.FormCode).Append(",");
                }
            });
            if (sbNotBelongWaybillNo.Length > 0)
            {
                sbNotBelongWaybillNo.Remove(sbNotBelongWaybillNo.Length - 1, 1);
                return string.Format("运单号:{0}不属于当前分拣中心!", sbNotBelongWaybillNo.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查状态
        /// </summary>
        /// <param name="lstCheckModel"></param>
        /// <param name="lstWaybillNo"></param>
        /// <returns></returns>
        private string CheckWaybillStatusValid(List<SortingPackingCheckModel> lstCheckModel, List<string> lstFormCode)
        {
            StringBuilder sbStatusNotValidFormCode = new StringBuilder();
            lstCheckModel.ForEach(m =>
            {
                if (m.Status != Enums.BillStatus.HaveBeenSorting)
                {
                    sbStatusNotValidFormCode.Append(m.FormCode).Append(",");
                }
            });
            if (sbStatusNotValidFormCode.Length > 0)
            {
                sbStatusNotValidFormCode.Remove(sbStatusNotValidFormCode.Length - 1, 1);
                return string.Format("运单号:{0}状态不符合!", sbStatusNotValidFormCode.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查存在
        /// </summary>
        /// <param name="lstCheckModel"></param>
        /// <param name="lstFormCode"></param>
        /// <returns></returns>
        private string CheckFormCodeExists(List<SortingPackingCheckModel> lstCheckModel, List<string> lstFormCode)
        {
            if (lstCheckModel == null || lstCheckModel.Count == 0)
            {
                return "运单全部不存在!";
            }
            StringBuilder sbNotExistsFormCode = new StringBuilder();
            lstFormCode.ForEach(l =>
            {
                if (!lstCheckModel.Exists(m => m.FormCode == l))
                {
                    sbNotExistsFormCode.Append(l).Append(",");
                }
            });
            if (sbNotExistsFormCode.Length > 0)
            {
                sbNotExistsFormCode.Remove(sbNotExistsFormCode.Length - 1, 1);
                return string.Format("运单号:{0}不存在!", sbNotExistsFormCode.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 构建分拣装箱对象
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="count"></param>
        /// <param name="arrivalID"></param>
        /// <param name="checkModel"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private InboundPackingEntityModel BuildPackingBox(string boxNo, decimal weight, int count
                                        , int arrivalID, SortingPackingCheckModel checkModel, int userID)
        {
            InboundPackingEntityModel model = new InboundPackingEntityModel();
            model.BoxNo = boxNo;
            model.Weight = weight;
            model.BillCount = count;
            model.ArrivalID = arrivalID;
            model.DepartureID = checkModel.DepartureID;
            model.IsOutbounded = false;
            model.InboundType = checkModel.InboundType;
            model.CreateBy = userID;
            model.UpdateBy = userID;
            return model;
        }


        #region ISortingPackingBLL 成员

        /// <summary>
        /// 取得装箱打印信息
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        public SortingPackingPrintModel GetPackingPrintModel(string boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("boxNo is null or empty.");
            return _sortingPackingDAL.GetPackingPrintModel(boxNo);
        }

        public SortingPackingPrintModel GetPackingPrintModelV2(string boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("boxNo is null or empty.");
            var pbox = _sortingPackingDAL.GetPackingPrintModel(boxNo);

            var boxList = _sortingPackingDAL.GetPackingModelList(pbox.DepartureID, pbox.ArrivalID, pbox.DepartureTime);
            var boxs = from b in boxList
                       orderby b.DepartureTime ascending, b.BoxNo ascending
                       select b;

            int i = 0;
            foreach (var bx in boxs)
            {
                i++;
                if (pbox.BoxNo == bx.BoxNo)
                {
                    break;
                }
            }

            pbox.TheN = i.ToString();

            return pbox;
        }

        #endregion


        public string GetBoxNoByFormcode(string formCode)
        {
            return _sortingPackingDAL.GetBoxNoByFormcode(formCode);
        }

        private string GetBoxNo(int expressID)
        {
            return _packingNoGenerator.Execute(new InboundPackingNoContextModel() { SortingCenterID = expressID, FillerCharacter = "0", NumberLength = 6 });
        }


        public ResultModel UnLoadBox(ViewInboundValidateModel inboundData, string boxNo, List<string> delFormCodes)
        {
            //取得原始装箱订单号
            var lstPastFormCode = _sortingPackingDAL.GetBillModelByBoxNo(boxNo);
            if (lstPastFormCode == null || lstPastFormCode.Count == 0)
            {
                return ErrorResult("该箱不存在或已被删除！");
            }

            //判断待删除单号是否在原始装箱单号里
            foreach (var delfcode in delFormCodes)
            {
                if (!lstPastFormCode.Exists(p => p.FormCode == delfcode))
                {
                    return ErrorResult(string.Format("运单号{0}不在箱号为{1}的箱子里", delfcode, boxNo));
                }
            }


            try
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    //删除解除装箱的数据
                    if (delFormCodes != null && delFormCodes.Count > 0)
                    {
                        _sortingPackingDAL.BatchDeleteInboundPackingDetail(boxNo, delFormCodes);
                    }
                    //只有一个运单的箱子，把箱子本身删掉
                    if (lstPastFormCode.Count == 1)
                    {
                        _sortingPackingDAL.DeleteInboundPacking(boxNo);
                    }

                    //写操作日志
                    var listLogModel = new List<BillChangeLogDynamicModel>();
                    foreach (var delfcode in delFormCodes)
                    {
                        var item = lstPastFormCode.Find(p => p.FormCode == delfcode);
						//更新sc_bill
						_billDAL.UpdateBillStatus(item.FormCode, Enums.BillStatus.HaveBeenSorting);
						//创建LogModel
                        var logItem = new BillChangeLogDynamicModel()
                        {
                            CreateBy = inboundData.OpUser.UserId,
                            CreateDept = inboundData.OpUser.ExpressId.Value,
                            CurrentDistributionCode = inboundData.OpUser.DistributionCode,
                            CurrentSatus = Enums.BillStatus.HaveBeenSorting,
                            DeliverStationID = item.DeliverStationID,
                            FormCode = item.FormCode,
                            OperateType = Enums.TmsOperateType.Packing,
                            PreStatus = item.Status
                        };
                        logItem.ExtendedObj.BoxNo = boxNo;
                        logItem.ExtendedObj.IsAdd = false;
                        listLogModel.Add(logItem);
                    }

                    if (listLogModel.Count > 0)
                    {
                        WriteBillChangeLog_Batch(listLogModel);
                    }
                    scope.Complete();
                }
                return InfoResult("解除箱子关联成功!");
            }
            catch (Exception ex)
            {
                return ErrorResult(String.Format("解除箱子关联失败!{0}", ex.Message));
            }
        }

        public bool UpdateBoxWeightWhenPrint(string boxNo, Decimal weight, int updateBy)
        {
            return _sortingPackingDAL.UpdateBoxWeightWhenPrint(boxNo, weight, updateBy) == 1;
        }

        public bool Validate(string formCode, int expressId, bool isFirst)
        {
            SortingPackingBillModel billModel = GetSortingPackingBill(formCode);
            if (billModel == null)
            {
                _validateMsg = "该运单不存在!";
                return false;
            }

            bool isAlreadyPacked = IsBillAlreadyPacked(formCode, expressId);
            if (isAlreadyPacked)
            {
                if (isFirst)
                {
                    _validateMsg = "第一单已装箱";
                    return true;
                }

                var boxno = GetBoxNoByFormcode(formCode);
                _validateMsg = string.Format("该运单已装箱，箱号{0} ", boxno);
                return false;
            }

            if (billModel.Status != Enums.BillStatus.HaveBeenSorting)
            {
                _validateMsg = string.Format("运单状态不符合装箱条件，当前状态为【{0}】", EnumHelper.GetDescription(billModel.Status));
                return false;
            }

            return true;
        }

        private string _validateMsg;
        public string ValidateMsg
        {
            get { return _validateMsg; }
        }
    }
}
