using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LadingBill
{

    public class TaskViewModel : BaseSearchModel
    {

        [Description("提货价格")]
        public decimal ORDERAMOUNT { get; set; }


        [Description("提货价格")]
        public decimal ONCEAMOUNT { get; set; }


        [Description("提货价格类型")]
        public int PICKPRICETYPE { get; set; }

        [Description("任务下达公司")]
        public string fromdistributionname { get; set; }

        [Description("提货人")]
        public string PICKMAN { get; set; }


        [Description("预计提货时间")]
        public DateTime PREDICTTIME { get; set; }


        [Description("预计提货重量")]
        public decimal PREDICTWEIGHT { get; set; }


        [Description("预计单量")]
        public decimal PREDICTORDERQUANTITY { get; set; }


        [Description("任务编号")]
        public virtual string taskcode { get; set; }


        [Description("提货公司")]
        public virtual string todistributioncode { get; set; }


        [Description("公里")]
        public virtual string mileage { get; set; }

        [Description("应付提货费用")]
        public virtual decimal pickgoodsamount { get; set; }


        [Description("任务id")]
        public virtual string id { get; set; }


        [Description("接受邮箱")]
        public virtual string receiveemail { get; set; }

        [Description("提货完成时间")]
        public virtual DateTime FINISHTIME { get; set; }



        [Description("实际提货单量")]
        public virtual decimal orderquantity { get; set; }


        [Description("实际重量")]
        public virtual decimal weight { get; set; }

        [Description("提货时间")]
        public virtual DateTime TASKTIME { get; set; }



        [Description("任务状态")]
        public virtual int taskstatus { get; set; }


        [Description("提货部门")]
        public virtual string department { get; set; }


        [Description("商家名称")]
        public virtual string merchantname { get; set; }

        [Description("联系人")]
        public virtual string LINKMAN { get; set; }

        [Description("联系电话")]
        public virtual string phone { get; set; }


        [Description("仓库地址")]
        public virtual string warehouseaddress { get; set; }

        [Description("仓库名称")]
        public virtual string warehousename { get; set; }

        [Description("提货公司")]
        public virtual string distributionname { get; set; }


    }
}
