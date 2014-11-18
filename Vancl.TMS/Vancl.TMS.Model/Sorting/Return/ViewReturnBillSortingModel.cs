using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 返货入库view对象
    /// </summary>
    public class ViewReturnBillSortingModel:BaseModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public long BillReturnInfoId { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public long FormCode { get; set; }
        /// <summary>
        /// 运单来源
        /// </summary>
        public string BillSource { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal NeedAmount { get; set; }
        /// <summary>
        /// 应退金额
        /// </summary>
        public decimal NeedBackAmount { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string BillType { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 封箱帖号
        /// </summary>
        public string BoxLabelNo { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string OpTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string OpMessage { get; set; }

        /// <summary>
        /// 返回结果，0失败,1成功,2重量不符,3箱号不符
        /// </summary>
        public int Code { get; set; }

        private bool isBox = false;
        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool IsBox
        {
            get { return isBox; }
            set { isBox = value; }
        }
    }
}
