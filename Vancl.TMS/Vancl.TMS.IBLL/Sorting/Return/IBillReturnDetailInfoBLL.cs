using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Return;

namespace Vancl.TMS.IBLL.Sorting.Return
{
    /// <summary>
    /// 退货单信息业务接口
    /// </summary>
    public interface IBillReturnDetailInfoBLL : ISortCenterBLL
    {
        /// <summary>
        /// 添加一条扫描单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddDetail(BillReturnDetailInfoModel model);
        /// <summary>
        /// 根据系统运单号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> GetListByFormCodes(string FormCodes);

        /// <summary>
        /// 根据箱号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> GetListByBoxNo(string boxNo);
        /// <summary>
        /// 根据箱号获取已经扫描的单号信息并更新ReturnStatus
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> ScanBoxNo(string boxNo, string createDept,string selectstationName);
        /// <summary>
        /// 获取箱号信息用于退货交接表打印
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<ReturnBillSortingDetailModel> GetDetailListByBoxNo(string boxNo);
        /// <summary>
        /// 判断运单是否已经退货分拣称重
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        bool IsReturn(string FormCode);
        /// <summary>
        /// 判断运单是否已经退货分拣称重至分拣中心
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        bool IsReturn(string FormCode,string returnTo,out string returnBoxNo);
        /// <summary>
        /// 获取已经退货装箱的箱号
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        string GetBoxNO(string formCode,string returnTo);
        /// <summary>
        /// 删除运单
        /// </summary>
        /// <param name="FormCodeLists"></param>
        /// <returns></returns>
        int Delete(string FormCodeLists);
        /// <summary>
        /// 查看箱中运单是否返货到同一商家或配送商
        /// </summary>
        /// <param name="returnBoxNo"></param>
        /// <returns></returns>
        BillReturnCountModel GetReturnToCount(string returnBoxNo);
        /// <summary>
        /// 查询退货出库后的剩余订单
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> GetBillAfterOutBound(string FormCodes, string CreateDept);
    }
}
