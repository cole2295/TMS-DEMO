using System.Collections.Generic;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using System.Transactions;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Carrier;
using System;
using System.Linq;
using Vancl.TMS.IDAL.BaseInfo.Carrier;
using Vancl.TMS.BLL.Formula.Common;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.IBLL.Log;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class CarrierBLL : BaseBLL, ICarrierBLL, IOperateLogBLL
    {
        ICarrierDAL _carrierDAL = ServiceFactory.GetService<ICarrierDAL>("CarrierDAL");
        IDelayCriteriaDAL _delayCriteriaDAL = ServiceFactory.GetService<IDelayCriteriaDAL>("DelayCriteriaDAL");
        ICoverageDAL _coverageDAL = ServiceFactory.GetService<ICoverageDAL>("CoverageDAL");
        IUserContextBLL userContextService = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");
        #region ICarriersBLL 成员

        /// <summary>
        /// 新增承运商
        /// </summary>
        /// <param name="carrier">承运商</param>
        /// <param name="delayCriteriaList">延误审核标准</param>
        /// <param name="coverageList">覆盖范围</param>
        /// <returns>操作结果模型</returns>
        public ResultModel Add(CarrierModel carrier, IList<DelayCriteriaModel> delayCriteriaList, IList<CoverageModel> coverageList)
        {
            if (carrier == null) throw new ArgumentNullException("carrier");
            if (IsExistCarrier(carrier.CarrierName, carrier.CarrierID, false))
            {
                return ErrorResult("已存在相同名称的承运商！");
            }
            if (IsExistCarrier(carrier.CarrierAllName, carrier.CarrierID, true))
            {
                return ErrorResult("已存在相同全称的承运商！");
            }
            if (IsExistEmail(carrier.Email, carrier.CarrierID))
            {
                return ErrorResult("已存在相同的邮件地址！");
            }
            if (string.IsNullOrWhiteSpace(carrier.CarrierNo))
            {
                carrier.CarrierNo = GetCarrierNo(carrier.IsAllCoverage, coverageList, carrier.CarrierNo);
            }
            carrier.CarrierID = Convert.ToInt32(_carrierDAL.GetNextSequence(carrier.SequenceName));
            userContextService.AddCarrier(carrier);
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _carrierDAL.Add(carrier);
                WriteInsertLog<CarrierModel>(carrier);
                if (delayCriteriaList != null && delayCriteriaList.Count > 0)
                {
                    foreach (DelayCriteriaModel delayCriteria in delayCriteriaList)
                    {
                        delayCriteria.CarrierID = carrier.CarrierID;
                        _delayCriteriaDAL.Add(delayCriteria);
                    }
                    WriteInsertLog<DelayCriteriaModel>(new DelayCriteriaModel() {CarrierID = carrier.CarrierID});
                }
                if (!carrier.IsAllCoverage && coverageList != null && coverageList.Count > 0)
                {
                    foreach (CoverageModel coverage in coverageList)
                    {
                        coverage.CarrierID = carrier.CarrierID;
                    }
                    _coverageDAL.Add(coverageList);
                    WriteInsertLog<CoverageModel>(new CoverageModel() { CarrierID = carrier.CarrierID });
                }
                scope.Complete();
            }
            return AddResult(i, "承运商");
        }

        /// <summary>
        /// 取得承运商编号
        /// </summary>
        /// <param name="isAllCoverage"></param>
        /// <param name="coverageList"></param>
        /// <returns></returns>
        private string GetCarrierNo(bool isAllCoverage, IList<CoverageModel> coverageList, string carrierNo)
        {
            CarrierNoGenerateFormula f = new CarrierNoGenerateFormula();
            Enums.CarrierCoverage cc = Enums.CarrierCoverage.All;
            if (!isAllCoverage)
            {
                if (coverageList == null || coverageList.Count <= 1)
                {
                    cc = Enums.CarrierCoverage.NotAll;
                }
                else
                {
                    //bug fixd for 6.	页面-合同管理，承运商编号规则：适用范围为全国时，Y00，非全国时，Y01，不再区分单个区域和多个区域。
                    cc = Enums.CarrierCoverage.NotAll;
                }
            }
            CarrierNoContextModel m = new CarrierNoContextModel(cc, carrierNo);
            return f.Execute(m);
        }

        /// <summary>
        /// 更新承运商
        /// </summary>
        /// <param name="carrier">承运商</param>
        /// <param name="delayCriteriaList">延误审核标准</param>
        /// <param name="coverageList">适用范围</param>
        /// <returns>操作结果</returns>
        public ResultModel Update(CarrierModel carrier, IList<DelayCriteriaModel> delayCriteriaList, IList<CoverageModel> coverageList)
        {
            if (carrier == null) throw new ArgumentNullException("carrier");

            if (IsExistCarrier(carrier.CarrierName, carrier.CarrierID, false))
            {
                return ErrorResult("已存在相同名称的承运商！");
            }
            if (IsExistCarrier(carrier.CarrierAllName, carrier.CarrierID, true))
            {
                return ErrorResult("已存在相同全称的承运商！");
            }
            if (IsExistEmail(carrier.Email, carrier.CarrierID))
            {
                return ErrorResult("已存在相同名称的邮件地址！");
            }

            int i = 0;
            //取得承运商更新前的数据
            CarrierModel pastCarrier = Get(carrier.CarrierID);
            if (pastCarrier == null)
            {
                return ErrorResult("该承运商不存在或者已经被删除！");
            }
            //取得承运商编号
            carrier.CarrierNo = GetCarrierNo(carrier.IsAllCoverage, coverageList, pastCarrier.CarrierNo);

            //Email不能修改，防止非法提交数据 //2120328 需求变更可以修改
            //carrier.Email = pastCarrier.Email;

            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                //更新承运商
                i = _carrierDAL.Update(carrier);
                //写日志
                WriteUpdateLog<CarrierModel>(carrier, pastCarrier);
                //删除延误考核标准
                _delayCriteriaDAL.Delete(new List<int> { carrier.CarrierID });
                //添加延误考核标准
                if (delayCriteriaList != null && delayCriteriaList.Count > 0)
                {
                    foreach (DelayCriteriaModel delayCriteria in delayCriteriaList)
                    {
                        delayCriteria.CarrierID = carrier.CarrierID;
                        _delayCriteriaDAL.Add(delayCriteria);
                    }
                    WriteForcedUpdateLog<DelayCriteriaModel>(carrier.CarrierID.ToString());
                }
                //删除适用范围
                _coverageDAL.Delete(new List<int> { carrier.CarrierID });
                //添加适用范围
                if (!carrier.IsAllCoverage && coverageList != null && coverageList.Count > 0)
                {
                    foreach (CoverageModel coverage in coverageList)
                    {
                        coverage.CarrierID = carrier.CarrierID;
                    }
                    _coverageDAL.Add(coverageList);
                    WriteForcedUpdateLog<CoverageModel>(carrier.CarrierID.ToString());
                }
                scope.Complete();
            }
            return UpdateResult(i, "承运商");
        }

        /// <summary>
        /// 批量删除承运商
        /// </summary>
        /// <param name="ids">承运商id列表</param>
        /// <returns>操作结果</returns>
        public ResultModel Delete(List<int> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }
            if (ids.Count == 0)
            {
                throw new ArgumentException("承运商id列表数必须大于0");
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                //删除承运商
                i = _carrierDAL.Delete(ids);
                //写日志
                WriteBatchDeleteLog<CarrierModel>(ids);
                //删除延误考核标准
                _delayCriteriaDAL.Delete(ids);
                //删除适用范围
                _coverageDAL.Delete(ids);
                scope.Complete();
            }

            return DeleteResult(i, "承运商");
        }

        /// <summary>
        /// 判断是否存在承运商
        /// </summary>
        /// <param name="name">承运商名称</param>
        /// <param name="isAllName">是否是全名</param>
        /// <returns>是/否</returns>
        public bool IsExistCarrier(string name, int carrierID, bool isAllName = false)
        {
            if (isAllName)
            {
                return _carrierDAL.IsExistCarrierAllName(name, carrierID);
            }
            else
            {
                return _carrierDAL.IsExistCarrierName(name, carrierID);
            }
        }

        /// <summary>
        /// 判断是否存在承运商合同编号
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="carrierID"></param>
        /// <returns></returns>
        public bool IsExistContractNumber(string contractNumber, int carrierID)
        {
            return _carrierDAL.IsExistContractNumber(contractNumber, carrierID);
        }

        /// <summary>
        /// 判断邮件是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <param name="carrierID"></param>
        /// <returns></returns>
        public bool IsExistEmail(string email, int carrierID)
        {
            return _carrierDAL.IsExistEmail(email, carrierID);
        }


        public CarrierModel Get(int carrierID)
        {
            return _carrierDAL.Get(carrierID);
        }

        public PagedList<CarrierModel> Search(CarrierSearchModel searchModel)
        {
            return _carrierDAL.Search(searchModel);
        }

        public IList<CarrierModel> GetAll()
        {
            return _carrierDAL.GetAll();
        }

        public IList<DelayCriteriaModel> GetDelayCriteriaByCarrierID(int carrierID)
        {
            return _delayCriteriaDAL.GetByCarrierID(carrierID);
        }


        public IList<CoverageModel> GetCoverageByCarrierID(int carrierID)
        {
            return _coverageDAL.GetByCarrierID(carrierID);
        }

        /// <summary>
        /// 读取操作日志
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        public List<Model.Log.OperateLogModel> Read(Model.Log.BaseOperateLogSearchModel searchmodel)
        {
            if (searchmodel == null) throw new ArgumentNullException("Read OperateLogModel.searchmodel is null");
            searchmodel.Module = Enums.SysModule.CarrierModule;
            return base.ReadLog<Model.Log.OperateLogModel>(searchmodel);
        }

        #endregion

        #region ICarrierBLL 成员


        public int GetCarrierIdByName(string name, string distributionCode)
        {
            return _carrierDAL.GetCarrierIdByName(name, distributionCode);
        }

        #endregion
    }
}
