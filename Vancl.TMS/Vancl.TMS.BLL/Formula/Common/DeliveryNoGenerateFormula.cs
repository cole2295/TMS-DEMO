using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.IDAL.Formula.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Formula.Common
{
    public class DeliveryNoGenerateFormula : IFormula<string, SerialNumberModel>
    {
        ISerialNumberGenerateFormulaDAL formulaDal = ServiceFactory.GetService<ISerialNumberGenerateFormulaDAL>();

        #region IFormula<string,SerialNumberModel> 成员

        public string Execute(SerialNumberModel context)
        {
            Vancl.TMS.Model.Common.Enums.SerialNumberType type = Vancl.TMS.Model.Common.Enums.SerialNumberType.DeliveryNo;
            string str = EnumHelper.GetDescription(type);
            string d = DateTime.Now.ToString("yyyyMMdd");
            string scode = formulaDal.GetUserDefinedSeqNo(Enums.SerialNumberType.DeliveryNo).ToString();
            string sequence = Vancl.TMS.Util.StringUtil.GetSequeceString(scode, 4, "0");

            return string.Format(str, d, sequence);
        }

        #endregion
    }
}
