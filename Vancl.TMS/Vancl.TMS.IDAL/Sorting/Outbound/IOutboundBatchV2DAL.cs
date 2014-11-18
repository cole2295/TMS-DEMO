using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Outbound;

namespace Vancl.TMS.IDAL.Sorting.Outbound
{
	/// <summary>
	/// 出库批次数据接口
	/// </summary>
	public interface IOutboundBatchV2DAL
	{
		/// <summary>
		/// 新增出库批次对象
		/// </summary>
		/// <param name="batchModel"></param>
		/// <returns></returns>
		int Add(OutboundBatchEntityModel batchModel);

		/// <summary>
		/// 更新出库批次对应的数量
		/// </summary>
		/// <returns></returns>
		int Update(OutboundBatchEntityModel batchModel);
		/// <summary>
		/// 是否存在批次号
		/// </summary>
		/// <param name="batchNo">批次号</param>
		/// <returns></returns>
		bool Exists(String batchNo);

		/// <summary>
		/// 是否已经从TMS同步到LMS物流主库
		/// </summary>
		/// <param name="batchNo"></param>
		/// <returns></returns>
		bool IsTmsSync2Lms(String batchNo);

		/// <summary>
		/// 取得TMS出库批次记录需要同步到LMS物流主库的出库批次实体对象
		/// </summary>
		/// <param name="batchNo"></param>
		/// <returns></returns>
		OutboundBatchEntityModel GetOutboundBatchEntityModel4TmsSync2Lms(String batchNo);

		/// <summary>
		/// 更新同步标识[TMS到LMS]
		/// </summary>
		/// <param name="outboundKey">出库批次Key</param>
		/// <returns></returns>
		int UpdateSyncedStatus4Tms2Lms(String outboundBatchKey);
	}
}
