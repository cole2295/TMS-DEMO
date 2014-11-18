using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Vancl.TMS.Util.Extensions;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库打印导出模型
    /// </summary>
    public class OutboundPrintExportModel : IExportXlsable
    {
        [ExportXls("序号")]
        public int No { get; set; }

        [ExportXls("批次号")]
        public string BatchNo { get; set; }

        [ExportXls("订单号")]
        public string FormCode { get; set; }

        [ExportXls("收货人电话")]
        public string ReceiveMobile { get; set; }

        [ExportXls("仓库")]
        public string WarehouseName { get; set; }

        [ExportXls("配送站点")]
        public string CompanyName { get; set; }

        [ExportXls("订单类型")]
        public string Source { get; set; }

        [ExportXls("出库时间")]
        public DateTime CreateTime { get; set; }

    }
}
