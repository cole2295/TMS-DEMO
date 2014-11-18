using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Truck;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class TruckViewModel : Vancl.TMS.Model.BaseInfo.Truck.TruckBaseInfoModel
    {
        public TruckViewModel()
        {

        }

        public TruckViewModel(TruckBaseInfoModel model)
        {
            this.CityID = model.CityID;
            this.CreateBy = model.CreateBy;
            this.CreateTime = model.CreateTime;
            this.DistributionCode = model.DistributionCode;
            this.GPSNO = model.GPSNO;
            this.IsDeleted = model.IsDeleted;
            this.ProvinceID = model.ProvinceID;
            this.TBID = model.TBID;
            this.TruckNO = model.TruckNO;
            this.UpdateBy = model.UpdateBy;
            this.UpdateTime = model.UpdateTime;
        }


        [Display(Name = "车辆主键Key")]
        public override string TBID { get; set; }

        [Required]
        [Display(Name = "车牌号")]
        public override string TruckNO { get; set; }

        [Required]
        [Display(Name = "GPS号码")]
        public override string GPSNO { get; set; }

        [Display(Name = "所在省份")]
        public override string ProvinceID { get; set; }

        [Display(Name = "所在城市")]
        public override string CityID { get; set; }

        [Display(Name = "配送商代号")]
        public override string DistributionCode { get; set; }

        [Display(Name = "创建人")]
        public override int CreateBy { get; set; }

        [Display(Name = "创建时间")]
        public override DateTime CreateTime { get; set; }

        [Display(Name = "修改人")]
        public override int UpdateBy { get; set; }

        [Display(Name = "修改时间")]
        public override DateTime UpdateTime { get; set; }

        [Display(Name = "停用状态")]
        public override bool IsDeleted { get; set; }

        [Display(Name = "同步标记")]
        public override Enums.SyncStatus SyncFlag { get; set; }
    }
}