using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;

namespace Vancl.TMS.BLL
{
    public class AddFormula : IFormula<decimal, AddContext>
    {

        #region IFormula<decimal,AddContext> 成员

        public decimal Execute(AddContext context)
        {
            return 0;
        }

        #endregion
    }

    public class AddContext
    {
        public decimal First { get; set; }
        public decimal Secend { get; set; }
    }

    public class ResultContext
    {
        public decimal Result { get; set; }
    }
}
