using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Vancl.TMS.Model.Sorting.WeighReview
{
   public class WeighReviewSearchModel :BaseSearchModel
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeCode { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public WeighReviewStatus WeighReviewStatus { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
   public enum WeighReviewStatus
   {
       /// <summary>
       /// 正常
       /// </summary>
       [Description("正常")]
       Normal = 1,
       /// <summary>
       /// 异常
       /// </summary>
       [Description("异常")]
       Abnormal = 2
   }
   public class WeighReviewViewModel
   {
       /// <summary>
       /// 序号
       /// </summary>
       public int SortRowID { get; set; }
       /// <summary>
       /// 复核状态
       /// </summary>
       public WeighReviewStatus WeighReviewStatus { get; set; }
       /// <summary>
       /// 订单号
       /// </summary>
       public string CustomerOrder { get; set; }
       /// <summary>
       /// 运单号
       /// </summary>
       public string FormCode { get; set; }
       /// <summary>
       /// 员工编号
       /// </summary>
       public string EmployeeCode { get; set; }
       /// <summary>
       /// 操作人
       /// </summary>
       public string OperatorName { get; set; }
       /// <summary>
       /// 操作时间
       /// </summary>
       public DateTime OperateTime { get; set; }
   }
}
