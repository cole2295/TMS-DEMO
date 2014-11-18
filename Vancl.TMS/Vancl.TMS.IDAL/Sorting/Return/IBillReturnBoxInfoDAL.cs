using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.IDAL.Sorting.Return
{

    /// <summary>
    /// 退货分拣数据接口层
    /// </summary>
    public interface IBillReturnBoxInfoDAL
    {
        /// <summary>
        /// 添加扫描箱号信息
        /// </summary>
        /// <param name="model">单实体对象</param>
        /// <returns></returns>
        int Add(BillReturnBoxInfoModel model);
         /// <summary>
        /// 根据箱号获取箱号信息
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillReturnBoxInfoModel GetBoxInfoByBoxNo(string BoxNo);
        /// <summary>
        /// 更新同步标识[TMS到LMS]
        /// </summary>
        /// <param name="outboundKey">Key</param>
        /// <returns></returns>
        int UpdateSyncedStatus4Tms2Lms(String BillReturnBoxKey);
        /// <summary>
        /// 更新装箱称重重量
        /// </summary>
        /// <param name="boxNos"></param>
        /// <returns></returns>
        int UpdateBoxWeight(decimal Weight,string BoxNo,int UpdateBy);
        /// <summary>
        /// 获取最大的箱号
        /// </summary>
        /// <param name="boxNoHead"></param>
        /// <returns></returns>
        string GetMaxBoxNO(string boxNoHead);
        /// <summary>
        /// 是否已经装箱打印
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool IsInBoxPrint(string BoxNo);
        /// <summary>
        /// 条件查询已经退货出库的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        List<BillReturnBoxInfoModel> GetReturnDetailList(string returnto, string startDate, string endDate, string formType, string code, string currentDeptName);
        /// <summary>
        /// 更新箱号状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        int UpdateBoxStatus(string BoxNo, Enums.ReturnStatus status);
        /// <summary>
        /// 更新装箱打印状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool UpdateIsPrintBackPacking(string BoxNo,decimal Weight);
        /// <summary>
        /// 更新箱号交接表打印状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool UpdateBoxIsPrintBackForm(string BoxNo);
        /// <summary>
        /// 取得TMS退货分拣称重箱号记录需要同步到LMS物流主库的出库实体对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillReturnBoxInfoModel GetBillReturnBoxIntoModel4TmsSync2Lms(String BoxNo);

        /// <summary>
        /// 是否已经在该部门装箱称重
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool IsInBoxWeight(string BoxNo, int CreateBy);
        /// <summary>
        /// 查看箱号的当前操作地
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        DataTable GetCreateDept(string BoxNo);
    }
}
