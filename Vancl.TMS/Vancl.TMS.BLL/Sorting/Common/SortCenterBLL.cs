using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Sorting.Common;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IBLL.SMS;
using System.Text.RegularExpressions;
using Vancl.TMS.IDAL.Sorting.BillPrint;

namespace Vancl.TMS.BLL.Sorting.Common
{
    public class SortCenterBLL : BaseBLL, ISortCenterBLL
    {
        #region 服务接口
        /// <summary>
        /// 系统单号最大长度
        /// </summary>
        private static readonly int SysBillNOMaxLength = 14;
        /// <summary>
        /// 表单主信息业务服务
        /// </summary>
        protected IBillBLL BillBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        /// <summary>
        /// 表单扩展信息业务服务
        /// </summary>
        protected IBillInfoDAL BillInfoDal = ServiceFactory.GetService<IBillInfoDAL>("BillInfoDAL");
        /// <summary>
        /// 分拣通用数据层服务
        /// </summary>
        protected ISortCenterDAL SortCenterDAL = ServiceFactory.GetService<ISortCenterDAL>("SortCenterDAL");
        /// <summary>
        /// KeyCode产生算法
        /// </summary>
        protected IFormula<String, KeyCodeContextModel> KeyCodeGenerator = FormulasFactory.GetFormula<IFormula<String, KeyCodeContextModel>>("keycodeBLLFormula");
        /// <summary>
        /// 配送商相关服务
        /// </summary>
        protected IDistributionBLL DistributionBLL = ServiceFactory.GetService<IDistributionBLL>();
        /// <summary>
        /// 组织架构服务
        /// </summary>
        protected IExpressCompanyBLL ExpressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        /// <summary>
        /// 短信服务
        /// </summary>
        protected ISMSSender SMSSender = ServiceFactory.GetService<ISMSSender>("SMSSender"); 

        #endregion

        /// <summary>
        /// 验证输入的单号,并返回系统主单号
        /// </summary>
        /// <param name="FormType">单号类型</param>
        /// <param name="InputCode">输入的单号</param>
        /// <param name="FormCode">系统主单号</param>
        /// <returns>验证结果</returns>
        protected virtual ResultModel ValidateFormCode(Enums.SortCenterFormType FormType, String InputCode, out String FormCode)
        {
            ResultModel Result = new ResultModel();
            FormCode = null;
            if (String.IsNullOrWhiteSpace(InputCode))
            {
                return Result.Failed("输入的单号为空.");
            }
            Regex reg = new Regex(@"[']");
            if (reg.IsMatch(InputCode))
            {
                return Result.Failed("输入的单号包含非法字符 ' 号");
            }
            if (FormType == Enums.SortCenterFormType.Waybill)
            {
                if (InputCode.Length > SysBillNOMaxLength)
                {
                    return Result.Failed("系统运单号超出规则，长度14.");
                }
                long tmpFormCode = 0;
                if (!long.TryParse(InputCode, out tmpFormCode))
                {
                    return Result.Failed("系统运单号不正确.");
                }
                FormCode = tmpFormCode.ToString();
            }
            if (FormType == Enums.SortCenterFormType.Order)
            {
                var listFormCode = BillBLL.GetFormCodeByCustomerOrder(InputCode);
                if (listFormCode == null)
                {
                    return Result.Failed("订单号不存在.");
                }
                if (listFormCode.Count <= 0)
                {
                    return Result.Failed("订单号不存在.");
                }
                if (listFormCode.Count > 1)
                {
                    return Result.Failed("订单号对应多个运单.");
                }
                FormCode = listFormCode.First();
            }
            if (FormType == Enums.SortCenterFormType.DeliverCode)
            {
                var deliverCodes = BillBLL.GetMerchantFormCodeRelation(Enums.SortCenterFormType.DeliverCode, InputCode);
                if (deliverCodes == null)
                {
                    return Result.Failed("配送单号不存在.");
                }
                if (deliverCodes.Count <= 0)
                {
                    return Result.Failed("配送单号不存在.");
                }
                if (deliverCodes.Count > 1)
                {
                    return Result.Failed("配送单号对应多个运单.");
                }
                FormCode = deliverCodes.First().FormCode;
            }
            //统一使用系统运单号作为系统操作单据
            return Result.Succeed();
        }

