using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 发货交接订单统计模型
    /// </summary>
    public class OutboundOrderCountModel
    {
        public string BatchNo { get; set; }
        public string CompanyName { get; set; }
        public int AllCount { get; set; }
        public int TurnCount { get; set; }
        public int VanclTurnCount { get; set; }
        public int VjiaTurnCount { get; set; }
        public int OtherTurnCount { get; set; }
        public int SecondSortingCount { get; set; }
        public int VanclSecondSortingCount { get; set; }
        public int VjiaSecondSortingCount { get; set; }
        public int OtherSecondSortingCount { get; set; }
        public int VanclAllCount { get; set; }
        public int VjiaAllCount { get; set; }
        public int OtherAllCount { get; set; }

        public decimal InsuredAmount { get; set; }

        public int VijiaNormalCount
        {
            get
            {
                return VjiaAllCount - VjiaTurnCount - VjiaSecondSortingCount;
            }
        }

        public int VanclNormalCount
        {
            get
            {
                return VanclAllCount - VanclTurnCount - VanclSecondSortingCount;
            }
        }
        
        public int OtherNormalCount
        {
            get
            {
                return OtherAllCount - OtherTurnCount - OtherSecondSortingCount;
            }
        }

        public int NormalCount
        {
            get
            {
                return AllCount - TurnCount - SecondSortingCount;
            }
        }
    }
}
