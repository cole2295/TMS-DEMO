using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Formula.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Formula.Common
{
    public class CarrierNoGenerateFormula : IFormula<string, CarrierNoContextModel>
    {
        ISerialNumberGenerateFormulaDAL dal = ServiceFactory.GetService<ISerialNumberGenerateFormulaDAL>("SerialNumberGenerateFormulaDAL");

        #region IFormula<string,CarrierNoContextModel> 成员

        public string Execute(CarrierNoContextModel context)
        {
            if (context != null)
            {
                if (string.IsNullOrWhiteSpace(context.CarrierNo))
                {
                    string str = EnumHelper.GetDescription(Vancl.TMS.Model.Common.Enums.SerialNumberType.CarrierNo);
                    string sequence = dal.GetNextNumber("seq_tms_carrier_carrierNO");

                    for (int i = 0; i < context.NumberModel.NumberLength; i++)
                    {
                        sequence = context.NumberModel.FillerCharacter + sequence;
                        if (sequence.Length >= context.NumberModel.NumberLength)
                            break;
                    }

                    return string.Format(str, EnumHelper.GetDescription(context.CarrierCoverage), sequence);
                }
                else
                    return context.CarrierNo;
            }
            else
                return "";
        }

        #endregion
    }
}
