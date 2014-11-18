using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Transport.Dispatch;
using System.Data;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Transport.DeliveryAbnormal;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.Plan;

namespace Vancl.TMS.BLL.Transport.Dispatch
{
    public partial class PreDispatchBLL : BaseBLL, IPreDispatchBLL
    {
        IFormula<PreDispatchGetTransportResultModel, PreDispatchGetTransportContext> transportformula = FormulasFactory.GetFormula<IFormula<PreDispatchGetTransportResultModel, PreDispatchGetTransportContext>>("PreDispatchGetTransportFormula");
        IFormula<PreDispatchGetLineResultModel, PreDispatchGetLineContext> lineplanformula = FormulasFactory.GetFormula<IFormula<PreDispatchGetLineResultModel, PreDispatchGetLineContext>>("PreDispatchGetLineFormula");

        IPreDispatchDAL _preDispatchDAL = ServiceFactory.GetService<IPreDispatchDAL>("PreDispatchDAL");
        IBoxDAL _boxDAL = ServiceFactory.GetService<IBoxDAL>("BoxDAL");
        #region IPreDispatchBLL 成员

        /// <summary>
        /// 写预调度异常日志
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="strMsg"></param>
        private void WritePreDispatchExceptLog(List<BoxModel> listBox, String strMsg)
        {
            var listExceptLog = new List<PreDispatchLogEntityModel>(listBox.Count);
            var listBID = new List<long>(listBox.Count);
            foreach (var item in listBox)
            {
                listBID.Add(item.BID);
                listExceptLog.Add(new PreDispatchLogEntityModel() { ArrivalID = item.ArrivalID, BatchNo = item.BoxNo, CustomerBatchNo = item.CustomerBatchNo, DepartureID = item.DepartureID, Note = strMsg });
            }
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                _preDispatchDAL.BatchAddPreDispatchLog(listExceptLog);
                _boxDAL.UpdatePreDispatchedError(listBID);
                scope.Complete();
            }
        }

        public BoxModel GetAbnormalPreDispatchByBID(Int64 bid)
        {
            if (bid == null) throw new ArgumentNullException("bid is null");

            return _preDispatchDAL.GetAbnormalPreDispatchByBID(bid);
        }

        public void PreDispatch(PreDispatchJobArgModel arguments)
        {
            if (arguments == null) throw new ArgumentNullException("PreDispatchJobArgModel is null");
            var boxList = _preDispatchDAL.GetNeededPreDispatchBatchList(arguments);

            CommonPreDispatch(boxList);
        }

