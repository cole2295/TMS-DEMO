using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    public class PrintBillNewModel
    {
        private List<KeyValue> _list;
        public PrintBillNewModel()
        {
            _list = new List<KeyValue>();

            _list.Add(new KeyValue()
            {
                Column = "总数",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "订单来源",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "订单号",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "订单类型",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "收件人",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "支付方式",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "手机",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "省",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "市",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "(区)县",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "地址",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "邮编",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "电话",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "应收金额",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "送货时间",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "备注",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "货物名称",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "数量",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "单价",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "金额",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "客户重量",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "箱码",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "发件省",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "发件市",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "发件区",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "发件地址",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "货品属性",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "配送站名称",
                Data = ""
            });
            _list.Add(new KeyValue()
            {
                Column = "称重重量",
                Data = ""
            });
           

        }

        public List<KeyValue> List
        {
            get { return _list; }
        }

        public void setValue(string column, string value)
        {
            foreach (var c in _list)
            {
                if (c.Column == column)
                {
                    c.Data = value;
                }
            }
        }

        public string getValue(string column)
        {
            foreach (var c in _list)
            {
                if (c.Column == column)
                    return c.Data;
            }
            return string.Empty;
        }
    }

    public class KeyValue
    {
        public string Column { get; set; }
        public string Data { get; set; }
    }

   
}
