using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Vancl.TMS.Tools.VCMRecords.Tool.DataGrid
{

    public enum OrderType {asc =0,desc=1 }

    /// <summary>
    /// 数据表分页对象
    /// </summary>
    public class PageModel
    {
        /// <summary>
        /// 记录的实体类型
        /// </summary>
        public Type entityType;

        /// <summary>
        /// 返回结果
        /// </summary>
        public  List<Object> result;

        /// <summary>
        /// 记录总数(如果count小于0则代表需要计算count)
        /// </summary>
        public int count =-1;

        private DataTable _table;

        /// <summary>
        /// 返回记录的数据表
        /// </summary>
        public DataTable table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                result = Transfer.TableToList(_table, entityType);
            }
        }

        /// <summary>
        /// 当前页 0-(pagecount-1)   从零开始计数
        /// </summary>
        public int currentPage = 0;

        /// <summary>
        /// 页面显示记录数
        /// </summary>
        public int pageSize = 20;

        ///// <summary>
        ///// 数据库连接字符串
        ///// </summary>
        //private string connectionStr;

        /// <summary>
        /// 过滤表达式
        /// </summary>
        public string whereClause="";

        /// <summary>
        /// 表名
        /// </summary>
        public string tableName;

        /// <summary>
        /// 显示列
        /// </summary>
        public Column[] columns;

        /// <summary>
        /// sql参数集合
        /// </summary>
        public SqlParameter[] sqlParameters = new  SqlParameter[]{} ;

        /// <summary>
        /// 排序字段(主要是指row_num需要用的排序字段)
        /// </summary>
        public String[] orderFields;

        /// <summary>
        /// 最后显示的排序
        /// </summary>
        public OrderType ordertype =  OrderType.asc;

        /// <summary>
        /// 页面总数
        /// </summary>
        public int pageCount
        {
            get
            {
                int pagecout = Convert.ToInt32( count/pageSize);
                if (count % pageSize == 0)
                    return pagecout;
                return pagecout + 1;
            }
        }

    }
}
