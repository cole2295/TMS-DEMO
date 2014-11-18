using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Plan
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：运输计划
    * 说明：获取两点间所有路径的检索条件
    * 作者：任 钰
    * 创建日期：2012-02-14 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public class PointPathSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public virtual int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public virtual int ArrivalID { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public virtual Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 是否中转
        /// </summary>
        public virtual bool IsTransit { get; set; }




    }
}
