using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vancl.TMS.Tools.VCMRecords.Entity
{
    /// <summary>
    /// VCM执行状态
    /// </summary>
    public enum ExecuteStatusType { 未执行 = 0, 已执行 = 1, 执行失败 = 2 }

    /// <summary>
    /// VCM操作记录
    /// </summary>    
    public class VCMRecord
    {
        #region  字段

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long DeliverOrderID { get; set; }

        /// <summary>
        /// 提出人提出人
        /// </summary>
        public String PromoteEmployee { get; set; }

        /// <summary>
        /// 提出时间
        /// </summary>
        public DateTime PromoteTime { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public String OperationEmployee { get; set; }

        /// <summary>
        /// 变更Sql
        /// </summary>
        public string SQL { get; set; }


        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public String Theme { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ExecuteStatusType ExecuteStatus { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public VCMRecord()
        {
            ID = -1;
        }

        /// <summary>
        /// 根据DataRow中生成
        /// </summary>
        /// <param name="row"></param>
        public VCMRecord(DataRow row)
        {
            if (row.Table.Columns.Contains("ID"))
                this.ID = (int)row["ID"];
            if (row.Table.Columns.Contains("PromoteEmployee"))
                this.PromoteEmployee = row["PromoteEmployee"].ToString().Trim();
            if (row.Table.Columns.Contains("PromoteTime"))
                this.PromoteTime = (DateTime)row["PromoteTime"];
            if (row.Table.Columns.Contains("OperationEmployee"))
                this.OperationEmployee = row["OperationEmployee"].ToString().Trim();
            if (row.Table.Columns.Contains("OperationTime"))
                this.OperationTime = (DateTime)row["OperationTime"];
            if (row.Table.Columns.Contains("SQL"))
                this.SQL = row["SQL"].ToString().Trim();
            if (row.Table.Columns.Contains("Theme"))
                this.Theme = row["Theme"].ToString().Trim();
            if (row.Table.Columns.Contains("Description"))
                this.Description = row["Description"] == null ? "" : row["Description"].ToString().Trim();
            if (row.Table.Columns.Contains("ExecuteStatus"))
                this.ExecuteStatus = (ExecuteStatusType)(short)row["ExecuteStatus"];
            if (row.Table.Columns.Contains("DeliverOrderID"))
                this.DeliverOrderID = (long)row["DeliverOrderID"];
        }

        /// <summary>
        /// 把obj转为ExecuteStatusType
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ExecuteStatusType ConvertToExecuteStatusType(Object obj)
        {
            if (obj is String || obj is string)
            {
                return (ExecuteStatusType)Enum.Parse(typeof(ExecuteStatusType), obj.ToString());
            }
            else
            {
                return (ExecuteStatusType)(Convert.ToInt32(obj));
            }
        }

        #endregion


    }
}
