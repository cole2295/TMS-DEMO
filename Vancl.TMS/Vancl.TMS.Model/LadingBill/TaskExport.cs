using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LadingBill
{
    public class TaskExport
    {

        [Description("任务编号")]
        public virtual string taskcode { get; set; }

        [Description("商家")]
        public virtual string merchantname { get; set; }

        [Description("仓库")]
        public virtual string warehousename { get; set; }

        [Description("仓库地址")]
        public virtual string warehouseaddress { get; set; }

        [Description("提货公司")]
        public virtual string distributionname { get; set; }

        [Description("提货部门")]
        public virtual string department { get; set; }

        [Description("任务接受邮箱")]
        public virtual string receiveemail { get; set; }

        [Description("单量")]
        public virtual decimal orderquantity { get; set; }

        [Description("重量kg")]
        public virtual decimal weight { get; set; }

        [Description("公里数")]
        public virtual string mileage { get; set; }

        [Description("提货价格")]
        public string ORDERAMOUNT { get; set; }

        [Description("应付提货费用")]
        public virtual decimal pickgoodsamount { get; set; }

        [Description("计划提货时间")]
        public string  PREDICTTIME { get; set; }


        [Description("提货完成时间")]
        public virtual string FINISHTIME { get; set; }

        [Description("任务状态")]
        public virtual string taskstatus { get; set; }

    }
}
