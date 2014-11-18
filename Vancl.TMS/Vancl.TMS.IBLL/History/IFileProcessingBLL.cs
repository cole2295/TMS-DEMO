using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.IBLL.History
{
    public interface IFileProcessingBLL
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="deadLine">截至时间</param>
        /// <returns></returns>
        int DeleteFile(string dir, DateTime deadLine, string searchPattern = "");
        /// <summary>
        /// 获取FTP文件
        /// </summary>
        /// <param name="sysname">pms 配置系统名称</param>
        /// <param name="menuname">pms 配置目录名称</param>
        /// <returns></returns>
        string GetHelpFile(string sysname, string menuname);
    }
}
