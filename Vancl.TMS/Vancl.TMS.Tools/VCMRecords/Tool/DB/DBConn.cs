using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Vancl.TMS.Tools.VCMRecords.Tool.DB
{
    public class DBConn
    {
        private static String _VCMRecordConnectionString;
        private static String _PMSConnectionString;

        /// <summary>
        /// VCM操作数据库连接字符串
        /// </summary>
        public static String VCMRecordsConnctionString
        {
            get
            {
                if (_VCMRecordConnectionString == null)
                {
                    _VCMRecordConnectionString = ConfigurationManager.ConnectionStrings["VCMRecordConnectionString"].ToString();
                    if (ConfigurationManager.AppSettings["VCMRecordConnectionString"].ToString() == "1")//如果是序列化过后的字符串
                    {

                    }
                }
                return _VCMRecordConnectionString;
            }
        }

        public static String PMSConnectionString
        {
            get
            {
                if (_PMSConnectionString == null)//如果是序列化过后的字符串
                {
                    _PMSConnectionString = ConfigurationManager.ConnectionStrings["PMSConnectionString"].ToString();
                    if (ConfigurationManager.AppSettings["PMSConnectionStringType"].ToString() == "1")
                    {

                    }
                }
                return _PMSConnectionString;
            }
        }

    }
}