        /// <summary>
        /// 验证输入的单号列表，并返回验证通过的系统单号列表
        /// </summary>
        /// <param name="FormType">单号类型</param>
        /// <param name="arrOrigInputCode">输入的单号集合</param>
        /// <param name="errorlist">错误的信息</param>
        /// <returns>验证通过的系统单号列表</returns>
        protected virtual List<String> ValidateFormCode(Enums.SortCenterFormType FormType,String[] arrOrigInputCode,out List<SortCenterBatchErrorModel> errorlist)
        {
            errorlist = new List<SortCenterBatchErrorModel>();
            List<String> validatedFormCodeList = new List<string>();
            if (arrOrigInputCode != null && arrOrigInputCode.Length > 0)
            {
                ResultModel result = null;
                ///系统运单号
                String sysFormCode = "";
                foreach (var item in arrOrigInputCode)
                {
                    //TODO:待考虑适合批量的更高效的验证
                    result = ValidateFormCode(FormType, item, out sysFormCode);
                    if (result.IsSuccess)
                    {
                        if (!validatedFormCodeList.Contains(sysFormCode))
                        {
                            validatedFormCodeList.Add(sysFormCode);
                            continue;
                        }
                        errorlist.Add(new SortCenterBatchErrorModel() { WaybillNo = sysFormCode, CustomerOrder = item, ErrorMsg = "单号重复"});
                    }
                    else
                    {
                        errorlist.Add(new SortCenterBatchErrorModel() { WaybillNo = item, CustomerOrder = item, ErrorMsg = result.Message });
                    }
                }
                return validatedFormCodeList;
            }
            return null;
        }


        /// <summary>
        /// 是否走城际运输[check]
        /// </summary>
        /// <param name="departureID"></param>
        /// <param name="arrivalID"></param>
        protected virtual bool IsNeedTMSTransfer(int departureID, int arrivalID)
        {
            try
            {
                bool val = false;
                using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
                {
                    val = client.IsNeedTmsTransfer(departureID, ref arrivalID) > 0;
                }
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 是否走城际运输[实际操作出库到TMS]
        /// </summary>
        /// <param name="departureID"></param>
        /// <param name="arrivalID"></param>
        protected virtual bool IsNeedTMSTransfer(int departureID, int arrivalID, out int TMSArrivalID)
        {
            try
            {
                bool val = false;
                using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
                {
                    val = client.IsNeedTmsTransfer(departureID, ref arrivalID) > 0;
                    TMSArrivalID = arrivalID;
                }
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过配送站ID得到二级分拣中心ID
        /// </summary>
        protected virtual int[] GetSecondSortCenterID(int ExpressId)
        {
            try
            {
                int[] val;
                using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
                {
                    val = client.GetSortingCenterID(ExpressId);
                    
                }
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通过配送站ID得到已配置运输关系的二级分拣中心ID
        /// </summary>
        protected virtual int GetSecondSortCenterIDByCityLine(int ExpressId)
        {
            try
            {
                int val;
                using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
                {
                    val = client.GetDeliverCenterByStation(ExpressId);

                }
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region ISortCenterBLL 成员

        /// <summary>
        /// 取得分拣用户对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public SortCenterUserModel GetUserModel(int UserID)
        {
            if (UserID <= 0) throw new ArgumentException("UserID must > 0");
            return SortCenterDAL.GetUserModel(UserID);
        }

        /// <summary>
        /// 取得目的地对象
        /// </summary>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        public SortCenterToStationModel GetToStationModel(int ArrivalID)
        {
            if (ArrivalID <= 0) throw new ArgumentException("ArrivalID must > 0");
            return SortCenterDAL.GetToStationModel(ArrivalID);
        }

        #endregion
    }
}
