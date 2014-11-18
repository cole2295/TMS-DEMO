using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Truck
{
    /// <summary>
    /// 车辆基础信息
    /// </summary>
    [Serializable]
    public class TruckBaseInfoModel : BaseModel, IKeyCodeable, IOperateLogable
    {
        /// <summary>
        /// 车辆主键Key
        /// </summary>
        public virtual string TBID { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public virtual string TruckNO { get; set; }

        /// <summary>
        /// GPS编号
        /// </summary>
        public virtual string GPSNO { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public virtual string ProvinceID { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public virtual string CityID { get; set; }

        /// <summary>
        /// 配送商代号
        /// </summary>
        public virtual string DistributionCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public virtual int UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 同步标记
        /// </summary>
        public virtual Enums.SyncStatus SyncFlag { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "TruckBaseInfo";
            }
        }

        public string PrimaryKey
        {
            get { return TBID; }
            set { TBID = value; }
        }

        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "004"; }
        }

        #endregion

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_truckbaseInfo_tbid"; }
        }

        #endregion
    }
}
