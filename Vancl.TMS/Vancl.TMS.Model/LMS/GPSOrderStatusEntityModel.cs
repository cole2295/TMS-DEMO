using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    public class GPSOrderStatusEntityModel : BaseModel, ISequenceable
    {
        #region Model

        private DateTime? _createtime;
        private string _distphone;
        private string _distributor;
        private string _gpsid;
        private int _id;
        private bool _issyn;
        private string _orderno;
        private string _statestr;
        private string _station;
        private int? _status;
        private DateTime? _time;
        private string _truckno;

        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderno
        {
            set { _orderno = value; }
            get { return _orderno; }
        }

        /// <summary>
        /// 订单状态：1/出库、2/到达配送站、3/派件、4/签收、5/完成、6/二次配送
        /// </summary>
        public int? status
        {
            set { _status = value; }
            get { return _status; }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string statestr
        {
            set { _statestr = value; }
            get { return _statestr; }
        }

        /// <summary>
        /// 派件之前是车牌号，之后是配送人
        /// </summary>
        public string distributor
        {
            set { _distributor = value; }
            get { return _distributor; }
        }

        /// <summary>
        /// 配送人电话
        /// </summary>
        public string distphone
        {
            set { _distphone = value; }
            get { return _distphone; }
        }

        /// <summary>
        /// 状态发生时间
        /// </summary>
        public DateTime? time
        {
            set { _time = value; }
            get { return _time; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createtime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        /// <summary>
        /// gps号
        /// </summary>
        public string gpsid
        {
            set { _gpsid = value; }
            get { return _gpsid; }
        }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string truckno
        {
            set { _truckno = value; }
            get { return _truckno; }
        }

        /// <summary>
        /// 同步
        /// </summary>
        public bool IsSyn
        {
            set { _issyn = value; }
            get { return _issyn; }
        }

        /// <summary>
        /// 转站目的站
        /// </summary>
        public string station
        {
            set { _station = value; }
            get { return _station; }
        }

        #endregion Model

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_GPSORDERSTATUS"; }
        }

        #endregion
    }
}
