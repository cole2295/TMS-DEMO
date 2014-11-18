using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Synchronous;
using System.Data;
using Vancl.TMS.Util.ClsExtender;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous
{
    public class InboundBLL : BaseBLL, IInboundBLL
    {
        IInboundLMSDAL _lmsDAL = ServiceFactory.GetService<IInboundLMSDAL>("InboundLMSDAL");
        IInboundTMSDAL _tmsDAL = ServiceFactory.GetService<IInboundTMSDAL>("InboundTMSDAL");
        #region IInboundBLL 成员

        public InboundModel GetInboundBox(int mod, int remainder)
        {
            DataSet ds = _lmsDAL.GetInboundBox(mod, remainder);
            if (ds.IsNull())
            {
                return null;
            }
            InboundModel model = new InboundModel();
            DataRow dr = ds.Tables[0].Rows[0];
            model.ID = long.Parse(dr["ID"].ToString());
            model.InboundID = int.Parse(dr["InboundID"].ToString());
            model.BoxNo = dr["BoxNo"].ToString();
            model.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
            model.IsFCL = true;
            return model;
        }

        public List<InboundModel> GetInboundOrder(int count, int mod, int remainder)
        {
            DataSet ds = _lmsDAL.GetInboundOrder(count, mod, remainder);
            if (ds.IsNull())
            {
                return null;
            }
            List<InboundModel> list = new List<InboundModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                InboundModel model = new InboundModel();
                model.ID = long.Parse(dr["ID"].ToString());
                model.InboundID = int.Parse(dr["InboundID"].ToString());
                model.CustomerOrder = dr["CustomerOrder"].ToString();
                model.WaybillNo = long.Parse(dr["WaybillNo"].ToString());
                model.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                model.IsFCL = false;
                list.Add(model);
            }
            return list;
        }

        public int UpdateBoxSyncFlag(long id, Enums.SC2TMSSyncFlag syncFlag)
        {
            return _lmsDAL.UpdateBoxSyncFlag(id, syncFlag);
        }

        public int UpdateOrderSyncFlag(string ids, Enums.SC2TMSSyncFlag syncFlag)
        {
            return _lmsDAL.UpdateOrderSyncFlag(ids, syncFlag);
        }

        public int UpdateTMS(List<InboundModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return 0;
            }
            int i = 0;
            if (list[0].IsFCL)
            {
                //整箱入库的情况
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    i = i + UpdateIsArrivedStatusByBoxNo(list[0].BoxNo, list[0].InboundID);
                    i = i + UpdateDispatchByBoxNo(list[0].BoxNo, list[0].InboundID, list[0].CreateTime);
                    //i = i + UpdateOrderTMSStatusByBoxNo(list[0].BoxNo, list[0].InboundID);
                    scope.Complete();
                }
            }
            else
            {
                //订单入库的情况
                string[] fomCodes = list.Select(c => c.WaybillNo.ToString()).ToArray<string>();
                int[] arrivalIDs = list.Select(c => c.InboundID).ToArray<int>();
                DateTime[] receiveDates = list.Select(c => c.CreateTime).ToArray<DateTime>();
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    i = i + UpdateIsArrivedStatusByFormCodes(fomCodes, arrivalIDs);
                    i = i + UpdateDispatchByFormCodes(fomCodes, arrivalIDs, receiveDates);
                    //i = i + UpdateOrderTMSStatusByFormCodes(fomCodes, arrivalIDs);
                    scope.Complete();
                }
            }
            return i;
        }

        #endregion

        /// <summary>
        /// 根据箱号更新分拣中心接受状态
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="arrivalID"></param>
        /// <returns></returns>
        private int UpdateIsArrivedStatusByBoxNo(string boxNo, int arrivalID)
        {
            return _tmsDAL.UpdateIsArrivedStatusByBoxNo(boxNo, arrivalID);
        }

        /// <summary>
        /// 根据单号更新分拣中心接受状态
        /// </summary>
        /// <param name="formCodes"></param>
        /// <param name="arrivalIDs"></param>
        /// <returns></returns>
        private int UpdateIsArrivedStatusByFormCodes(string[] formCodes, int[] arrivalIDs)
        {
            return _tmsDAL.UpdateIsArrivedStatusByFormCodes(formCodes, arrivalIDs);
        }

        /// <summary>
        /// 根据箱号更新调度表
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="arrivalID"></param>
        /// <param name="receiveDate"></param>
        /// <returns></returns>
        private int UpdateDispatchByBoxNo(string boxNo, int arrivalID, DateTime receiveDate)
        {
            return _tmsDAL.UpdateDispatchByBoxNo(boxNo, arrivalID, receiveDate);
        }

        /// <summary>
        /// 根据单号更新调度表
        /// </summary>
        /// <param name="formCodes"></param>
        /// <param name="arrivalIDs"></param>
        /// <param name="receiveDates"></param>
        /// <returns></returns>
        private int UpdateDispatchByFormCodes(string[] formCodes, int[] arrivalIDs, DateTime[] receiveDates)
        {
            return _tmsDAL.UpdateDispatchByFormCodes(formCodes, arrivalIDs, receiveDates);
        }

        /// <summary>
        /// 根据箱号更新到库状态
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="arrivalID"></param>
        /// <returns></returns>
        private int UpdateOrderTMSStatusByBoxNo(string boxNo, int arrivalID)
        {
            return _tmsDAL.UpdateOrderTMSStatusByBoxNo(boxNo, arrivalID);
        }

        /// <summary>
        /// 根据单号更新到库状态
        /// </summary>
        /// <param name="formCodes"></param>
        /// <param name="arrivalIDs"></param>
        /// <returns></returns>
        private int UpdateOrderTMSStatusByFormCodes(string[] formCodes, int[] arrivalIDs)
        {
            return _tmsDAL.UpdateOrderTMSStatusByFormCodes(formCodes, arrivalIDs);
        }
    }
}
