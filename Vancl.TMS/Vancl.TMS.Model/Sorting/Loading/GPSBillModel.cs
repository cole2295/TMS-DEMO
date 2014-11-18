using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Loading
{
    public class GPSBillModel
    {
        public string GBID { set; get; }

        public string FormCode { set; get; }

        public string CityName { set; get; }

        public int UpDeptID { set; get; }

        public int DeliverStationID { set; get; }

        public string CustomerAddress { set; get; }

        public string TruckNO { set; get; }

        public string GPSNO { set; get; }

        public int CreateBy { set; get; }

        public DateTime CreateTime { set; get; }
    }
}
