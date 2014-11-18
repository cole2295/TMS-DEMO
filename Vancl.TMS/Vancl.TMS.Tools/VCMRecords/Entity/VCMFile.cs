using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vancl.TMS.Tools.VCMRecords.Entity
{
    /// <summary>
    /// VCM操作的日志
    /// </summary>
    public class VCMFile
    {
        #region 字段

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 关联的记录ID
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public String FileContent { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 文件大小（K/M）
        /// </summary>
        public string SizeLabel { get; set; }

        #endregion

        #region 方法

        public VCMFile()
        {
            ID = -1;
        }

        /// <summary>
        /// 根据DataRow中生成
        /// </summary>
        /// <param name="row"></param>
        public VCMFile(DataRow row)
        {
            if (row.Table.Columns.Contains("ID"))
                this.ID = (int)row["ID"];
            if (row.Table.Columns.Contains("RecordID"))
                this.RecordID = (int)row["RecordID"];
            if (row.Table.Columns.Contains("FileName"))
                this.FileName = row["FileName"].ToString();
            if (row.Table.Columns.Contains("FileContent"))
                this.FileContent = row["FileContent"].ToString();
            if (row.Table.Columns.Contains("UploadTime"))
                this.UploadTime = (DateTime)row["UploadTime"];
            if (row.Table.Columns.Contains("Size"))
                this.Size = (int)row["Size"];
            if (row.Table.Columns.Contains("SizeLabel"))
                this.SizeLabel = row["SizeLabel"].ToString();
        }
        #endregion
    }
}
