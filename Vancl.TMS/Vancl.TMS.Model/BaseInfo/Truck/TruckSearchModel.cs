using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo.Truck
{
    [Serializable]
    public class TruckSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public  string TruckNO { get; set; }
    }
}
