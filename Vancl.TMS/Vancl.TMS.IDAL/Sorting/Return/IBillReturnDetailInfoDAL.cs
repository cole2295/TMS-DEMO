using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Sorting.Return
{
    /// <summary>
    /// 退货分拣单数据层
    /// </summary>
    public interface IBillReturnDetailInfoDAL
    {
        /// <summary>
        /// 添加扫描单
        /// </summary>
        /// <param name="model">单实体对象</param>
        /// <returns></returns>
        int Add(BillReturnDetailInfoModel model);

        /// <summary>
        /// 判断运单是否已经扫描退货出库
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        bool isExist(string FormCode);
        /// <summary>
        /// 判断运单是否已经退货分拣称重至分拣中心
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        bool IsReturn(string FormCode, string returnTo, out string returnBoxNo);
        /// <summary>
        /// 更新同步标识[TMS到LMS]
        /// </summary>
        /// <param name="outboundKey">Key</param>
        /// <returns></returns>
        int UpdateSyncedStatus4Tms2Lms(String BillReturnDetailKey);
        /// <summary>
        /// 获取已经退货装箱的箱号
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        string GetBoxNO(string formCode, string returnTo);
        /// <summary>
        /// 根据运单号获取运单信息列表
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> GetListByFormCodes(string FormCodes);

        /// <summary>
        /// 根据箱号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> GetListByBoxNo(string boxNo, string createdept);
        /// <summary>
        /// 根据箱号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnDetailInfoModel> GetListByBoxNo(string boxNo);
        /// <summary>
        /// 获取箱号信息用于退货交接表打印
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<ReturnBillSortingDetailModel> GetDetailListByBoxNo(string boxNo);
        /// <summary>
        /// 删除运单
        /// </summary>
        /// <param name="FormCodeLists"></param>
        /// <returns></returns>
        int Delete(string FormCodeLists);

        /// <summary>
        /// 装箱打印操作
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool InBoxPrint(string BoxNo);
        /// <summary>
        /// 取得TMS退货分拣称重单号记录需要同步到LMS物流主库的出库实体对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillReturnDetailInfoModel GetBillReturnDetailIntoModel4TmsSync2Lms(String FormCode);
        /// <summary>
        /// 重新设置未同步标志
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        bool UpdateSyncedStatus(string FormCode);
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
        /// <summary>
        /// 判断是否已经同步完成
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        bool IsSynced(string FormCode);


    }
}
