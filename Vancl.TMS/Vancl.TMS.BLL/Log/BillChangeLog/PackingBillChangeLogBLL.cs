using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
	/// <summary>
	/// 分拣装箱日志
	/// </summary>
	internal class PackingBillChangeLogBLL : BillChangeLogBLL
	{
		private const string DELETE_NOTE = "运单已取消装箱操作，箱号{0}";
		private const string ADD_NOTE = "运单已打包装箱，箱号{0}";

		protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
		{
			if (dynamicModel.ExtendedObj.IsAdd == null)
			{
				throw new ArgumentNullException("IsAdd is Null");
			}
			if (dynamicModel.ExtendedObj.BoxNo == null)
			{
				throw new ArgumentNullException("BoxNo is Null");
			}
			return string.Format(dynamicModel.ExtendedObj.IsAdd ? ADD_NOTE : DELETE_NOTE, dynamicModel.ExtendedObj.BoxNo);
		}
	}
}
