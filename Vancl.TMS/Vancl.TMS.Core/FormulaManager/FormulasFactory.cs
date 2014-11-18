using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Vancl.TMS.Model.Common;
using System.IO;
using Vancl.TMS.Util.IO;
using Vancl.TMS.Util;
using System.Collections;
using System.Reflection;
using Vancl.TMS.Core.ObjectFactory;

namespace Vancl.TMS.Core.FormulaManager
{
    public class FormulasFactory : ObjectFactory.ObjectFactory
    {
        /// <summary>
        /// 获取函数实体
        /// </summary>
        /// <typeparam name="T">函数返回值类型</typeparam>
        /// <param name="formulaName"></param>
        /// <returns></returns>
        public static T GetFormula<T>(string formulaName)
        {
            return (T)GetNormalObject(formulaName, ObjectCategory.Formula);
        }
    }
}
