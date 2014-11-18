using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.BLL.Transport.Dispatch
{
    public partial class PreDispatchBLL
    {
        public void PreDispatchV1(PreDispatchJobArgModel arguments)
        {
            if (arguments == null) throw new ArgumentNullException("PreDispatchJobArgModel is null");
            var boxList = _preDispatchDAL.GetNeededPreDispatchBatchList(arguments);

            CommonPreDispatchV1(boxList);
        }

        public List<ResultModel> CommonPreDispatchV1(List<BoxModel> boxList)
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
                StringBuilder sbBoxNo = new StringBuilder();
                foreach (BoxModel b in sameDABoxitem.ToList())
                {
                    sbBoxNo.Append(b.CustomerBatchNo + ",");
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

                List<long> listBID = new List<long>();
                List<PreDispatchModel> listPreDispatch = BuildPreDispatchList(sameDABoxitem.ToList(), transport, transportPlanDetail, lineplanList, out listBID);

                if (listPreDispatch.Count > 0)
                {
                    using (IACID scope = ACIDScopeFactory.GetTmsACID())
                    {
                        _preDispatchDAL.BatchAddV1(listPreDispatch);
                        _boxDAL.UpdatePreDispatched(listBID);
                        scope.Complete();
                    }
                }
            }
            return resultList;
        }

        private List<PreDispatchModel> BuildPreDispatchList(List<BoxModel> sameDABoxitem,
            PreDispatchGetTransportResultModel transport,
            List<TransportPlanDetailModel> transportPlanDetail,
            List<LinePlanModel> lineplanList, out List<long> listBID)
        {
            List<PreDispatchModel> listPreDispatch = new List<PreDispatchModel>();
            listBID = new List<long>();
            foreach (var item in sameDABoxitem)
            {
                listBID.Add(item.BID);
                if (transport.TransportPlan.IsTransit)
                {
                    int n = 0;
                    foreach (var planDetail in transportPlanDetail)
                    {
                        listPreDispatch.Add(new PreDispatchModel()
                        {
                            ArrivalID = lineplanList[n].ArrivalID,
                            BoxNo = item.BoxNo,
                            CreateBy = UserContext.CurrentUser.ID,
                            DepartureID = lineplanList[n].DepartureID,
                            DispatchStatus = n == 0 ? Model.Common.Enums.DispatchStatus.CanDispatch : Model.Common.Enums.DispatchStatus.CanNotDispatch,
                            IsDeleted = false,
                            LineGoodsType = lineplanList[n].LineGoodsType,
                            TPID = transport.TransportPlan.TPID,
                            LPID = lineplanList[n].LPID,
                            SeqNo = planDetail.SeqNo,
                            UpdateBy = UserContext.CurrentUser.ID
                        });
                        n++;
                    }
                }
                else
                {
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
                }
            }
            return listPreDispatch;
        }

        public List<ViewDispatchBoxModel> GetPreDispatchBoxListV1(int LPID)
        {
            return _preDispatchDAL.GetPreDispatchBoxListV1(LPID);
        }
    }
}
