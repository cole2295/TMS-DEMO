using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.IDAL.Formula.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Formula.Outbound
{
    /// <summary>
    /// 出库批次号产生算法
    /// </summary>
    public class OutboundBatchNoGenerateFormula : IFormula<string, SerialNumberModel>
    {
        ISerialNumberGenerateFormulaDAL formulaDal = ServiceFactory.GetService<ISerialNumberGenerateFormulaDAL>();

        #region IFormula<string,SerialNumberModel> 成员

        public string Execute(SerialNumberModel context)
        {
            if (context == null) throw new ArgumentNullException("SerialNumberModel is null.");
            if (String.IsNullOrWhiteSpace(context.FillerCharacter)) throw new ArgumentNullException("context.FillerCharacter is null or empty.");
            if (context.NumberLength <= 0) throw new ArgumentException("must > 0", "context.NumberLength ", null);
            Enums.SerialNumberType type = Enums.SerialNumberType.OutBatchNo;
            string str = EnumHelper.GetDescription(type);
            string d = DateTime.Now.ToString("yyyyMMdd");
            int nseqNo = formulaDal.GetUserDefinedSeqNo(type);
            nseqNo += 100000;           //TMS新批次号默认+10W，同原有LMS版本区分开来
            string scode = nseqNo.ToString();
            string sequence = scode.PadLeft(context.NumberLength, context.FillerCharacter[0]);
            return string.Format(str, d, sequence);
        }

        #endregion
    }

}
