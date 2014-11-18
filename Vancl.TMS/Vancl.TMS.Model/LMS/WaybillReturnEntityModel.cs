using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    public class WaybillReturnEntityModel
    {
        private long _waybillreturninfoid;
        private long _waybillno;
        private decimal? _weight;
        private string _labelno;
        private string _boxno;
        private int? _createby;
        private DateTime? _createtime = DateTime.Now;
        private int? _updateby;
        private DateTime? _updatetime;
        private int? _boxstatus = 0;
        private string _boxlabelno;

        /// <summary>
        /// 自增ID
        /// </summary>
        public long WaybillReturnInfoID
        {
            set { _waybillreturninfoid = value; }
            get { return _waybillreturninfoid; }
        }
        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo
        {
            set { _waybillno = value; }
            get { return _waybillno; }
        }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            set { _labelno = value; }
            get { return _labelno; }
        }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo
        {
            set { _boxno = value; }
            get { return _boxno; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateBy
        {
            set { _createby = value; }
            get { return _createby; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 更新
        /// </summary>
        public int? UpdateBy
        {
            set { _updateby = value; }
            get { return _updateby; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 装箱状态（0:未装箱，1:已装箱,2:已拆箱）
        /// </summary>
        public int? BoxStatus
        {
            set { _boxstatus = value; }
            get { return _boxstatus; }
        }
        /// <summary>
        /// 封箱帖号
        /// </summary>
        public string BoxLabelNo
        {
            set { _boxlabelno = value; }
            get { return _boxlabelno; }
        }
    }
}
