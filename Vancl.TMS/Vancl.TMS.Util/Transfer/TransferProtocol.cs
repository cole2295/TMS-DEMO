using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Vancl.TMS.Util.Transfer
{
    /*
     * (C)Copyright 2011-2012 TMS
     * 
     * 模块名称：通信协议
     * 说明：通信协议
     * 作者：任 钰
     * 创建日期：2012-01-16 11:34:00
     * 修改人：
     * 修改时间：
     * 修改记录：记录以便查阅  / 包头+类型+总长度
     */
    public class TransferProtocol
    {
        /// <summary>
        /// 包头身份标记字节长度
        /// </summary>
        public const int PackageIdentLength = 1;
        /// <summary>
        /// 包类型标记字节长度
        /// </summary>
        public const int PackageTypeLength = 1;
        /// <summary>
        /// 每个数据包具体数据的总长度
        /// </summary>
        public const int PackageTotalLength = 4;


        /// <summary>
        /// 当前数据包完整类型
        /// </summary>
        [Description("当前数据包完整类型")]
        public enum emPackageFullyType
        {
            /// <summary>
            /// 数据包包头完整
            /// </summary>
            [Description("数据包包头完整")]
            PackageHeadFully = 0,
            /// <summary>
            /// 数据包头不完整
            /// </summary>
            [Description("数据包包头不完整")]
            PackageHeadPartly = 1,
            /// <summary>
            /// 数据包数据完整
            /// </summary>
            [Description("数据包数据完整")]
            PackageDataFully = 2,
            /// <summary>
            /// 数据包数据不完整
            /// </summary>
            [Description("数据包数据不完整")]
            PackageDataPartly = 3
        }

        /// <summary>
        /// 包头长度
        /// </summary>
        public static int PackageHeadLength
        {
            get
            {
                return PackageIdentLength + PackageTypeLength + PackageTotalLength ;
            }
        }

    }
}
