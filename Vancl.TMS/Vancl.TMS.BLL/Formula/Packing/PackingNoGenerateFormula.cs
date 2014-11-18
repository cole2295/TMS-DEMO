using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.IDAL.Formula.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Formula.Packing
{
    public class PackingNoGenerateFormula : IFormula<String, InboundPackingNoContextModel>
    {
        ISerialNumberGenerateFormulaDAL formulaDal = ServiceFactory.GetService<ISerialNumberGenerateFormulaDAL>();

        #region IFormula<string,InboundPackingNoContextModel> 成员

        public string Execute(InboundPackingNoContextModel context)
        {
            if (context == null) throw new ArgumentNullException("context is null.");
            if (String.IsNullOrWhiteSpace(context.FillerCharacter)) throw new ArgumentNullException("context.FillerCharacter is null or empty.");
            if (context.NumberLength <= 0) throw new ArgumentException("must > 0", "context.NumberLength ", null);
            if (context.SortingCenterID <= 0) throw new ArgumentException("must > 0", "context.SortingCenterID", null);

            Enums.SerialNumberType type = Enums.SerialNumberType.SortingBoxNo;
            String str = EnumHelper.GetDescription(type);
            String sortcenter = context.SortingCenterID.ToString();
            String d = DateTime.Now.ToString("yyyyMMdd");
            int nseqNo = formulaDal.GetUserDefinedSeqNo(type);
            String scode = nseqNo.ToString();
            String sequence = scode.PadLeft(context.NumberLength, context.FillerCharacter[0]);

            return String.Format(str, sortcenter, d, sequence);
        }

        #endregion
    }
}
