using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Log;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Core.ServiceFactory;
using System.Reflection;
using System.Web;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class BillChangeLogFactory
    {
        public static BillChangeLogBLL GetBillChangeLogBLL(Enums.TmsOperateType operateType)
        {
            Type type = Assembly.GetExecutingAssembly().GetType("Vancl.TMS.BLL.Log.BillChangeLog." + operateType.ToString() + "BillChangeLogBLL");
            if (type == null)
            {
                return null;
            }
            return (BillChangeLogBLL)Activator.CreateInstance(type);
        }
    }

    /// <summary>
    /// 状态改变日志逻辑层
    /// </summary>
    internal abstract class BillChangeLogBLL
    {
        IBillChangeLogDAL _billChangeLogDAL = ServiceFactory.GetService<IBillChangeLogDAL>("BillChangeLogDAL");
        IOutsourcingLogDAL _outsourcingLogDAL = ServiceFactory.GetService<IOutsourcingLogDAL>("OutsourcingLogDAL");

        private string _ipAddress = null;
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        private string IPAddress
        {
            get
            {
                if (_ipAddress == null)
                {
                    if (HttpContext.Current == null)
                    {
                        _ipAddress = "";
                    }
                    else if (HttpContext.Current.Request == null)
                    {
                        _ipAddress = "";
                    }
                    else
                    {
                        if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                        {
                            _ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null ? "" : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                        }
                        else
                        {
                            _ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? "" : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        }
                    }
                }
                return _ipAddress;
            }
        }

        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="dynamicModel"></param>
        /// <returns></returns>
        public int AddLog(BillChangeLogDynamicModel dynamicModel)
        {
            BillChangeLogModel logModel = new BillChangeLogModel();
            logModel.CurrentDistributionCode = dynamicModel.CurrentDistributionCode;
	        logModel.ToDistributionCode = dynamicModel.ToDistributionCode;
	        logModel.ToExpressCompanyID = dynamicModel.ToExpressCompanyID;
            logModel.DeliverStationID = dynamicModel.DeliverStationID;
            logModel.CurrentStatus = dynamicModel.CurrentSatus;
            logModel.PreStatus = dynamicModel.PreStatus;
            logModel.FormCode = dynamicModel.FormCode;
            logModel.OperateType = dynamicModel.OperateType;
            logModel.SyncFlag = Enums.SyncStatus.NotYet;
            logModel.Note = GetNote(dynamicModel);
            logModel.IPAddress = IPAddress;
            logModel.ClientInfo = GetClientInfo();
            logModel.CreateBy = dynamicModel.CreateBy;
            logModel.CreateDept = dynamicModel.CreateDept;
            logModel.ReturnStatus = dynamicModel.ReturnStatus;
            OutsourcingLogModel outsourcingLog = null;
            if (GetOutsourcingLogModel(dynamicModel, out outsourcingLog))
            {
                //如果有外包操作，则记录外包操作日志
                if (outsourcingLog == null) throw new Exception("程序严重错误，有外包行为，但是外包对象为null");
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    _billChangeLogDAL.Add(logModel);
                    _outsourcingLogDAL.Add(outsourcingLog);
                    scope.Complete();
                }
                return 1;
            }
            else
            {
                return _billChangeLogDAL.Add(logModel);
            }
        }

	    /// <summary>
        /// 批量新增日志
        /// </summary>
        /// <param name="listdynamicModel"></param>
        /// <returns></returns>
        public int BatchAddLog(List<BillChangeLogDynamicModel> listdynamicModel)
        {
            if (listdynamicModel == null) throw new ArgumentNullException("listdynamicModel is null");
            List<BillChangeLogModel> listlogModel = new List<BillChangeLogModel>(listdynamicModel.Count);
            String clientinfo = GetClientInfo();
            listdynamicModel.ForEach(p =>
            {
                listlogModel.Add(new BillChangeLogModel()
                {
                    CurrentDistributionCode = p.CurrentDistributionCode,
					ToDistributionCode = p.ToDistributionCode,
					ToExpressCompanyID = p.ToExpressCompanyID,
                    DeliverStationID = p.DeliverStationID,
                    CurrentStatus = p.CurrentSatus,
                    PreStatus = p.PreStatus,
                    FormCode = p.FormCode,
                    OperateType = p.OperateType,
                    SyncFlag = Enums.SyncStatus.NotYet,
                    Note = GetNote(p),
                    IPAddress = IPAddress,
                    ClientInfo = clientinfo,
                    CreateBy = p.CreateBy,
                    CreateDept = p.CreateDept
                });
            });
            List<OutsourcingLogModel> listOutsourcingLog = null;
            if (BatchGetOutsourcingLogModel(listdynamicModel, out listOutsourcingLog))
            {
                //如果为外包操作，则记录外包操作日志
                if (listOutsourcingLog == null) throw new Exception("程序严重错误，有外包行为，但是外包列表对象为null");
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    _billChangeLogDAL.BatchAdd(listlogModel);
                    _outsourcingLogDAL.BatchAdd(listOutsourcingLog);
                    scope.Complete();
                }
                return 1;
            }
            else
            {
                return _billChangeLogDAL.BatchAdd(listlogModel);
            }
        }


        /// <summary>
        /// 获得客户端信息
        /// </summary>
        /// <returns></returns>
        private string GetClientInfo()
        {
            if (HttpContext.Current == null)
            {
                return "系统";
            }
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append("员工ID：" + UserContext.CurrentUser.ID + Environment.NewLine);
            sbMessage.Append("员工名称：" + UserContext.CurrentUser.UserName + Environment.NewLine);
            sbMessage.Append("员工账号：" + UserContext.CurrentUser.UserCode + Environment.NewLine);
            sbMessage.Append("所在部门ID：" + UserContext.CurrentUser.DeptID + Environment.NewLine);
            sbMessage.Append("所在部门名称：" + UserContext.CurrentUser.DeptName + Environment.NewLine);
            sbMessage.Append("配送商编号：" + UserContext.CurrentUser.DistributionCode + Environment.NewLine);
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                sbMessage.Append("客户端IP：" + IPAddress + Environment.NewLine);
                sbMessage.Append("客户端DNS主机名：" + HttpContext.Current.Request.UserHostName + Environment.NewLine);
                sbMessage.Append("客户端使用平台：" + HttpContext.Current.Request.Browser.Platform + Environment.NewLine);
                sbMessage.Append("客户端使用浏览器：" + HttpContext.Current.Request.Browser.Type + Environment.NewLine);
                sbMessage.Append("客户端浏览器版本号：" + HttpContext.Current.Request.Browser.Version + Environment.NewLine);
                sbMessage.Append("客户端请求URL：" + HttpContext.Current.Request.Url + Environment.NewLine);
            }
            return sbMessage.ToString();
        }

        /// <summary>
        /// 取得描述信息
        /// </summary>
        /// <param name="dynamicModel"></param>
        /// <returns></returns>
        protected abstract string GetNote(BillChangeLogDynamicModel dynamicModel);

        /// <summary>
        /// 取得代理用户的外包操作对象[如果是WIN服务等需要重写此方法]
        /// </summary>
        /// <param name="dynamicModel"></param>
        /// <returns></returns>
        protected virtual bool GetOutsourcingLogModel(BillChangeLogDynamicModel dynamicModel, out OutsourcingLogModel osLogModel)
        {
            if (dynamicModel == null) throw new ArgumentNullException("dynamicModel is null");
            osLogModel = null;
            var agentUser = UserContext.AgentUser;
            if (agentUser != null && agentUser.ID > 0)
            {
                OutsourcingLogModel outsourcingLog = new OutsourcingLogModel();
                outsourcingLog.FormCode = dynamicModel.FormCode;
                outsourcingLog.AgentDeptID = agentUser.DeptID;
                outsourcingLog.AgentUserID = agentUser.ID;
                outsourcingLog.AgentDistributionCode = agentUser.DistributionCode;
                outsourcingLog.PrincipalUserID = dynamicModel.CreateBy;
                outsourcingLog.PrincipalDeptID = dynamicModel.CreateDept;
                outsourcingLog.PrincipalDistributionCode = dynamicModel.CurrentDistributionCode;
                outsourcingLog.UpdateBy = dynamicModel.CreateBy;
                outsourcingLog.OperateType = dynamicModel.OperateType;
                outsourcingLog.Note = GetNote(dynamicModel);            //方便扩展，后续可记录外包自定义日志

                osLogModel = outsourcingLog;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 批量取得代理用户的外包操作对象[如果是WIN服务等需要重写此方法]
        /// </summary>
        /// <param name="dynamicModel"></param>
        /// <returns></returns>
        protected virtual bool BatchGetOutsourcingLogModel(List<BillChangeLogDynamicModel> listdynamicModel, out List<OutsourcingLogModel> listOutsourcingLog)
        {
            if (listdynamicModel == null) throw new ArgumentNullException("listdynamicModel is null");
            listOutsourcingLog = null;
            var agentUser = UserContext.AgentUser;
            if (agentUser != null && agentUser.ID > 0)
            {
                var tmplistOutsourcingLog = new List<OutsourcingLogModel>(listdynamicModel.Count);
                listdynamicModel.ForEach(l =>
                {
                    tmplistOutsourcingLog.Add(new OutsourcingLogModel()
                    {
                        FormCode = l.FormCode,
                        AgentDeptID = agentUser.DeptID,
                        AgentUserID = agentUser.ID,
                        AgentDistributionCode = agentUser.DistributionCode,
                        PrincipalUserID = l.CreateBy,
                        PrincipalDeptID = l.CreateDept,
                        PrincipalDistributionCode = l.CurrentDistributionCode,
                        UpdateBy = l.CreateBy,
                        OperateType = l.OperateType,
                        Note = GetNote(l)               //方便扩展，后续可记录外包自定义日志
                    });
                });

                listOutsourcingLog = tmplistOutsourcingLog;
                return true;
            }
            return false;
        }

    }
}
