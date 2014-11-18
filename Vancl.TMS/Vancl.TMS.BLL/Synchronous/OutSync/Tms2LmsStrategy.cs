using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.WaybillLifeCycleService;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Model.Log;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// TMS分拣数据同步到LMS物流主库
    /// </summary>
    public abstract class Tms2LmsStrategy
    {
        #region 公用的私有字段

        private IWaybillDAL _wl_waybill_sql = null;
        private IWaybillDAL _wl_waybill_oracle = null;
        private IOperateLogDAL _wl_operatelog_sql = null;
        private IOperateLogDAL _wl_operatelog_oracle = null;
        private IWaybillStatusChangeLogDAL _wl_WaybillStatusChangeLog_sql = null;
        private IWaybillStatusChangeLogDAL _wl_WaybillStatusChangeLog_oracle = null;
        private IBillDAL _sc_billinfo = null;
        private IEmployeeDAL _employeedal = null;
        private IFormula<String, KeyCodeContextModel> _keycodeformula = null;
        private bool? _islmsSqlEnabled;
        private DateTime? _lmsSqlDeadLine;

        #endregion

        /// <summary>
        /// 表单主信息业务服务
        /// </summary>
        protected IBillBLL BillBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        ISortingPackingDAL _sortingPackingDAL = ServiceFactory.GetService<ISortingPackingDAL>("SortingPackingDAL");

        /// <summary>
        /// 是否启用同步回物流主库LMS【SQL】
        /// </summary>
        public bool IsLmsSQLEnabled
        {
            get
            {
                if (!_islmsSqlEnabled.HasValue)
                {
                    var cfgValue = ConfigurationHelper.GetAppSettingByDefault("IsLmsSQLEnabled");
                    if (String.IsNullOrWhiteSpace(cfgValue))
                    {
                        //没有配置，默认需要写SQL
                        _islmsSqlEnabled = true;
                    }
                    else
                    {
                        _islmsSqlEnabled = Convert.ToBoolean(cfgValue);
                    }
                }
                return _islmsSqlEnabled.Value;
            }
        }

        /// <summary>
        /// 同步回物流主库LMS【SQL】的截止时间
        /// </summary>
        public DateTime? LmsSQLDeadLine
        {
            get
            {
                if (!_lmsSqlDeadLine.HasValue)
                {
                    var cfgValue = ConfigurationHelper.GetAppSettingByDefault("LmsSQLDeadLine");
                    if (String.IsNullOrWhiteSpace(cfgValue))
                    {
                        //没有配置，默认为最大时间
                        _lmsSqlDeadLine = DateTime.MaxValue;
                    }
                    else
                    {
                        _lmsSqlDeadLine = Convert.ToDateTime(cfgValue);
                    }
                }
                return _lmsSqlDeadLine.Value;
            }
        }



        /// <summary>
        /// 是否操作LMS物流SQL版本主库
        /// </summary>
        protected bool IsOperateLMSSQL
        {
            get
            {
                if (IsLmsSQLEnabled)
                {
                    if (LmsSQLDeadLine.HasValue)
                    {
                        if (LmsSQLDeadLine.Value > DateTime.Now)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 物流主库waybill sql数据层实现
        /// </summary>
        protected IWaybillDAL WL_Waybill_SQLDAL
        {
            get
            {
                if (_wl_waybill_sql == null)
                {
                    _wl_waybill_sql = ServiceFactory.GetService<IWaybillDAL>("LMSWaybillDAL_SQL");
                }
                return _wl_waybill_sql;
            }
        }
        /// <summary>
        /// 物流主库waybill oracle数据层实现
        /// </summary>
        protected IWaybillDAL WL_Waybill_OracleDAL
        {
            get
            {
                if (_wl_waybill_oracle == null)
                {
                    _wl_waybill_oracle = ServiceFactory.GetService<IWaybillDAL>("LMSWaybillDAL_Oracle");
                }
                return _wl_waybill_oracle;
            }
        }
        /// <summary>
        /// 物流主库Operatelog sql数据层实现
        /// </summary>
        protected IOperateLogDAL WL_OperateLog_SQLDAL
        {
            get
            {
                if (_wl_operatelog_sql == null)
                {
                    _wl_operatelog_sql = ServiceFactory.GetService<IOperateLogDAL>("LMSOperateLogDAL_SQL");
                }
                return _wl_operatelog_sql;
            }
        }

        /// <summary>
        /// 物流主库Operatelog oracle数据层实现
        /// </summary>
        protected IOperateLogDAL WL_OperateLog_OracleDAL
        {
            get
            {
                if (_wl_operatelog_oracle == null)
                {
                    _wl_operatelog_oracle = ServiceFactory.GetService<IOperateLogDAL>("LMSOperateLogDAL_Oracle");
                }
                return _wl_operatelog_oracle;
            }
        }

        /// <summary>
        /// 物流主库WaybillStatusChangeLog sql数据层实现
        /// </summary>
        protected IWaybillStatusChangeLogDAL WL_WaybillStatusChangeLog_SQLDAL
        {
            get
            {
                if (_wl_WaybillStatusChangeLog_sql == null)
                {
                    _wl_WaybillStatusChangeLog_sql = ServiceFactory.GetService<IWaybillStatusChangeLogDAL>("LMSWaybillStatusChangeLogDAL_SQL");
                }
                return _wl_WaybillStatusChangeLog_sql;
            }
        }

        /// <summary>
        /// 物流主库WaybillStatusChangeLog oracle数据层实现
        /// </summary>
        protected IWaybillStatusChangeLogDAL WL_WaybillStatusChangeLog_OracleDAL
        {
            get
            {
                if (_wl_WaybillStatusChangeLog_oracle == null)
                {
                    _wl_WaybillStatusChangeLog_oracle = ServiceFactory.GetService<IWaybillStatusChangeLogDAL>("LMSWaybillStatusChangeLogDAL_Oracle");
                }
                return _wl_WaybillStatusChangeLog_oracle;
            }
        }

        /// <summary>
        /// Key Code产生
        /// </summary>
        protected IFormula<String, KeyCodeContextModel> KeyCodeGenerator
        {
            get
            {
                if (_keycodeformula == null)
                {
                    _keycodeformula = FormulasFactory.GetFormula<IFormula<String, KeyCodeContextModel>>("keycodeBLLFormula");
                }
                return _keycodeformula;
            }
        }

        /// <summary>
        /// Employee数据层
        /// </summary>
        protected IEmployeeDAL EmployeeDAL
        {
            get
            {
                if (_employeedal == null)
                {
                    _employeedal = ServiceFactory.GetService<IEmployeeDAL>("EmployeeDAL");
                }
                return _employeedal;
            }
        }

        /// <summary>
        /// TMS分拣运单数据
        /// </summary>
        protected IBillDAL SC_BillInfoDAL
        {
            get
            {
                if (_sc_billinfo == null)
                {
                    _sc_billinfo = ServiceFactory.GetService<IBillDAL>("BillDAL");
                }
                return _sc_billinfo;
            }
        }

        /// <summary>
        /// 创建LMS物流主库OperateLog实体对象
        /// </summary>
        /// <param name="scchangelogmodel">TMS billchange对象</param>
        /// <returns></returns>
        protected virtual Vancl.TMS.Model.LMS.OperateLogEntityModel CreateLMSOperateLogEntityModel(BillChangeLogModel scchangelogmodel)
        {
            var EmployeeName = EmployeeDAL.GetEmployeeName(scchangelogmodel.CreateBy);
            var logmodel = new Model.LMS.OperateLogEntityModel()
            {
                CreateBy = scchangelogmodel.CreateBy,
                CreateTime = scchangelogmodel.CreateTime,
                IsDeleted = false,
                IsSyn = 0,
                LogOperator = String.IsNullOrWhiteSpace(EmployeeName) ? "系统" : EmployeeName,
                LogType = 0,
                OldDeliverMan = null,
                OperateTime = scchangelogmodel.CreateTime,
                Operation = EnumHelper.GetDescription<Enums.TmsOperateType>(scchangelogmodel.OperateType),
                OperatorStation = scchangelogmodel.CreateDept,
                Result = scchangelogmodel.Note,
                Status = scchangelogmodel.CurrentStatus,
                UpdateBy = scchangelogmodel.CreateBy,
                UpdateTime = scchangelogmodel.CreateTime,
                WaybillNO = long.Parse(scchangelogmodel.FormCode)
            };
            IKeyCodeable ikey = logmodel as IKeyCodeable;
            logmodel.OperateLogKid = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = ikey.SequenceName, TableCode = ikey.TableCode });
            return logmodel;
        }

        /// <summary>
        /// 创建LMS物流主库WaybillStatusChangeLog实体对象
        /// </summary>
        /// <param name="waybillmodel">LMS物流主库运单对象</param>
        /// <param name="scchangelogmodel">TMS分拣billchanglog对象</param>
        /// <returns></returns>
        protected virtual Vancl.TMS.Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel, BillChangeLogModel scchangelogmodel)
        {
            //TODO:注释部分请予继承的子类赋值
            return new Model.LMS.WaybillStatusChangeLogEntityModel()
            {
                CreateBy = scchangelogmodel.CreateBy,
                CreateDept = scchangelogmodel.CreateDept,
                CreateTime = scchangelogmodel.CreateTime,
                //CurNode = Enums.StatusChangeNodeType.DeliverCenter,
                CustomerOrder = waybillmodel.CustomerOrder,
                DeliverStationID = scchangelogmodel.DeliverStationID,
                DistributionCode = waybillmodel.DistributionCode,
                IsBISyn = false,
                IsDeleted = false,
                IsM2sSyn = "0",
                IsSyn = false,
                LMS_WaybillStatusChangeLogKid = scchangelogmodel.BCID,
                MerchantID = waybillmodel.MerchantID,
                Note = scchangelogmodel.Note,
                //OperateType = Enums.Lms2TmsOperateType.Inbound,
                Status = scchangelogmodel.CurrentStatus,
                //SubStatus = (int)scchangelogmodel.CurrentSatus,
                TmsSyncStatus = Enums.SyncStatus.Already,       //从TMS同步回LMS物流主库，表示已经做过了数据同步
                UpdateBy = scchangelogmodel.CreateBy,
                UpdateTime = scchangelogmodel.CreateTime,
                WaybillNO = long.Parse(scchangelogmodel.FormCode)
            };
        }

        public ResultModel DoSync(BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null.");

            //调用WaybillLifeCycleService
            AddLifeCycle(model);
            return Sync(model);
        }

        protected abstract ResultModel Sync(BillChangeLogModel model);

        private void AddLifeCycle(BillChangeLogModel model)
        {
            WaybillLifeCycleClient client = new WaybillLifeCycleClient();

            var lifeCycle = new WaybillLifeCycleDto();
            lifeCycle.CurNode = (int)model.OperateType;
            lifeCycle.OpDate = DateTime.Now;
            lifeCycle.OpDeptID = model.CreateDept;
            lifeCycle.OpManID = model.CreateBy;
            lifeCycle.DistributionCode = model.CurrentDistributionCode;

			lifeCycle.DeliverStationID = model.DeliverStationID;
			lifeCycle.ToExpressCompanyID = model.ToExpressCompanyID;
			lifeCycle.ToDistributionCode = model.ToDistributionCode;
            lifeCycle.Status = (int)model.CurrentStatus;
            lifeCycle.SubStatus = (int)model.PreStatus;
            lifeCycle.WaybillNo = Convert.ToInt64(model.FormCode);
            lifeCycle.Note = model.Note;
			#region
			//增加操作功能点描述
	        switch (model.OperateType)
	        {
				case Enums.TmsOperateType.Inbound:
					lifeCycle.Operation = "运单入库";
			        break;
				case Enums.TmsOperateType.Packing:
					lifeCycle.Operation = "运单装箱";
			        break;
				case Enums.TmsOperateType.Outbound:
					lifeCycle.Operation = "运单出库";
			        break;
				case Enums.TmsOperateType.ReturnInbound:
			        lifeCycle.Operation = "返货入库";
			        break;
				case Enums.TmsOperateType.ReturnOutbound:
					lifeCycle.Operation = "返货出库";
			        break;
				default:
			        break;
	        }

	        #endregion
            client.AddLifeCycle(lifeCycle);
        }
    }
}
