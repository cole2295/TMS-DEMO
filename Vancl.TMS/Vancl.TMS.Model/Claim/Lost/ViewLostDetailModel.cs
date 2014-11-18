using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Claim.Lost
{
    public class ViewLostDetailModel : BaseModel
    {
        /// <summary>
        /// 箱号列表
        /// </summary>
        public List<ViewLostBoxModel> BoxList
        {
            get;
            set;
        }

        /// <summary>
        /// 预计丢失的单号列表
        /// </summary>
        public List<LostOrderDetailModel> PreLostOrderList { get; set; }
    }
}
