using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Formula.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Formula.Common
{
    /// <summary>
    /// 表KeyCode产生算法
    /// </summary>
    public class KeyCodeGenerateFormula : IFormula<String, KeyCodeContextModel>
    {
        /// <summary>
        /// KeyCode格式[DDD-XXX-000000000000001]
        /// [DD] --数据库编码
        /// [XXX] --表列编码
        /// [000000000000001] --14位流水码
        /// </summary>
        private static readonly String KeyCodeFormatter = @"{0}{1}{2}";

        /// <summary>
        /// 表编码长度
        /// </summary>
        private static readonly int TableCodeLength = 3;

        /// <summary>
        /// 数据库编码长度
        /// </summary>
        private static readonly int DBCodeLength = 3;

        /// <summary>
        /// 序列号编码长度
        /// </summary>
        private static readonly int SerialNumberLength = 14;

        /// <summary>
        /// 流水码数据层
        /// </summary>
        ISerialNumberGenerateFormulaDAL formulaDal = ServiceFactory.GetService<ISerialNumberGenerateFormulaDAL>();

        #region IFormula<string,KeyCodeContextModel> 成员


        /// <summary>
        /// 取得表的当前序列流水号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private String GetSerialNumber(String SequenceName)
        {
            String SerialNumber = formulaDal.GetNextNumber(SequenceName);
            return SerialNumber.PadLeft(SerialNumberLength, '0');
        }

        public string Execute(KeyCodeContextModel context)
        {
            if (context == null) throw new ArgumentNullException("context is null");
            //TODO:拆库后添加check
            //if (String.IsNullOrWhiteSpace(context.DBCode)) throw new ArgumentNullException("context.DBCode is null");
            //if (context.DBCode.Length != 3) throw new ArgumentNullException("context.DBCode's length <> 3");
            if (String.IsNullOrWhiteSpace(context.TableCode)) throw new ArgumentNullException("context.TableCode is null");
            if (context.TableCode.Length != TableCodeLength) throw new ArgumentNullException(String.Format("context.TableCode's length <> {0}", TableCodeLength));
            if (String.IsNullOrWhiteSpace(context.SequenceName)) throw new ArgumentNullException("context.SequenceName is null");

            return String.Format(KeyCodeFormatter
                , String.IsNullOrWhiteSpace(context.DBCode) ? "ST0" : context.DBCode
                , context.TableCode
                , GetSerialNumber(context.SequenceName));

        }

        #endregion
    }

}
