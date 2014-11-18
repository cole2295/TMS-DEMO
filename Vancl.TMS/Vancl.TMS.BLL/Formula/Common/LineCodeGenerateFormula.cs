using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Formula.Common
{
    public class LineCodeGenerateFormula : IFormula<string, LineCodeContextModel>
    {
        #region IFormula<string,LineCodeContextModelcs> 成员

        public string Execute(LineCodeContextModel context)
        {
            string str = EnumHelper.GetDescription(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode);
            if (context != null)
            {
                return string.Format(str, context.Header, context.Departure, context.Arrival, context.BusinessTypeString, context.TransportTypeString, context.CarrierID);
            }
            else
                return "";
        }

        #endregion
    }
}
