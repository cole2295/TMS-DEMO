using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Except
{
    public class SyncFileException: Exception
    {
        /// <summary>
        /// 异常文件完整路径
        /// </summary>
        public string AbnormalFileFullName
        {
            get;
            private set;
        }

        public SyncFileException(string abnormalmsg)
            : base(abnormalmsg)
        {

        }


        public SyncFileException(string fullfilename, string abnormalmsg)
            : base(abnormalmsg)
        {
            AbnormalFileFullName = fullfilename;
        }

        public SyncFileException(string fullfilename, string abnormalmsg, Exception ex)
            : base(abnormalmsg, ex)
        {
            AbnormalFileFullName = fullfilename;
        }

    }
}