        public List<ResultModel> CommonPreDispatch(List<BoxModel> boxList)
        {
            if (boxList == null || boxList.Count <= 0)
            {
                return new List<ResultModel>() { new ResultModel { IsSuccess = false, Message = "没有找到数据" } };
            }
            List<ResultModel> resultList = new List<ResultModel>();
            //相同出发地，目的地，货物属性 归类
            var groupedList = boxList.GroupBy(p => new { p.DepartureID, p.ArrivalID, p.ContentType });
            foreach (var sameDABoxitem in groupedList)
            {
                var transport = transportformula.Execute(new PreDispatchGetTransportContext() { DepartureID = sameDABoxitem.Key.DepartureID, ArrivalID = sameDABoxitem.Key.ArrivalID, ContentType = sameDABoxitem.Key.ContentType });
                StringBuilder sbBoxNo=new StringBuilder();
                foreach(BoxModel b in sameDABoxitem.ToList())
                {
                    sbBoxNo.Append(b.BoxNo+",");
                }
                //未找到运输计划，无法进行正常的预调度
                if (!transport.IsSuccess)
                {
                    WritePreDispatchExceptLog(sameDABoxitem.ToList(), transport.Message);
                    resultList.Add(new ResultModel() { IsSuccess = false, Message = sbBoxNo.ToString().TrimEnd(',') + ":" + transport.Message });
                    continue;
                }
                var lineplanList = new List<LinePlanModel>();
                var transportPlanDetail = transport.TransportPlanDetail.OrderBy(p => p.SeqNo).ToList();
                foreach (var item in transportPlanDetail)
                {
                    var lineplan = lineplanformula.Execute(new PreDispatchGetLineContext() { LineID = item.LineID });
                    if (lineplan.IsSuccess)
                    {
                        lineplanList.Add(lineplan.LinePlan);
                    }
                    else
                    {
                        WritePreDispatchExceptLog(sameDABoxitem.ToList(), lineplan.Message);
                        resultList.Add(new ResultModel() { IsSuccess = false, Message = sbBoxNo.ToString().TrimEnd(',') + ":" + lineplan.Message });
                        break;
                    }
                }
                //有一些线路未找到相应的线路计划，无法进行正常的预调度
                if (lineplanList.Count != transportPlanDetail.Count || lineplanList.Count <= 0)
                {
                    resultList.Add(new ResultModel() { IsSuccess = false, Message = sbBoxNo.ToString().TrimEnd(',') + ":线路未找到相应的线路计划，无法进行正常的预调度" });
                    continue;
                }
                List<PreDispatchModel> listPreDispatch = new List<PreDispatchModel>();
                List<long> listBID = new List<long>();
                foreach (var item in sameDABoxitem)
                {
                    listBID.Add(item.BID);
                    listPreDispatch.Add(new PreDispatchModel()
                    {
                        ArrivalID = item.ArrivalID,
                        BoxNo = item.BoxNo,
                        CreateBy = UserContext.CurrentUser.ID,
                        DepartureID = item.DepartureID,
                        DispatchStatus = Model.Common.Enums.DispatchStatus.CanDispatch,
                        IsDeleted = false,
                        LineGoodsType = lineplanList[0].LineGoodsType,
                        TPID = transport.TransportPlan.TPID,
                        LPID = lineplanList[0].LPID,
                        SeqNo = transportPlanDetail[0].SeqNo,
                        UpdateBy = UserContext.CurrentUser.ID
                    });
                    if (transport.TransportPlan.IsTransit)
                    {
                        listPreDispatch.Add(new PreDispatchModel()
                        {
                            ArrivalID = item.ArrivalID,
                            BoxNo = item.BoxNo,
                            CreateBy = UserContext.CurrentUser.ID,
                            DepartureID = transport.TransportPlan.TransferStation.Value,
                            DispatchStatus = Model.Common.Enums.DispatchStatus.CanDispatch,
                            IsDeleted = false,
                            LineGoodsType = lineplanList[1].LineGoodsType,
                            TPID = transport.TransportPlan.TPID,
                            LPID = lineplanList[1].LPID,
                            SeqNo = transportPlanDetail[1].SeqNo,
                            UpdateBy = UserContext.CurrentUser.ID
                        });
                    }
                }
                if (listPreDispatch.Count > 0)
                {
                    using (IACID scope = ACIDScopeFactory.GetTmsACID())
                    {
                        _preDispatchDAL.BatchAdd(listPreDispatch);
                        _boxDAL.UpdatePreDispatched(listBID);
                        scope.Complete();
                    }
                }
            }
            return resultList;
        }

        public int PreDispatch(int count)
        {
            //获得调度的箱号和运输计划ID号
            DataTable dt = _preDispatchDAL.GetValidBoxNoAndTPID(count);
            if (dt == null || dt.Rows.Count == 0)
            {
                return 0;
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                string boxNos = "'" + string.Join("','", dt.AsEnumerable().Select(m => m.Field<string>("BoxNo"))) + "'";
                //添加到预调度表中
                i = _preDispatchDAL.Add(boxNos, Convert.ToInt32(dt.Rows[0]["TPID"]));
                //更新箱表状态
                _boxDAL.UpdatePreDispatched(boxNos);
                scope.Complete();
            }
            return i;
        }

        public List<ViewDispatchBoxModel> GetPreDispatchBoxList(int LPID)
        {
            return _preDispatchDAL.GetPreDispatchBoxList(LPID);
        }

        /// <summary>
        /// 更新城际批次为待预调度状态
        /// </summary>
        /// <param name="box">城际批次对象</param>
        /// <returns></returns>
        public int UpdateBoxToWaitforDispatch(BoxModel box)
        {
            if (box == null) throw new ArgumentNullException("BoxModel is null");
            return _preDispatchDAL.UpdateBoxToWaitforDispatch(box);
        }

        /// <summary>
        /// 批量更新城际批次为待预调度状态
        /// </summary>
        /// <param name="listbox">城际批次对象</param>
        /// <returns></returns>
        public int BatchUpdateUpdateBoxToWaitforDispatch(List<BoxModel> listbox)
        {
            if (listbox == null) throw new ArgumentNullException("listbox is null");
            return _preDispatchDAL.BatchUpdateUpdateBoxToWaitforDispatch(listbox);
        }

        public PagedList<PreDispatchAbnormalModel> GetPreDispatchAbnormalList(PreDispatchAbnormalSearchModel searchModel)
        {
            return _boxDAL.GetPreDispatchAbnormalList(searchModel);
        }
        #endregion
    }
}
