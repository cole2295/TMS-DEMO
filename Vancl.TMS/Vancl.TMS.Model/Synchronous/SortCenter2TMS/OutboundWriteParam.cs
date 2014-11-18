using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Synchronous
{
    /// <summary>
    /// 出库同步服务数据文件To TMS订单主表参数对象
    /// </summary>
    public class OutboundWriteParam
    {
        private string _filedir;
        private string _abnormalfiledir;
        private string _backupfiledir;

        public OutboundWriteParam(string filedir)
            : this(filedir, "r*.xml", "BackUp", "Abnormal")
        {

        }

        public OutboundWriteParam(string filedir, string filesearchpattern, string backupDirName,string abnormalDirName)
        {
            FileDir = filedir;
            FileSearchPattern = filesearchpattern;
            BackUpFileDir = FileDir + backupDirName;
            AbnormalFileDir = FileDir + abnormalDirName;
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
        /// 异常文件夹
        /// </summary>
        public string AbnormalFileDir
        {
            get
            {
                if (!_abnormalfiledir.EndsWith(@"\"))
                {
                    _abnormalfiledir += @"\";
                }
                return _abnormalfiledir;
            }
            private set
            {
                _abnormalfiledir = value.Trim();
            }

        }

        /// <summary>
        /// 备份文件夹
        /// </summary>
        public string BackUpFileDir
        {
            get
            {
                if (!_backupfiledir.EndsWith(@"\"))
                {
                    _backupfiledir += @"\";
                }
                return _backupfiledir;
            }
            private set
            {
                _backupfiledir = value.Trim();
            }
        }


        /// <summary>
        /// 文件检索模式
        /// </summary>
        public string FileSearchPattern
        {
            get;
            private set;
        }

        /// <summary>
        /// 多线程拆分条件
        /// </summary>
        public string SplitCondition
        {
            get;
            set;
        }

    }
}
