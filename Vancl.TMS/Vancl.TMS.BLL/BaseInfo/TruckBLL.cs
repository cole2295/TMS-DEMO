using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Truck;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class TruckBLL : BaseBLL, ITruckBLL, IOperateLogBLL
    {
        ITruckDAL _dal = ServiceFactory.GetService<ITruckDAL>("TruckDAL");

        public PagedList<ViewTruckModel> GetTruckList(TruckSearchModel searchModel)
        {
            return _dal.GetTruckList(searchModel);
        }

        public ResultModel Add(TruckBaseInfoModel model)
        {
            try
            {
                model.TBID = _dal.GetNextSequence(model.SequenceName).ToString();
                var i = _dal.Add(model);
                WriteInsertLog<TruckBaseInfoModel>(model);
                return new ResultModel { IsSuccess = i > 0, Message = i > 0 ? "新增车辆信息成功" : "新增车辆信息失败" };
            }
            catch (Exception ex)
            {
                return new ResultModel { IsSuccess = false, Message = ex.Message };
            }
        }

        public ResultModel Update(TruckBaseInfoModel model)
        {
            try
            {
                TruckBaseInfoModel pastModel = GetTruckBaseInfo(model.TBID);
                var i = _dal.Update(model);
                WriteUpdateLog<TruckBaseInfoModel>(model,pastModel);
                return new ResultModel { IsSuccess = i > 0, Message = i > 0 ? "修改车辆信息成功" : "修改车辆信息失败" };
            }
            catch (Exception ex)
            {
                return new ResultModel { IsSuccess = false, Message = ex.Message };
            }
        }

        public ResultModel SetDisabled(List<string> tbidList)
        {
            try
            {
                var i = _dal.SetDisabled(tbidList);
                WriteBatchDeleteLog<TruckBaseInfoModel>(tbidList);
                return new ResultModel { IsSuccess = i > 0, Message = i > 0 ? "停用车辆信息成功" : "停用车辆信息失败" };
            }
            catch (Exception ex)
            {
                return new ResultModel { IsSuccess = false, Message = ex.Message };
            }
        }

        public TruckBaseInfoModel GetTruckBaseInfo(string tbid)
        {
            return _dal.GetTruckBaseInfo(tbid);
        }

        public IList<TruckBaseInfoModel> GetAll()
        {
            return _dal.GetAll();
        }

        #region IOperateLogBLL 成员
        /// <summary>
        /// 读取操作日志
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        public List<Model.Log.OperateLogModel> Read(Model.Log.BaseOperateLogSearchModel searchmodel)
        {
            if (searchmodel == null) throw new ArgumentNullException("Read OperateLogModel.searchmodel is null");
            searchmodel.Module = Enums.SysModule.LineModule;
            return base.ReadLog<Model.Log.OperateLogModel>(searchmodel);
        }

        #endregion
    }
}
