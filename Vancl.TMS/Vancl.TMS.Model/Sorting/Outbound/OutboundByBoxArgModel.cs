using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    public class OutboundByBoxArgModel : IOutboundArgModel
    {
        public List<string> CurrentFormCodes { get; set; }

        public string CurrentBoxNo { get; set; }

        public List<string> BoxNos { get; set; }

        #region IOutboundArgModel 成员

        public OutboundPreConditionModel PreCondition { get; set; }
        

        #endregion

        #region ISortCenterArgModel 成员

        public SortCenterToStationModel ToStation { get; set; }

        public SortCenterUserModel OpUser { get; set; }

        #endregion
    }
}
