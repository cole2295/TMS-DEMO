using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Model.Common
{
    public static class Consts
    {
        /// <summary>
        /// TMS只读配置
        /// </summary>
        public const string TMS_READONLY_CONNECTION_KEY = "TmsReadOnlyConnectionString";
        /// <summary>
        /// TMS写配置
        /// </summary>
        public const string TMS_WRITE_CONNECTION_KEY = "TmsConnectionString";

        /// <summary>
        /// LMS_RFD[SQL SERVER]只读配置
        /// </summary>
        public const string LMS_RFD_READONLY_CONNECTION_KEY = "LmsRfdReadOnlyConnectionString";
        /// <summary>
        /// LMS_RFD[SQL SERVER]写配置
        /// </summary>
        public const string LMS_RFD_WRITE_CONNECTION_KEY = "LmsRfdConnectionString";

        /// <summary>
        /// PS_LMS物流主库[ORACLE]只读配置
        /// </summary>
        public const string PS_LMS_Oracle_READONLY_CONNECTION_KEY = "LmsOracleReadOnlyConnectionString";
        /// <summary>
        /// PS_LMS物流主库[ORACLE]写配置
        /// </summary>
        public const string PS_LMS_Oracle_WRITE_CONNECTION_KEY = "LmsOracleConnectionString";

        /// <summary>
        /// 允许的连接池最大线程数
        /// </summary>
        public static readonly int CONNECTION_POOL_MAX_COUNT = Convert.ToInt32(ConfigurationHelper.GetAppSetting("ConnectionPoolMaxCount"));

        /// <summary>
        /// 服务配置文件
        /// </summary>
        public static readonly string SERVICE_CONFIG_PATH = ConfigurationHelper.GetAppSetting("ServiceConfigPath");

        /// <summary>
        /// 公式配置文件
        /// </summary>
        public static readonly string FORMULA_CONFIG_PATH = ConfigurationHelper.GetAppSetting("FormulaConfigPath");

        /// <summary>
        /// 服务文件路径
        /// </summary>
        public static readonly string SERVER_PATH = ConfigurationHelper.GetAppSetting("ServerPath");

        /// <summary>
        /// 清空pool的执行间隔时间
        /// </summary>
        public static readonly double POOL_CLEAR_INTERVAL = double.Parse(ConfigurationHelper.GetAppSetting("PoolClearInterval"));

        /// <summary>
        /// 清空pool的过期时间,最后一次使用到现在超过该时长则清掉
        /// </summary>
        public static readonly int POOL_CLEAR_TIMESPAN = int.Parse(ConfigurationHelper.GetAppSetting("PoolClearTimeSpan"));

        /// <summary>
        /// socket等待队列中的最大允许数量
        /// </summary>
        public static readonly int SOCKET_WAITING_QUEUE_MAX_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("SocketWaitingQueueMaxCount"));

        /// <summary>
        /// socket排队等待超时时间
        /// </summary>
        public static readonly int SOCKET_WAITING_TIME_OUT = int.Parse(ConfigurationHelper.GetAppSetting("SocketWaitingTimeOut"));

        /// <summary>
        /// socket池最大socket数
        /// </summary>
        public static readonly int SOCKET_POOL_MAX_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("SocketPoolMaxCount"));

        /// <summary>
        /// 操作日志更新时的Note模板
        /// </summary>
        public static readonly string UPDATE_OPERATE_LOG_NOTE = ConfigurationHelper.GetAppSetting("UpdateOperateLogNote");

        /// <summary>
        /// 空操作日志更新时的Note模板
        /// </summary>
        public static readonly string NULL_UPDATE_OPERATE_LOG_NOTE = ConfigurationHelper.GetAppSetting("NullUpdateOperateLogNote");

        /// <summary>
        /// 写操作日志时不用比较的列名
        /// </summary>
        public static readonly string OPERATE_LOG_NOT_COMPARE_COLUMNS = ConfigurationHelper.GetAppSetting("OperateLogNotCompareColumns");

        /// <summary>
        /// 同步服务操作密码
        /// </summary>
        public static readonly string SYNC_OPERATE_PWD = ConfigurationHelper.GetAppSetting("SyncOperatePwd");

        /// <summary>
        /// 新增成功默认信息
        /// </summary>
        public const string ADD_RESULT_SUCCESS_MESSAGE = "新增{0}成功！";

        /// <summary>
        /// 新增失败默认信息
        /// </summary>
        public const string ADD_RESULT_FAILED_MESSAGE = "新增{0}失败！";

        /// <summary>
        /// 更新成功默认信息
        /// </summary>
        public const string UPDATE_RESULT_SUCCESS_MESSAGE = "更新{0}成功！";

        /// <summary>
        /// 更新失败默认信息
        /// </summary>
        public const string UPDATE_RESULT_FAILED_MESSAGE = "更新{0}失败！";

        /// <summary>
        /// 删除成功默认信息
        /// </summary>
        public const string DELETE_RESULT_SUCCESS_MESSAGE = "成功删除{0}条{1}！";

        /// <summary>
        /// 删除失败默认信息
        /// </summary>
        public const string DELETE_RESULT_FAILED_MESSAGE = "删除{0}失败！";

        /// <summary>
        /// PMS系统承运商角色ID
        /// </summary>
        public static readonly int CARRIER_ROLE_ID = int.Parse(ConfigurationHelper.GetAppSetting("CarrierRoleID"));

        /// <summary>
        /// 承运商默认密码
        /// </summary>
        public static readonly string CARRIER_DEFAULT_PWD = ConfigurationHelper.GetAppSetting("CarrierDefaultPwd");

        /// <summary>
        /// TMS系统KEY值(PMS)
        /// </summary>
        public static readonly string TMS_SYS_KEY = ConfigurationHelper.GetAppSetting("TmsSysKey");

        /// <summary>
        /// Cookie过期默认时长(天)
        /// </summary>
        public static readonly int COOKIE_EXPIRES = int.Parse(ConfigurationHelper.GetAppSetting("CookieExpires"));

        /// <summary>
        /// 聊天室默认多少时间没有请求就会被认为是已经下线
        /// </summary>
        public static readonly int CHAT_OFFLINE_TIMESPAN = int.Parse(ConfigurationHelper.GetAppSetting("ChatOfflineTimeSpan"));

        /// <summary>
        /// 同步服务失败重试次数
        /// </summary>
        public static readonly int SYNC_SERVICE_TRY_TIMES = int.Parse(ConfigurationHelper.GetAppSetting("SyncServiceTryTimes"));

        /// <summary>
        /// LMS同步到TMS状态相同错误内容
        /// </summary>
        public static readonly string SYNC_LMS2TMS_STATUS_SAME_ERROR = "TMS运单状态【{0}{1}】与LMS运单状态相同";

        /// <summary>
        /// LMS同步到TMS返货状态相同错误内容
        /// </summary>
        public static readonly string SYNC_LMS2TMS_RETURN_STATUS_SAME_ERROR = "TMS返货状态【{0}{1}】与LMS返货状态相同";

        /// <summary>
        /// LMS同步到TMS状态变更日志
        /// </summary>
        public static readonly string SYNC_LMS2TMS_TMS_STATUS_CHANGED_LOG = "TMS状态由【{0}{1}】更新为【{2}{3}】";

        /// <summary>
        /// LMS同步到TMS配送站点变更日志
        /// </summary>
        public static readonly string SYNC_LMS2TMS_TMS_DELIVERSTATION_CHANGED_LOG = "配送站点由【{0}】更新为【{1}】";

        /// <summary>
        /// LMS同步到TMS入库变更日志
        /// </summary>
        public static readonly String SYNC_LMS2TMS_INBOUND_CHANGE_LOG = "TMS状态由【{0}{1}】更新为【{2}{3}】,InboundID:【{4}】";

        /// <summary>
        /// LMS同步到TMS出库变更日志
        /// </summary>
        public static readonly String SYNC_LMS2TMS_OUTBOUND_CHANGE_LOG = "TMS状态由【{0}{1}】更新为【{2}{3}】,OutboundID:【{4}】,当前配送商编号由【{5}】更新为【{6}】";

        /// <summary>
        /// LMS同步到TMS当前配送商编号变更日志
        /// </summary>
        public static readonly string SYNC_LMS2TMS_TMS_CURRENTDISTRIBUTIONCODE_CHANGED_LOG = "当前配送商编号由【{0}】更新为【{1}】";

        /// <summary>
        /// LMS同步到TMS插入运单日志
        /// </summary>
        public static readonly string SYNC_LMS2TMS_TMS_INSERT_LOG = "插入新运单,状态为【{0}{1}】";

        /// <summary>
        /// 代理用户COOKIE名
        /// </summary>
        public static readonly string AGENT_USER_COOKIE_NAME = ".AGENTUSER";
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public static readonly string SMTP_HOST = ConfigurationHelper.GetAppSetting("SmtpHost");
        /// <summary>
        /// 邮件服务器帐号
        /// </summary>
        public static readonly string SMTP_ACCOUNT = ConfigurationHelper.GetAppSetting("SmtpAccount");
        /// <summary>
        /// 邮件服务器密码
        /// </summary>
        public static readonly string SMTP_PASSWORD = ConfigurationHelper.GetAppSetting("SmtpPassword");
        /// <summary>
        /// 邮件服务器发件人
        /// </summary>
        public static readonly string SMTP_FROM = ConfigurationHelper.GetAppSetting("SmtpFrom");

        /// <summary>
        /// 是否判断混操作装车
        /// </summary>
        public static readonly bool ISCHECKLMSSTATUSFORWAYBILLLOADING = true;

        /// <summary>
        /// 控件js域地址
        /// </summary>
        public static readonly string UserControlsBYJS_URL = ConfigurationHelper.GetAppSetting("UserControlsBYJS_URL");

        /// <summary>
        /// 日志接口API
        /// </summary>
        public static readonly string CloudlogAPIUrl = ConfigurationHelper.GetAppSetting("CloudlogAPIUrl");

        static Consts()
        {
            if (!ConfigurationHelper.GetAppSetting("IsCheckLmsStatusForWaybillLoading").Equals("0"))
            {
                ISCHECKLMSSTATUSFORWAYBILLLOADING = Convert.ToBoolean(ConfigurationHelper.GetAppSetting("IsCheckLmsStatusForWaybillLoading"));
            }
        }

    }
}
