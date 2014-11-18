using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Loading
{
    public class BillTruckModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 订单装车主键
        /// </summary>
        public string BTID { set; get; }

        /// <summary>
        /// 订单装车批次号
        /// </summary>
        public string BatchNO { set; get; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string FormCode { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string TruckNO { set; get; }

        /// <summary>
        /// GPS编号
        /// </summary>
        public string GPSNO { set; get; }

        /// <summary>
        /// 司机ID
        /// </summary>
        public int DriverID { set; get; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 更新部门
        /// </summary>
        public int UpdateDept { get; set; }

        /// <summary>
        /// 操作用户所在单位名称
        /// </summary>
        public string OpStationName { set; get; }

        /// <summary>
        /// 同步标记
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "SC_BillTruck";
            }
        }

        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "013"; }
        }


        public string SequenceName
        {
            get { return "seq_sc_billtruck_btid"; }
        }

        #endregion
    }
}
