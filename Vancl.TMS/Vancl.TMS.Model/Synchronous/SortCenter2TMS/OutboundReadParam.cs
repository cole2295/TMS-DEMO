using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Synchronous
{
    /// <summary>
    /// TMS分拣出库同步服务LMS读取数据To文件参数对象
    /// </summary>
    public class OutboundReadParam
    {
        private string _filedir;
        private string _filewrritename;
        private string _filereadname;

        public OutboundReadParam(string filedir)
            : this(filedir, "w", "r")
        {
        }

        public OutboundReadParam(string filedir, string writefileheader, string readfileheader)
        {
            FileDir = filedir;
            WriteFileHeader = writefileheader;
            ReadFileHeader = readfileheader;
        }

        public DateTime OnlineTime { get; set; }

        /// <summary>
        /// 同步间隔，用于缩小中间表检索的数据量
        /// </summary>
        public int IntervalDay { get; set; }

        private DateTime? _SyncTime;

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncTime
        {
            get
            {
                if (!_SyncTime.HasValue)
                {
                    DateTime tmpTime = DateTime.Now.AddDays(-IntervalDay);
                    _SyncTime = OnlineTime > tmpTime ? OnlineTime : tmpTime;
                }
                return _SyncTime.Value;
            }
        }

        /// <summary>
        /// 编号类型
        /// </summary>
        public Enums.SyncNoType NoType { get; set; }

        /// <summary>
        /// 余数
        /// </summary>
        public int Remaider { get; set; }

        /// <summary>
        /// 正常按照箱号取模的值
        /// </summary>
        public int NormalModValue { get; set; }

        /// <summary>
        /// 按照批次取模的值
        /// </summary>
        public int BatchModValue { get; set; }

        /// <summary>
        /// 写文件头标记
        /// </summary>
        public string WriteFileHeader
        {
            get;
            private set;
        }

        /// <summary>
        /// 读文件头标记
        /// </summary>
        public string ReadFileHeader
        {
            get;
            private set;
        }

      

        /// <summary>
        /// 文件路径名称
        /// </summary>
        public string FileDir
        {
            get
            {
                if (!_filedir.EndsWith(@"\"))
                {
                    _filedir += @"\";
                }
                return _filedir;
            }
            private set
            {
                _filedir = value.Trim();
            }
        }

        /// <summary>
        /// 多线程拆分条件
        /// </summary>
        public string SplitCondition
        {
            get;
            set;
        }

        /// <summary>
        /// 写文件时的文件全名
        /// </summary>
        public string FileWrittenName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_filewrritename))
                {
                    _filewrritename = String.Format("{0}{1}{2}.xml",
                    FileDir,
                    WriteFileHeader,
                    DateTime.Now.ToString("yyyyMMddHHmmssms"));
                }
                return _filewrritename;
            }
        }

        /// <summary>
        /// 可以读取的文件名
        /// </summary>
        public string FileReadName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_filereadname))
                {
                    _filereadname = String.Format("{0}{1}{2}.xml",
                   FileDir,
                   ReadFileHeader,
                   DateTime.Now.ToString("yyyyMMddHHmmssms"));
                }
                return _filereadname;
            }
        }

    }

}
