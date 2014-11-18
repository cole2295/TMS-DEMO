﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.CompilerServices;

namespace Vancl.TMS.Web.Areas.Sorting.Models
{
    public class SortingPackingResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public dynamic ExtendedObj { get; set; }
    }
}