using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.Model.Transport.Plan
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：运输计划
    * 说明：记录两点之间所有路径的映射Model对象
    * 作者：任 钰
    * 创建日期：2012-02-14 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    /// <summary>
    /// 两点之间路径映射对象
    /// </summary>
    [Serializable]
    public class PointPathModel
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 点之间所有路径集合
        /// 预留
        /// </summary>
        private List<List<LinePlanModel>> AllPath
        {
            get;
            set;
        }

        /// <summary>
        /// 中转分拣中心
        /// </summary>
        public List<ExpressCompanyModel> TransferStation
        {
            get;
            set;
        }
        
    }
}
