using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Extensions;

namespace Vancl.TMS.Model.Sorting.CityScan
{
    /// <summary>
    /// 同城单量扫描统计导出模板
    /// </summary>
    public class CityScanExprotModel : IExportXlsable
    {
        [ExportXls("序号")]
        public Int32 SerialNumber { get; set; }

        [ExportXls("批次号")]
        public String BatchNO { get; set; }

        [ExportXls("运单号")]
        public String FormCode { get; set; }

        [ExportXls("配送站点")]
        public String DeliverStationName { get; set; }

        [ExportXls("商家名称")]
        public String MerchantName { get; set; }

        [ExportXls("扫描时间")]
        public String ScanTime { get; set; }

        [ExportXls("应收金额")]
        public String ReceivableAmount { get; set; }
    }
}
