using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.Return;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using System.Data;

namespace Vancl.TMS.BLL.Sorting.Return
{
    /// <summary>
    /// 退货业务实现
    /// </summary>
    public class BillReturnBoxInfoBLL : SortCenterBLL, IBillReturnBoxInfoBLL
    {

        IBillReturnBoxInfoDAL BillReturnBoxInfoDAL = ServiceFactory.GetService<IBillReturnBoxInfoDAL>();
        /// <summary>
        /// 添加一条箱号信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddBox(BillReturnBoxInfoModel model)
        {
            int BoxID = -1;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    var logModel = new BillChangeLogDynamicModel
                    {
                        CreateBy = model.CreateBy,
                        CreateDept = model.CreateDeptID,
                        CurrentDistributionCode = model.CurrentDistributionCode,
                        CurrentSatus = Enums.BillStatus.ReturnInBound,
                        ReturnStatus = Enums.ReturnStatus.ReturnInbounded,
                        DeliverStationID = -1,
                        FormCode = model.BoxNo,
                        OperateType = Enums.TmsOperateType.BillReturnBox,
                        PreStatus = Enums.BillStatus.ReturnInBound,
                    };
                    WriteBillChangeLog(logModel);
                    BoxID = BillReturnBoxInfoDAL.Add(model);
                    scope.Complete();
                    return BoxID;
                }
                catch (Exception ex)
                {
                    throw new Exception("更单号的逆向状态失败", ex);
                }
            }
        }
        /// <summary>
        /// 根据箱号获取箱号信息
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public BillReturnBoxInfoModel GetBoxInfoByBoxNo(string BoxNo)
        {
            return BillReturnBoxInfoDAL.GetBoxInfoByBoxNo(BoxNo);
        }

        /// <summary>
        /// 更新装箱称重重量
        /// </summary>
        /// <param name="boxNos"></param>
        /// <returns></returns>
        public int UpdateBoxWeight(decimal Weight, string BoxNo, int UpdateBy)
        {
            if (string.IsNullOrEmpty(BoxNo)) return -1;
            var billBox = BillReturnBoxInfoDAL.GetBoxInfoByBoxNo(BoxNo);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    if (billBox != null)
                    {
                        var log = new BillChangeLogDynamicModel
                        {
                            CreateBy = billBox.CreateBy,
                            CreateDept = billBox.CreateDeptID,
                            CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                            DeliverStationID = -1,
                            FormCode = billBox.BoxNo,
                            OperateType = Enums.TmsOperateType.BillReturnBox,
                            PreStatus = Enums.BillStatus.ReturnInBound,
                        };
                        if (BillReturnBoxInfoDAL.UpdateBoxWeight(Weight, BoxNo, UpdateBy) > 0)
                        {
                            log.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                            WriteBillChangeLog(log);
                        }
                        scope.Complete();
                        return 1;
                    }
                    return -1;
                }
                catch (Exception ex)
                {
                    return -1;
                    throw new Exception("箱号称重失败", ex);
                }
            }
        }


        /// <summary>
        /// 是否已经装箱打印
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool IsInBoxPrint(string BoxNo)
        {
            return BillReturnBoxInfoDAL.IsInBoxPrint(BoxNo);
        }
        /// <summary>
        /// 条件查询已经退货出库的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnBoxInfoModel> GetReturnDetailList(string returnto, string startDate, string endDate, string formType, string code, string currentDeptName)
        {
            return BillReturnBoxInfoDAL.GetReturnDetailList(returnto, startDate, endDate, formType, code, currentDeptName);
        }

        /// <summary>
        /// 更新箱号状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateBoxStatus(string BoxNo, Enums.ReturnStatus status)
        {
            return BillReturnBoxInfoDAL.UpdateBoxStatus(BoxNo, status) > 0;
        }
        /// <summary>
        /// 更新箱号交接表打印状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool UpdateBoxIsPrintBackForm(string BoxNo)
        {
            if (string.IsNullOrEmpty(BoxNo)) return false;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    BillReturnBoxInfoModel model = BillReturnBoxInfoDAL.GetBoxInfoByBoxNo(BoxNo);
                    var logModel = new BillChangeLogDynamicModel
                    {
                        CreateBy =UserContext.CurrentUser.ID,
                        CreateDept = UserContext.CurrentUser.DeptID,
                        CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                        CurrentSatus = Enums.BillStatus.ReturnInBound,
                        ReturnStatus =Enums.ReturnStatus.ReturnInbounded,
                        DeliverStationID = -1,
                        FormCode = BoxNo,
                        OperateType = Enums.TmsOperateType.ReturnBoxPrintBackForm,
                        PreStatus = Enums.BillStatus.ReturnInBound,

                    };
                    if (BillReturnBoxInfoDAL.UpdateBoxIsPrintBackForm(BoxNo))
                    {
                        logModel.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                    }
                    WriteBillChangeLog(logModel);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception("打印交接表失败", ex);
                }
                return true;
            }

        }
        /// <summary>
        /// 是否已经在该部门装箱称重
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool IsInBoxWeight(string BoxNo, int CreateBy)
        {
            return BillReturnBoxInfoDAL.IsInBoxWeight(BoxNo, CreateBy);
        }

        /// <summary>
        /// 查看箱号的当前操作地
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public DataTable GetCreateDept(string BoxNo)
        {
            return BillReturnBoxInfoDAL.GetCreateDept(BoxNo);
        }

    }
}
