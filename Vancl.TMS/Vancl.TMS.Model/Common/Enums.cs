using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Model.Common
{
    public class Enums
    {

        /// <summary>
        /// 数据库日志类型
        /// </summary>
        public enum DBLogType
        {
            /// <summary>
            /// 错误日志
            /// </summary>
            [Description("错误日志")]
            ErrorLog = 0,
            /// <summary>
            /// 操作日志
            /// </summary>
            [Description("操作日志")]
            OperateLog = 1,
            /// <summary>
            /// 运输日志
            /// </summary>
            [Description("运输日志")]
            DeliveryLog = 2
        }

        /// <summary>
        /// 数据库连接类型
        /// </summary>
        public enum ConnectionType
        {
            /// <summary>
            /// TMS[Oracle]只读
            /// </summary>
            [Description("TMS只读")]
            TMSReadOnly = 0,
            /// <summary>
            /// TMS[Oracle]写
            /// </summary>
            [Description("TMS写")]
            TMSWrite = 1,
            /// <summary>
            /// LMS_RFD[sql server]只读
            /// </summary>
            [Description("LMS_RFDsql只读")]
            LMSSqlReadOnly = 2,
            /// <summary>
            /// LMS_RFD[sql server]写
            /// </summary>
            [Description("LMS_RFDsql写")]
            LMSSqlWrite = 3,
            /// <summary>
            /// LMS_RFD[oracle]只读
            /// </summary>
            [Description("LMS_RFDoracle只读")]
            LMSOracleReadOnly = 4,
            /// <summary>
            /// LMS_RFD[oracle]写
            /// </summary>
            [Description("LMS_RFDoracle写")]
            LMSOracleWrite = 5,
        }

        /// <summary>
        /// 操作日志类型
        /// </summary>
        public enum LogOperateType
        {
            /// <summary>
            /// 新增
            /// </summary>
            [Description("新增")]
            Insert = 1,
            /// <summary>
            /// 修改
            /// </summary>
            [Description("修改")]
            Update = 2,
            /// <summary>
            /// 删除
            /// </summary>
            [Description("删除")]
            Delete = 3,
            /// <summary>
            /// 启用停用
            /// </summary>
            [Description("启用停用")]
            SetEnable = 4,
            /// <summary>
            /// 自定义
            /// </summary>
            [Description("服务修改")]
            Customize = 5
        }

        /// <summary>
        /// 承运商状态
        /// </summary>
        public enum CarrierStatus
        {
            /// <summary>
            /// 停用
            /// </summary>
            [Description("停用")]
            Invalid = 0,
            /// <summary>
            /// 启用
            /// </summary>
            [Description("启用")]
            Valid = 1
        }


        /// <summary>
        /// 承运商状态
        /// </summary>
        public enum PlanStatus
        {
            /// <summary>
            /// 启用
            /// </summary>
            [Description("启用")]
            Valid = 0,
            /// <summary>
            /// 停用
            /// </summary>
            [Description("停用")]
            Invalid = 1
        }


        /// <summary>
        /// 提货价格类型，0次数计费，1单量计费
        /// </summary>
        public enum PriceType
        {
            /// <summary>
            /// 次数计费
            /// </summary>
            [Description("次数计费")]
            Once = 0,
            /// <summary>
            /// 单量计费
            /// </summary>
            [Description("单量计费")]
            OrderCount = 1
        }

        //每天、工作日、双休日、星期、不确定
        public enum DateType
        {
            /// <summary>
            /// 每天
            /// </summary>
            [Description("每天")]
            Day = 0,
            /// <summary>
            /// 工作日
            /// </summary>
            [Description("工作日")]
            WorkDay = 1,
            /// <summary>
            /// 星期
            /// </summary>
            [Description("双休日")]
            AllWeek = 2,
            /// <summary>
            /// 不确定
            /// </summary>
            [Description("不确定")]
            Uncertain = 3
        }

        /// <summary>
        /// 出库同步编号类型
        /// </summary>
        public enum SyncNoType
        {
            /// <summary>
            /// 箱号类型
            /// </summary>
            [Description("箱号类型")]
            Box = 1,
            /// <summary>
            /// 批次号类型
            /// </summary>
            [Description("批次号类型")]
            Batch = 2,
        }

        /// <summary>
        /// TMS城际同步标记
        /// </summary>
        public enum SC2TMSSyncFlag
        {
            /// <summary>
            /// 未同步
            /// </summary>
            [Description("未同步")]
            Notyet = 0,

            /// <summary>
            /// 已同步
            /// </summary>
            [Description("已同步")]
            Already = 1,

            /// <summary>
            /// 同步出错
            /// </summary>
            [Description("同步出错")]
            Error = 2,

            /// <summary>
            /// 同步中
            /// </summary>
            [Description("同步中")]
            Synchronizing = 3
        }

        /// <summary>
        /// 运输流程日志类型
        /// </summary>
        public enum DeliverFlowType
        {
            /// <summary>
            /// 无
            /// </summary>
            [Description("无")]
            None = 0,
            /// <summary>
            /// 新增提货单
            /// </summary>
            [Description("新增提货单")]
            Add = 1,
            /// <summary>
            /// 修改提货单
            /// </summary>
            [Description("修改提货单")]
            Update = 2,
            /// <summary>
            /// 撤回提货单
            /// </summary>
            [Description("撤回提货单")]
            Reject = 3,
            /// <summary>
            /// 提货单出库
            /// </summary>
            [Description("提货单出库")]
            Outbound = 4,
            /// <summary>
            /// 提交丢失信息
            /// </summary>
            [Description("提交丢失信息")]
            AddLost = 5,
            /// <summary>
            /// 修改丢失信息
            /// </summary>
            [Description("修改丢失信息")]
            UpdateLost = 6,
            /// <summary>
            /// 审核通过丢失信息
            /// </summary>
            [Description("审核通过丢失信息")]
            ApproveLost = 7,
            /// <summary>
            /// 驳回丢失信息
            /// </summary>
            [Description("驳回丢失信息")]
            DismissLost = 8,
            /// <summary>
            /// 删除丢失信息
            /// </summary>
            [Description("删除丢失信息")]
            DeleteLost = 9,
            /// <summary>
            /// 提交预计延误信息
            /// </summary>
            [Description("提交预计延误信息")]
            AddExpectDelay = 10,
            /// <summary>
            /// 修改预计延误信息
            /// </summary>
            [Description("修改预计延误信息")]
            UpdateExpectDelay = 11,
            /// <summary>
            /// 审核通过预计延误信息
            /// </summary>
            [Description("审核通过预计延误信息")]
            ApproveExpectDelay = 12,
            /// <summary>
            /// 驳回预计延误信息
            /// </summary>
            [Description("驳回预计延误信息")]
            DismissExpectDelay = 13,
            /// <summary>
            /// 删除预计延误信息
            /// </summary>
            [Description("删除预计延误信息")]
            DeleteExpectDelay = 14,
            /// <summary>
            /// 提货单全部丢失
            /// </summary>
            [Description("提货单全部丢失")]
            AllLost = 15,
            /// <summary>
            /// 提货单到货
            /// </summary>
            [Description("提货单到货")]
            Inbound = 16,
            /// <summary>
            /// 提货单审核
            /// </summary>
            [Description("提货单审核")]
            Approve = 17,
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        public enum ApproveStatus
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [Description("未审核")]
            NotApprove = 0,
            /// <summary>
            /// 已审核
            /// </summary>
            [Description("已审核")]
            Approved = 1,
            /// <summary>
            /// 已驳回
            /// </summary>
            [Description("已驳回")]
            Dismissed = 2
        }

        /// <summary>
        /// 运输方式
        /// </summary>
        public enum TransportType
        {
            /// <summary>
            /// 铁路
            /// </summary>
            [Description("铁路")]
            Railway = 1,
            /// <summary>
            /// 公路
            /// </summary>
            [Description("公路")]
            Highway = 2,
            /// <summary>
            /// 航空
            /// </summary>
            [Description("航空")]
            Aviation = 3
        }

        /// <summary>
        /// 线路优先级别
        /// </summary>
        public enum LinePriority
        {
            /// <summary>
            /// 优先
            /// </summary>
            [Description("优先")]
            Priority = 1,
            /// <summary>
            /// 可用
            /// </summary>
            [Description("可用")]
            Available = 2,
            /// <summary>
            /// 暂停
            /// </summary>
            [Description("暂停")]
            Pause = 3
        }

        /// <summary>
        /// 运输计划状态
        /// </summary>
        public enum TransportStatus
        {
            /// <summary>
            /// 未生效
            /// </summary>
            [Description("未生效")]
            NotEffective = 0,
            /// <summary>
            /// 已生效
            /// </summary>
            [Description("已生效")]
            Effective = 1,
            /// <summary>
            /// 已过期
            /// </summary>
            [Description("已过期")]
            Expired = 2
        }

        /// <summary>
        /// 线路状态
        /// </summary>
        public enum LineStatus
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [Description("未审核")]
            NotApprove = 0,
            /// <summary>
            /// 已审核
            /// </summary>
            [Description("已审核")]
            Approved = 1,
            /// <summary>
            /// 已驳回
            /// </summary>
            [Description("已驳回")]
            Dismissed = 2,
            /// <summary>
            /// 已生效
            /// </summary>
            [Description("已生效")]
            Effective = 3,
            /// <summary>
            /// 已作废
            /// </summary>
            [Description("已作废")]
            Expired = 4
        }

        /// <summary>
        /// 线路类型
        /// </summary>
        public enum LineType
        {
            /// <summary>
            /// 发货线路
            /// </summary>
            [Description("发货线路")]
            DeliveryLine = 0,
            /// <summary>
            /// 退货线路
            /// </summary>
            [Description("退货线路")]
            ReturnLine = 1,
            /// <summary>
            /// 通用线路
            /// </summary>
            [Description("通用线路")]
            UniversalLine = 2
        }

        /// <summary>
        /// LMS系统货物属性
        /// </summary>
        public enum LMSSyncGoodsType
        {
            /// <summary>
            /// 普通货物
            /// </summary>
            [Description("普通货物")]
            Normal = 0,

            /// <summary>
            /// 易碎
            /// </summary>
            [Description("易碎")]
            Frangible = 1,

            /// <summary>
            /// 禁航
            /// </summary>
            [Description("禁航")]
            Contraband = 2
        }

        /// <summary>
        /// 货物类型
        /// </summary>
        [Flags]
        public enum GoodsType
        {
            /// <summary>
            /// 普货
            /// </summary>
            [Description("普货")]
            Normal = 1,
            /// <summary>
            /// 禁航
            /// </summary>
            [Description("禁航")]
            Contraband = 2,
            /// <summary>
            /// 易碎
            /// </summary>
            [Description("易碎")]
            Frangible = 4
        }

        /// <summary>
        /// 运费类型
        /// </summary>
        public enum ExpressionType
        {
            /// <summary>
            /// 固定价格
            /// </summary>
            [Description("固定价格")]
            Fixed = 0,
            /// <summary>
            /// 阶梯价格
            /// </summary>
            [Description("阶梯价格")]
            Ladder = 1,
            /// <summary>
            /// 公式价格
            /// </summary>
            [Description("公式价格")]
            Formula = 2
        }

        /// <summary>
        /// 预计延误类型
        /// </summary>
        public enum ExpectDelayType
        {
            /// <summary>
            /// 不可控因素
            /// </summary>
            [Description("不可控因素")]
            Uncontrollable = 1,
            /// <summary>
            /// 可控因素
            /// </summary>
            [Description("可控因素")]
            Controllable = 2,
            /// <summary>
            /// 操作失误
            /// </summary>
            [Description("操作失误")]
            Misoperation = 3,
            /// <summary>
            /// 线路问题
            /// </summary>
            [Description("线路问题")]
            LineUnusual = 4,
            /// <summary>
            /// 承运商异常
            /// </summary>
            [Description("承运商异常")]
            CarrierUnusual = 5,
            /// <summary>
            /// 火车异常
            /// </summary>
            [Description("火车异常")]
            TrainUnusual = 6,
            /// <summary>
            /// 飞机异常
            /// </summary>
            [Description("飞机异常")]
            PlanUnusual = 7,
            /// <summary>
            /// 天气原因
            /// </summary>
            [Description("天气原因")]
            WeatherUnusual = 8,
            /// <summary>
            /// 交通原因
            /// </summary>
            [Description("交通原因")]
            TrafficUnusual = 9,
            /// <summary>
            /// 其他原因
            /// </summary>
            [Description("其他原因")]
            Other = 10,
            /// <summary>
            /// 库房原因
            /// </summary>
            [Description("库房原因")]
            StorehouseUnusual = 11,
            /// <summary>
            /// 快递原因
            /// </summary>
            [Description("快递原因")]
            ExpressUnusual = 12,
            /// <summary>
            /// 系统原因
            /// </summary>
            [Description("系统原因")]
            SystemUnusual = 13
        }

        /// <summary>
        /// 延误类型
        /// </summary>
        public enum DelayType
        {
            /// <summary>
            /// 操作失误
            /// </summary>
            [Description("操作失误")]
            Misoperation = 1,
            /// <summary>
            /// 线路问题
            /// </summary>
            [Description("线路问题")]
            LineUnusual = 2,
            /// <summary>
            /// 承运商异常
            /// </summary>
            [Description("承运商异常")]
            CarrierUnusual = 3,
            /// <summary>
            /// 火车异常
            /// </summary>
            [Description("火车异常")]
            TrainUnusual = 4,
            /// <summary>
            /// 飞机异常
            /// </summary>
            [Description("飞机异常")]
            PlanUnusual = 5,
            /// <summary>
            /// 快递原因
            /// </summary>
            [Description("快递原因")]
            ExpressUnusual = 6,
            /// <summary>
            /// 天气原因
            /// </summary>
            [Description("天气原因")]
            WeatherUnusual = 7,
            /// <summary>
            /// 交通原因
            /// </summary>
            [Description("交通原因")]
            TrafficUnusual = 8,
            /// <summary>
            /// 库房原因
            /// </summary>
            [Description("库房原因")]
            StorehouseUnusual = 9,
            /// <summary>
            /// 其他原因
            /// </summary>
            [Description("其他原因")]
            Other = 10
        }

        /// <summary>
        /// 箱子类型
        /// </summary>
        public enum BoxType
        {
            [Description("普通")]
            Normal = 1,
            [Description("其他")]
            Other = 2
        }

        /// <summary>
        /// 预调度批次处理状态
        /// </summary>
        [Description("批次预调度处理状态")]
        public enum BatchPreDispatchedStatus
        {
            /// <summary>
            /// 未预调度
            /// </summary>
            [Description("未预调度")]
            NoDispatched = 0,

            /// <summary>
            /// 已预调度
            /// </summary>
            [Description("已预调度")]
            IsDispatched = 1,

            /// <summary>
            /// 预调度失败
            /// </summary>
            [Description("预调度失败")]
            DispatchedError = 2
        }


        /// <summary>
        /// 预调度状态
        /// </summary>
        public enum DispatchStatus
        {
            /// <summary>
            /// 可调度
            /// </summary>
            [Description("可调度")]
            CanDispatch = 1,
            /// <summary>
            /// 不可调度
            /// </summary>
            [Description("不可调度")]
            CanNotDispatch = 2,
            /// <summary>
            /// 已调度
            /// </summary>
            [Obsolete("采用新的预调度方式，次状态停用")]
            [Description("已调度")]
            Dispatched = 3,
            /// <summary>
            /// 已作废
            /// </summary>
            [Obsolete("采用新的预调度方式，次状态停用")]
            [Description("已作废")]
            Invalid = 4,
            /// <summary>
            /// 可预调度 被预知下一条线路可被调度 生成提货单时改下一条
            /// </summary>
            [Description("可预调度")]
            EstiDispatch = 5,
            /// <summary>
            /// 待调度中 生成提货单改
            /// </summary>
            [Description("待调度中")]
            EstiDispatching = 6,
            /// <summary>
            /// 调度中 确认调度改
            /// </summary>
            [Description("调度中")]
            Dispatching = 7,
            /// <summary>
            /// 调度完成 确认到货改
            /// </summary>
            [Description("调度完成")]
            DispatchSuccess = 8,
        }

        /// <summary>
        /// 承运商适用范围
        /// </summary>
        public enum CarrierCoverage
        {
            /// <summary>
            /// 全国
            /// </summary>
            [Description("00")]
            All,
            /// <summary>
            /// 非全国
            /// </summary>
            [Description("01")]
            NotAll
        }

        /// <summary>
        /// 编号类型
        /// </summary>
        public enum SerialNumberType
        {
            /// <summary>
            /// 线路编号
            /// </summary>
            [Description("{0}-{1}-{2}-{3}-{4}-{5}")]
            LineCode = 0,
            /// <summary>
            /// 提货单号
            /// </summary>
            [Description("LN{0}{1}")]
            DeliveryNo = 1,
            /// <summary>
            /// 承运商编号
            /// </summary>
            [Description("Y{0}-{1}")]
            CarrierNo = 2,
            /// <summary>
            /// 出库批次号
            /// </summary>
            [Description("{0}{1}")]
            OutBatchNo = 3,

            /// <summary>
            /// 分拣装箱号
            /// </summary>
            [Description("{0}{1}{2}")]
            SortingBoxNo = 4
        }

        /// <summary>
        /// 综合报表提货单状态
        /// </summary>
        public enum ComplexReportDeliveryStatus
        {
            /// <summary>
            /// 待调度
            /// </summary>
            [Description("待调度")]
            PreDispatched = 0,
            /// <summary>
            /// 无法识别
            /// </summary>
            [Description("无法识别")]
            CannotRecognize = 101,
            /// <summary>
            /// 已调度
            /// </summary>
            [Description("已调度")]
            Dispatched = 1,
            /// <summary>
            /// 发货在途
            /// </summary>
            [Description("发货在途")]
            InTransit = 2,
            /// <summary>
            /// 准时到货
            /// </summary>
            [Description("准时到货")]
            ArrivedOnTime = 3,
            /// <summary>
            /// 到货延误
            /// </summary>
            [Description("到货延误")]
            ArrivedDelay = 4,
            /// <summary>
            /// 全部丢失
            /// </summary>
            [Description("全部丢失")]
            AllLost = 5,
            /// <summary>
            /// KPI已审核
            /// </summary>
            [Description("KPI已审核")]
            KPIApproved = 6
        }

        /// <summary>
        /// KPI延误计费类型
        /// </summary>
        [Description("KPI延误计费类型")]
        public enum KPIDelayType
        {
            /// <summary>
            /// 延误折扣
            /// </summary>
            [Description("延误折扣")]
            DelayDiscount = 0,
            /// <summary>
            /// 延误扣款
            /// </summary>
            [Description("延误扣款")]
            DelayAmount = 1
        }



        /// <summary>
        /// 提货单状态
        /// </summary>
        public enum DeliveryStatus
        {
            /// <summary>
            /// 待调度
            /// </summary>
            [Description("待调度")]
            WaitForDispatch = 0,
            /// <summary>
            /// 已调度
            /// </summary>
            [Description("已调度")]
            Dispatched = 1,
            /// <summary>
            /// 发货在途
            /// </summary>
            [Description("发货在途")]
            InTransit = 2,
            /// <summary>
            /// 准时到货
            /// </summary>
            [Description("准时到货")]
            ArrivedOnTime = 3,
            /// <summary>
            /// 到货延误
            /// </summary>
            [Description("到货延误")]
            ArrivedDelay = 4,
            /// <summary>
            /// 全部丢失
            /// </summary>
            [Description("全部丢失")]
            AllLost = 5,
            /// <summary>
            /// KPI已审核
            /// </summary>
            [Description("KPI已审核")]
            KPIApproved = 6
        }

        /// <summary>
        /// 到货延误复议申请处理状态
        /// </summary>
        public enum DelayHandleApproveStatus
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [Description("未审核")]
            NotApprove = 0,
            /// <summary>
            /// 审核通过
            /// </summary>
            [Description("审核通过")]
            ApprovePasss = 1,
            /// <summary>
            /// 审核未通过
            /// </summary>
            [Description("审核未通过")]
            ApproveNOPasss = 2,
        }

        public enum TransPortPlanDeadLineType
        {
            /// <summary>
            /// 自定义
            /// </summary>
            [Description("自定义")]
            UnDefined = 0,
            /// <summary>
            /// 一天
            /// </summary>
            [Description("一天")]
            OneDay = 1,
            /// <summary>
            /// 一周
            /// </summary>
            [Description("一周")]
            OneWeek = 2,
            /// <summary>
            /// 一月
            /// </summary>
            [Description("一月")]
            OneMonth = 3,
            /// <summary>
            /// 一季度
            /// </summary>
            [Description("一季度")]
            OneSeason = 4,
            /// <summary>
            /// 一年
            /// </summary>
            [Description("一年")]
            OneYear = 5
        }

        /// <summary>
        /// 订单TMS状态
        /// </summary>
        public enum OrderTMSStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Normal = 0,
            /// <summary>
            /// 丢失
            /// </summary>
            [Description("丢失")]
            Lost = 1,
            /// <summary>
            /// TMS完成
            /// </summary>
            [Description("TMS完成")]
            Finished = 2
        }

        /// <summary>
        /// 营业类型
        /// </summary>
        public enum BusinessType
        {
            /// <summary>
            /// 自营
            /// </summary>
            [Description("自营")]
            SelfManagement = 0,
            /// <summary>
            /// 外包
            /// </summary>
            [Description("外包")]
            Outsourcing = 1
        }
        /// <summary>
        /// 箱子状态
        /// </summary>
        public enum BoxStatus
        {
            /// <summary>
            /// 未装箱
            /// </summary>
            [Description("未装箱")]
            NoBox = 0,
            /// <summary>
            /// 已装箱
            /// </summary>
            [Description("已装箱")]
            Boxed = 1,
            /// <summary>
            /// 已拆箱
            /// </summary>
            [Description("已拆箱")]
            UnBox = 2
        }
        /// <summary>
        /// 箱子内订单丢失状态
        /// </summary>
        public enum BoxLostStatus
        {
            /// <summary>
            /// 没有丢失订单
            /// </summary>
            [Description("没有丢失订单")]
            None = 0,
            /// <summary>
            /// 包含丢失订单
            /// </summary>
            [Description("包含丢失订单")]
            Has = 1,
            /// <summary>
            /// 整箱丢失
            /// </summary>
            [Description("整箱丢失")]
            AllLost = 2,
        }

        /// <summary>
        /// 系统模块
        /// </summary>
        public enum SysModule
        {
            /// <summary>
            /// 承运商合同模块
            /// </summary>
            [Description("承运商合同模块")]
            CarrierModule = 1,

            /// <summary>
            /// 线路模块
            /// </summary>
            [Description("线路模块")]
            LineModule = 2,

            /// <summary>
            /// 运输计划模块
            /// </summary>
            [Description("运输计划模块")]
            TransPortPlanModule = 3

        }

        /// <summary>
        /// 调度页面的状态
        /// </summary>
        public enum DispatchingPageStatus
        {
            /// <summary>
            /// 待调度
            /// </summary>
            [Description("待调度")]
            PreDispatched = 1,
            /// <summary>
            /// 已调度
            /// </summary>
            [Description("已调度")]
            Dispatched = 2,
            /// <summary>
            /// 无法识别
            /// </summary>
            [Description("无法识别")]
            CannotRecognize = 3,
        }

        /// <summary>
        /// 丢失类型
        /// </summary>
        [Description("丢失类型")]
        public enum LostType
        {
            /// <summary>
            /// 实质丢失
            /// </summary>
            [Description("实质丢失")]
            SubstanceLost = 0,
            /// <summary>
            /// 破损拒收
            /// </summary>
            [Description("破损拒收")]
            DamagedRejection = 1,
            /// <summary>
            /// 淋湿拒收
            /// </summary>
            [Description("淋湿拒收")]
            WetRejection = 2,
            /// <summary>
            /// 污损拒收
            /// </summary>
            [Description("污损拒收")]
            DefacedRejection = 3
        }

        /// <summary>
        /// 报表种类
        /// </summary>
        [Description("报表种类")]
        public enum ReportCategory
        {
            /// <summary>
            /// 综合报表
            /// </summary>
            [Description("综合报表")]
            ComplexReport = 0
        }

        /// <summary>
        /// 分拣操作单类型
        /// </summary>
        [Description("分拣操作单类型")]
        [Serializable]
        public enum SortCenterFormType
        {
            /// <summary>
            /// 按运单
            /// </summary>
            [Description("按运单")]
            Waybill = 0,
            /// <summary>
            /// 按订单
            /// </summary>
            [Description("按订单")]
            Order = 1,
            /// <summary>
            /// 按配送单号
            /// </summary>
            [Description("按配送单号")]
            DeliverCode = 2
        }

        /// <summary>
        /// 运单状态变更发生环节
        /// </summary>
        public enum StatusChangeNodeType
        {
            /// <summary>
            /// 商家
            /// </summary>
            [Description("商家")]
            Merchant = 1,

            /// <summary>
            /// 分拣中心
            /// </summary>
            [Description("分拣中心")]
            DeliverCenter = 5,

            /// <summary>
            /// 运输中心
            /// </summary>
            [Description("运输中心")]
            TransportCenter = 10,

            /// <summary>
            /// 配送站
            /// </summary>
            [Description("配送站")]
            Station = 15,

            /// <summary>
            /// 第三方配送
            /// </summary>
            [Description("第三方配送")]
            ThirdPartStation = 20,

            /// <summary>
            /// 退货仓库
            /// </summary>
            [Description("退货仓库")]
            ReturnWareHouse = 25,

            /// <summary>
            /// 未知
            /// </summary>
            [Description("未知")]
            NotType = 30
        }

        /// <summary>
        /// 短信模板替换
        /// </summary>
        [Description("短信模板替换")]
        public enum SMSTemplateReplaceLabel
        {
            /// <summary>
            /// [当前城市]
            /// </summary>
            [Description("[当前城市]")]
            DepartureCity = 0,

            /// <summary>
            /// [目标城市]
            /// </summary>
            [Description("[目标城市]")]
            ArrivalCity = 1,

            /// <summary>
            /// [目标部门]
            /// </summary>
            [Description("[目标部门]")]
            ArrivalDept = 2
        }


        /// <summary>
        /// 分拣出入库操作类型
        /// </summary>
        [Description("分拣出入库操作类型")]
        public enum SortCenterOperateType
        {
            /// <summary>
            /// 发货分拣到站点
            /// </summary>
            [Description("发货分拣")]
            SimpleSorting = 0,
            /// <summary>
            /// 转站分拣
            /// </summary>
            [Description("转站分拣")]
            TurnSorting = 1,
            /// <summary>
            /// 二级分拣
            /// </summary>
            [Description("二级分拣")]
            SecondSorting = 2,
            /// <summary>
            /// 分拣至配送商
            /// </summary>
            [Description("分拣至配送商")]
            DistributionSorting = 3
        }



        /// <summary>
        /// lms同步到tms操作类型
        /// </summary>
        [Description("lms同步到tms操作类型")]
        public enum Lms2TmsOperateType
        {
            /// <summary>
            /// 置为无效
            /// </summary>
            [Description("置为无效")]
            SetInvalid = 0,

            /// <summary>
            /// 数据进入Waybill的功能
            /// 系统对接，同步服务，分发服务等
            /// </summary>
            [Description("订单入口服务")]
            BillSyncService = 1,

            /// <summary>
            /// 分配站点
            /// GIS分配，人工分配，修改分配等等会修改配送站点的功能
            /// </summary>
            [Description("分配站点")]
            AssignStation = 2,

            /// <summary>
            /// 面单打印
            /// </summary>
            [Description("面单打印")]
            BillPrint = 3,

            /// <summary>
            /// 入库
            /// </summary>
            [Description("入库")]
            Inbound = 4,

            /// <summary>
            /// 出库
            /// </summary>
            [Description("出库")]
            Outbound = 5,

            /// <summary>
            /// 运单装车
            /// </summary>
            [Description("运单装车")]
            BillLoading = 6,

            /// <summary>
            /// 返货交接单打印
            /// 改为：返货在途状态
            /// </summary>
            [Description("返货交接单打印")]
            ReturnPrint = 7,

            /// <summary>
            /// 置为返货入库状态的功能
            /// </summary>
            [Description("返货入库")]
            ReturnInbound = 8,

            /// <summary>
            /// 改为拒收入库
            /// 改为退换货入库
            /// </summary>
            [Description("外商订单退库")]
            MerchantBillRefund = 9,

            /// <summary>
            /// 转站申请
            /// </summary>
            [Description("转站申请")]
            TurnStationApply = 10,

            /// <summary>
            /// 跳过面单打印服务
            /// </summary>
            [Description("跳过面单打印服务")]
            SkipBillPrint = 11,
            /// <summary>
            /// 运单下车
            /// </summary>
            [Description("运单下车")]
            BillGetOff = 12,

            /// <summary>
            /// 转站打印
            /// </summary>
            [Description("转站打印")]
            TurnStationPrint = 13,
            /// <summary>
            /// 置为返货入库状态的功能
            /// </summary>
            [Description("返货出库")]
            ReturnOutbound = 14,

            /// <summary>
            /// TMS数据入口
            /// </summary>
            [Description("TMS数据入口")]
            TmsDataEntrance = 15,

            /// <summary>
            /// 分拣装箱
            /// </summary>
            [Description("分拣装箱")]
            SortingPacking = 16
        }

        /// <summary>
        /// tms操作类型
        /// </summary>
        [Description("tms操作类型")]
        public enum TmsOperateType
        {
            /// <summary>
            /// 面单打印（包括打印 和 称重）
            /// </summary>
            [Description("面单打印")]
            WeighPrint = 0,
            /// <summary>
            /// 重新称重
            /// </summary>
            [Description("重新称重")]
            ReWeigh = 1,
            /// <summary>
            /// 入库
            /// </summary>
            [Description("入库")]
            Inbound = 2,
            /// <summary>
            /// 装箱
            /// </summary>
            [Description("分拣装箱")]
            Packing = 3,
            /// <summary>
            /// 出库
            /// </summary>
            [Description("出库")]
            Outbound = 4,
            /// <summary>
            /// 运单装车
            /// </summary>
            [Description("运单装车")]
            BillLoading = 5,
            /// <summary>
            /// 返货入库
            /// </summary>
            [Description("返货入库")]
            ReturnInbound = 6,
            /// <summary>
            /// 返货入库
            /// </summary>
            [Description("外商订单退库")]
            MerchantBillRefund = 7,
            /// <summary>
            /// 配送商装车
            /// </summary>
            [Description("配送商装车")]
            DistributionLoading = 8,
            /// <summary>
            /// 运单下车
            /// </summary>
            [Description("运单下车")]
            BillGetOff = 9,

            /// <summary>
            /// 入库【不限制站点】
            /// </summary>
            [Description("入库【不限制站点】")]
            Inbound_NoLimitedStation = 10,
            /// <summary>
            /// 退货分拣称重装箱
            /// </summary>
            [Description("退货分拣称重装箱")]
            BillReturnBox = 11,
            /// <summary>
            /// 退货分拣称重装箱
            /// </summary>
            [Description("退货分拣称重交接表打印")]
            ReturnBoxPrintBackForm = 12,
            /// <summary>
            /// 退货分拣称重装箱
            /// </summary>
            [Description("退货分拣称重装箱打印")]
            ReturnBoxPrintBackPacking = 13,
            /// <summary>
            /// 返货出库
            /// </summary>
            [Description("返货出库")]
            ReturnOutbound = 14,
            /// <summary>
            /// 退库单入库
            /// </summary>
            [Description("退库单入库")]
            ReturnBillInbound = 15,
            /// <summary>
            /// 拒收入库
            /// </summary>
            [Description("拒收入库")]
            RejectBillInbound = 16,
            /// <summary>
            /// 商家入库确认
            /// </summary>
            [Description("商家入库确认")]
            MerchantReturnBillInbound = 17,

            /// <summary>
            /// 发货在途
            /// </summary>
            [Description("发货在途")]
            IntercityInTransit = 18,

            /// <summary>
            /// 到货确认
            /// </summary>
            [Description("到货确认")]
            IntercityArrivaled = 19,

            /// <summary>
            /// 面单校验
            /// </summary>
            [Description("面单校验")]
            BillValidate = 20,

			/// <summary>
			/// 批量入库
			/// </summary>
			[Description("批量入库")]
			BatchInbound = 21,

			/// <summary>
			/// 批量出库
			/// </summary>
			[Description("批量出库")]
			BatchOutbound = 22

        }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Description("运单状态")]
        public enum BillStatus
        {
            /// <summary>
            /// 无效
            /// </summary>
            [Description("无效")]
            InvalidBill = -9,
            /// <summary>
            /// 有效待分配
            /// </summary>
            [Description("有效待分配")]
            ValidWaitingAssign = -5,
            /// <summary>
            /// 已分配站点
            /// </summary>
            [Description("已分配站点")]
            GisAssigned = -4,
            /// <summary>
            /// 待入库
            /// </summary>
            [Description("待入库")]
            WaitingInbound = -3,
            /// <summary>
            /// 已入库(未使用) 已装箱(新)
            /// </summary>
            [Description("已入库，已装箱(新)")]
            Inbounded = -2,
            /// <summary>
            /// 已分拣， 已入库(新)
            /// </summary>
            [Description("已分拣, 已入库(新)")]
            HaveBeenSorting = -1,
            /// <summary>
            /// 运输在途
            /// </summary>
            [Description("运输在途")]
            InTransit = 0,
            /// <summary>
            /// 已入站
            /// </summary>
            [Description("已入站")]
            InStation = 1,
            /// <summary>
            /// 已分配配送员
            /// </summary>
            [Description("已分配配送员")]
            Assigned = 2,
            /// <summary>
            /// 妥投
            /// </summary>
            [Description("妥投")]
            DeliverySuccess = 3,
            /// <summary>
            /// 滞留
            /// </summary>
            [Description("滞留")]
            Delay = 4,
            /// <summary>
            /// 拒收
            /// </summary>
            [Description("拒收")]
            Rejected = 5,
            /// <summary>
            /// 转站中
            /// </summary>
            [Description("转站中")]
            TransferringStation = 8,
            /// <summary>
            /// 已出库
            /// </summary>
            [Description("已出库")]
            Outbounded = 10,
            /// <summary>
            /// 配送中
            /// </summary>
            [Description("配送中")]
            Delivering = 22,
            /// <summary>
            /// 退换货入库
            /// </summary>
            [Description("退换货入库")]
            ReturnBounded = 6,
            /// <summary>
            /// 拒收入库
            /// </summary>
            [Description("拒收入库")]
            RefusedBounded = 7,
            /// <summary>
            /// 返货在途
            /// </summary>
            [Description("返货在途")]
            ReturnOnStation = 11,

            /// <summary>
            /// 返货入库
            /// </summary>
            [Description("返货入库")]
            ReturnInBound = 12,
            /// <summary>
            /// 签单返回入库
            /// </summary>
            [Description("签单返回入库")]
            ReturnSignedBounded = 13,

            /// <summary>
            /// 城际已发货
            /// </summary>
            [Description("城际已发货")]
            IntercityInTransit = 201,

            /// <summary>
            /// 城际到货
            /// </summary>
            [Description("城际到货")]
            IntercityArrivaled = 202,

            /// <summary>
            /// 分配配送商
            /// </summary>
            [Description("分配配送商")]
            AssignDistribution = 20,

            /// <summary>
            /// 分配配送站
            /// </summary>
            [Description("分配配送站")]
            AssignStation = 30

        }

        /// <summary>
        /// 代理类型
        /// </summary>
        [Description("代理类型")]
        public enum AgentType
        {
            /// <summary>
            /// 没有代理
            /// </summary>
            [Description("没有代理")]
            NoAgent = 0,
            /// <summary>
            /// 单用户代理
            /// </summary>
            [Description("单用户代理")]
            SingleAgent = 1
        }

        /// <summary>
        /// 运单类型
        /// </summary>
        [Description("运单类型")]
        public enum BillType
        {
            /// <summary>
            /// 普通订单
            /// </summary>
            [Description("普通订单")]
            Normal = 0,
            /// <summary>
            /// 上门换货单
            /// </summary>
            [Description("上门换货单")]
            Exchange = 1,
            /// <summary>
            /// 上门退货单
            /// </summary>
            [Description("上门退货单")]
            Return = 2,
            /// <summary>
            /// 签单返回
            /// </summary>
            [Description("签单返回")]
            SignReturn = 3
        }


        /// <summary>
        /// 逆向状态
        /// </summary>
        [Description("逆向状态")]
        public enum ReturnStatus
        {
            /// <summary>
            /// 返货在途
            /// </summary>
            [Description("返货在途")]
            ReturnInTransit = -1,
            /// <summary>
            /// 返货入库
            /// </summary>
            [Description("返货入库")]
            ReturnInbounded = 1,
            /// <summary>
            /// 退换货入库
            /// </summary>
            [Description("退换货入库")]
            ReturnExchangeInbounded = 6,
            /// <summary>
            /// 拒收入库
            /// </summary>
            [Description("拒收入库")]
            RejectedInbounded = 7,
            /// <summary>
            /// 签单返回入库
            /// </summary>
            [Description("签单返回入库")]
            ReturnSignedBounded = 13
        }

        /// <summary>
        /// 货品属性
        /// </summary>
        [Description("货品属性")]
        public enum BillGoodsType
        {
            /// <summary>
            /// 普货
            /// </summary>
            [Description("普货")]
            Normal = 0,
            /// <summary>
            /// 禁航
            /// </summary>
            [Description("禁航")]
            NoFly = 1,
            /// <summary>
            /// 易碎
            /// </summary>
            [Description("易碎")]
            Frangible = 2
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        public enum PayType
        {
            /// <summary>
            /// POS机
            /// </summary>
            [Description("POS机")]
            POS = 0,
            /// <summary>
            /// 现金
            /// </summary>
            [Description("现金")]
            Cash = 1
        }

        /// <summary>
        /// TMS数据入口来源
        /// </summary>
        [Description("TMS数据入口来源")]
        public enum TMSEntranceSource
        {
            /// <summary>
            /// VANCL WMS系统
            /// </summary>
            [Description("VANCL WMS")]
            VANCL = 0,

            /// <summary>
            /// VJIA WMS系统
            /// </summary>
            [Description("VJIA WMS")]
            VJIA = 1,

            /// <summary>
            /// 如风达系统分拣出库
            /// </summary>
            [Description("分拣出库")]
            SortingOutbound = 2,

            /// <summary>
            /// 分拣装箱出库
            /// </summary>
            [Description("装箱出库")]
            SortingPacking = 3
        }

        /// <summary>
        /// 运单来源
        /// </summary>
        [Description("运单来源")]
        public enum BillSource
        {
            /// <summary>
            /// VANCL
            /// </summary>
            [Description("VANCL")]
            VANCL = 0,
            /// <summary>
            /// VJIA
            /// </summary>
            [Description("VJIA")]
            VJIA = 1,
            /// <summary>
            /// 其他
            /// </summary>
            [Description("其他")]
            Others = 2,
        }

        /// <summary>
        /// 同步状态
        /// </summary>
        [Description("同步状态")]
        public enum SyncStatus
        {
            /// <summary>
            /// 已处理通知
            /// </summary>
            [Description("已处理通知")]
            HasDealNoticeFail = -3,
            /// <summary>
            /// 已处理通知
            /// </summary>
            [Description("已处理通知")]
            HasDealNotice = -2,
            /// <summary>
            /// 已收到通知
            /// </summary>
            [Description("已收到通知")]
            HasReceiveNotice = -1,
            /// <summary>
            /// 未同步
            /// </summary>
            [Description("未同步")]
            NotYet = 0,
            /// <summary>
            /// 已同步
            /// </summary>
            [Description("已同步")]
            Already = 1,
            /// <summary>
            /// 同步出错
            /// </summary>
            [Description("同步出错")]
            Error = 2
        }


        /// <summary>
        /// 队列处理状态
        /// </summary>
        [Description("队列处理状态")]
        public enum SeqStatus
        {
            /// <summary>
            /// 未处理
            /// </summary>
            [Description("未处理")]
            NoHand = 0,
            /// <summary>
            /// 已处理
            /// </summary>
            [Description("已处理")]
            Handed = 1,
            /// <summary>
            /// 错误
            /// </summary>
            [Description("错误")]
            Error = 2
        }


        /// <summary>
        /// 服务日志类型
        /// </summary>
        [Description("服务日志类型")]
        public enum ServiceLogType
        {
            /// <summary>
            /// LMS同步到TMS服务
            /// </summary>
            [Description("LMS同步到TMS服务")]
            Lms2TmsSync = 0,
            /// <summary>
            /// TMS同步到LMS服务
            /// </summary>
            [Description("TMS同步到LMS服务")]
            Tms2LmsSync = 1
        }

        /// <summary>
        /// 服务日志处理状态
        /// </summary>
        [Description("服务日志处理状态")]
        public enum ServiceLogProcessingStatus
        {
            /// <summary>
            /// 未处理
            /// </summary>
            [Description("未处理")]
            NotYet = 0,

            /// <summary>
            /// 已处理
            /// </summary>
            [Description("已处理")]
            Handled = 1
        }

        /// <summary>
        /// 部门类型
        /// </summary>
        public enum CompanyFlag
        {
            /// <summary>
            /// 行政部门
            /// </summary>
            [Description("行政部门")]
            Administration = 0,
            /// <summary>
            /// 分拣中心
            /// </summary>
            [Description("分拣中心")]
            SortingCenter = 1,
            /// <summary>
            /// 配送站
            /// </summary>
            [Description("配送站")]
            DistributionStation = 2,
            /// <summary>
            /// 配送商
            /// </summary>
            [Description("配送商")]
            Distributor = 3,
            /// <summary>
            /// 运输中心
            /// </summary>
            [Description("运输中心")]
            TransportationCenter = 4,
            /// <summary>
            /// 商家
            /// </summary>
            [Description("商家")]
            Merchant = 5
        }

        /// <summary>
        /// 用户权限类型
        /// </summary>
        public enum UserAuthorityType
        {
            /// <summary>
            /// 只读权限
            /// </summary>
            [Description("只读权限")]
            ReadOnly = 0,
            /// <summary>
            /// 操作权限
            /// </summary>
            [Description("操作权限")]
            Operate = 1
        }

        /// <summary>
        /// 系统类别
        /// </summary>
        public enum SpecificSystem
        {
            /// <summary>
            /// 权限管理系统
            /// </summary>
            [Description("权限管理系统")]
            PMS = 0,
            /// <summary>
            /// 配送管理系统
            /// </summary>
            [Description("配送管理系统")]
            LMS = 1,
            /// <summary>
            /// 费用管理系统
            /// </summary>
            [Description("费用管理系统")]
            OA = 2,
            /// <summary>
            /// 拣运管理系统
            /// </summary>
            [Description("拣运管理系统")]
            TMS = 4,
            /// <summary>
            /// 综合报表管理系统
            /// </summary>
            [Description("综合报表管理系统")]
            BI = 6
        }

        /// <summary>
        /// 箱子出库状态
        /// </summary>
        public enum BoxOutBoundedStatus
        {
            /// <summary>
            /// 箱子不存在
            /// </summary>
            [Description("箱子不存在")]
            NotExists = 0,
            /// <summary>
            /// 没有出库
            /// </summary>
            [Description("没有出库")]
            NotYet = 1,
            /// <summary>
            /// 已出库
            /// </summary>
            [Description("已出库")]
            Outbounded = 2
        }

        /// <summary>
        /// 提货单来源
        /// </summary>
        public enum DeliverySource
        {
            /// <summary>
            /// 对接
            /// </summary>
            [Description("系统对接")]
            Interfacing = 1,

            /// <summary>
            /// 导入
            /// </summary>
            [Description("提货单导入")]
            Import = 2,

            /// <summary>
            /// 其他
            /// </summary>
            [Description("其他")]
            Others = 3,

            /// <summary>
            /// 订单明细系统对接
            /// </summary>
            [Description("订单明细系统对接")]
            OrderInterfacing = 4,

            /// <summary>
            /// 订单明细系统对接
            /// </summary>
            [Description("扫描生成提货单")]
            Scan = 5,

            /// <summary>
            /// 订单明细系统对接
            /// </summary>
            [Description("查询生成提货单")]
            Search = 6

        }
        /// <summary>
        /// COD操作类型
        /// </summary>
        public enum CODOperateType
        {
            [Description("发货")]
            Delivery = 1,
            [Description("转站转配送商（出）")]
            TransferOUT = 2,
            [Description("转站转配送商（入）")]
            TransferIN = 3,
            [Description("拒收")]
            Rejection = 4,
            [Description("置运单无效")]
            Invalid = 5,
            [Description("逆向返货入库")]
            ReverseInbound = 6,
            [Description("出库")]
            OutBound = 7,
            [Description("装车")]
            TruckIn = 8
        }
		/// <summary>
		/// 出库交接表查询时间类型
		/// </summary>
	    public enum OutboundPrintTimeType
	    {
			[Description("出库时间")]
			OutboundTime = 0,
			[Description("批次打印时间")]
			BatchPrintTime = 1
	    }

        public enum TurnType
        {
            //转类型0为没有转部门，1为部门转，2为配送商转
            NoTurn = 0,
            ToDep = 1,
            ToDistribution = 2
        }
    }
}
