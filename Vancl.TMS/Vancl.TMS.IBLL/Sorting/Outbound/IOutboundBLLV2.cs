using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Outbound.SMS;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.Sorting.Outbound
{
	/// <summary>
	/// 出库业务接口
	/// </summary>
	public interface IOutboundBLLV2 : ISortCenterBLL
	{
		/// <summary>
		/// 取得出库前置条件
		/// </summary>
		/// <param name="DistributionCode"></param>
		/// <returns></returns>
		OutboundPreConditionModel GetPreCondition(String DistributionCode);

		/// <summary>
		/// 逐单出库
		/// </summary>
		/// <param name="argument">逐单出库对象</param>
		/// <returns></returns>
		ViewOutboundSimpleModel SimpleOutbound(OutboundSimpleArgModel argument,bool isBatchOutbound);

		/// <summary>
		/// 查询需要出库的运单信息
		/// </summary>
		/// <param name="argument">查询出库条件对象</param>
		/// <returns></returns>
		ViewOutboundSearchListModel GetNeededOutboundInfo(OutboundSearchArgModel argument);

		/// <summary>
		/// 批量出库
		/// </summary>
		/// <param name="argument">批量出库参数对象</param>
		/// <returns></returns>
		ViewOutboundBatchModel BatchOutbound(OutboundBatchArgModel argument);

		/// <summary>
		/// 查询出库 
		/// </summary>
		/// <param name="argument">查询出库参数对象</param>
		/// <returns></returns>
		ViewOutboundBatchModel SearchOutbound(OutboundSearchArgModel argument);

		/// <summary>
		/// 按箱出库查询
		/// </summary>
		/// <param name="argument">查询参数</param>
		/// <returns></returns>
		List<ViewOutBoundByBoxModel> SearchOutBoundByBox(OutboundSearchModel argument);

		/// <summary>
		/// 分拣按箱出库
		/// </summary>
		/// <param name="argument">分拣按箱出库参数对象</param>
		/// <returns></returns>
		ViewOutboundPackingModel PackingOutbound(OutboundPackingArgModel argument);

		/// <summary>
		/// 出库打印
		/// </summary>
		/// <param name="toStation"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <param name="expressID"></param>
		/// <param name="batchNo"></param>
		/// <param name="waybillNo"></param>
		/// <param name="companyFlag"></param>
		/// <returns></returns>
		IList<OutboundPrintModel> SearchOutboundPrint(int toStation, DateTime beginTime, DateTime endTime
											, int expressID, string batchNo, string waybillNo);

		/// <summary>
		/// 出库打印V2
		/// </summary>
		/// <param name="searchModel"></param>
		/// <returns></returns>
		PagedList<OutboundPrintModelV2> SearchOutboundPrintV2(OutboundPrintSearchModel searchModel);

		/// <summary>
		/// 获取出库打印交接单V2
		/// </summary>
		/// <param name="searchModel"></param>
		/// <returns></returns>
		IList<OutboundPrintModelV2> GetOutboundPrintReceipt(OutboundPrintSearchModel searchModel);

		/// <summary>
		/// 获得出库已打印的交接单（批次号）
		/// </summary>
		/// <param name="batchNoList"></param>
		/// <returns></returns>
		IList<OutboundPrintModelV2> GetOutboundPrintReceiptByBatchNos(IList<string> batchNoList);

		/// <summary>
		/// 创建并更新批次号
		/// </summary>
		/// <param name="searchModel"></param>
		/// <returns></returns>
		ResultModel CreateAndUpdateBatchNo(OutboundPrintSearchModel searchModel);
		/// <summary>
		/// 获取出库打印导出数据
		/// </summary>
		/// <param name="batchNoList"></param>
		/// <returns></returns>
		IList<OutboundPrintExportModel> GetOutboundPrintExportModel(IList<string> batchNoList);

		/// <summary>
		/// 获取出库打印导出数据V2
		/// </summary>
		/// <param name="batchNoList"></param>
		/// <returns></returns>
		IList<OutboundPrintExportDetailsModelV2> GetOutboundPrintExportModelV2(IList<string> batchNoList);

		/// <summary>
		/// 获取出库打印导出数据V2
		/// </summary>
		/// <param name="searchModel"></param>
		/// <returns></returns>
		IList<OutboundPrintExportDetailsModelV2> GetOutboundPrintExportModelV2(OutboundPrintSearchModel searchModel);
		/// <summary>
		/// 获取出库打印交接单统计
		/// </summary>
		/// <param name="batchNoList"></param>
		/// <returns></returns>
		IList<OutboundOrderCountModel> GetOrderCount(IList<string> batchNoList);

		/// <summary>
		/// 获取批次打印明细
		/// </summary>
		/// <param name="batchNo"></param>
		/// <returns></returns>
		IList<PrintBatchDetailModel> GetPrintBatchDetail(string batchNo);

		/// <summary>
		/// 取得出库短信配置
		/// </summary>
		/// <returns></returns>
		OutboundSMSConfigModel GetSMSConfig();

		/// <summary>
		/// 出库打印批量发送邮件
		/// </summary>
		/// <param name="expressCompanyId"></param>
		/// <param name="batchNoList"></param>
		/// <param name="emailList"></param>
		/// <returns></returns>
		ResultModel OutBoundSendEmail(int expressCompanyId, IEnumerable<string> batchNoList, IEnumerable<string> emailList);

		/// <summary>
		/// 出库打印批量发送邮件V2
		/// </summary>
		/// <param name="expressCompanyId"></param>
		/// <param name="batchNoList"></param>
		/// <param name="emailList"></param>
		/// <param name="selTimeType"></param>
		/// <param name="searchArg"></param>
		/// <returns></returns>
		ResultModel OutBoundSendEmailV2(int expressCompanyId, IEnumerable<string> batchNoList,
		                                IEnumerable<string> emailList, string selTimeType, string searchArg);
		/// <summary>
		/// 查询返货目的地
		/// </summary>
		/// <returns></returns>
		string GetReturnTo(string formCode, int arrivalID);

		/// <summary>
		/// 根据箱号获取订单明细
		/// </summary>
		/// <param name="boxNo"></param>
		/// <returns></returns>
		ViewOutBoundBoxDetailModel GetBoxBillsByBoxNo(string boxNo);

		/// <summary>
		/// 根据箱号获取包装箱信息
		/// </summary>
		/// <param name="boxNo"></param>
		/// <returns></returns>
		ViewOutBoundByBoxModel GetBoxInfoByBoxNo(string boxNo);

		/// <summary>
		/// 按箱出库(单箱和多箱) 
		/// </summary>
		/// <param name="argument">按箱出库参数对象</param>
		/// <returns></returns>
		List<ViewOutboundBatchModel> OutboundByBox(OutboundByBoxArgModel argument);

		/// <summary>
		/// 按箱出库(单箱)
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		ViewOutboundBatchModel BoxOutbound(OutboundByBoxArgModel argument);

		/// <summary>
		/// 设置箱表出库状态
		/// </summary>
		/// <param name="boxNo">箱号</param>
		/// <param name="isOutBounded">是否已出库</param>
		/// <returns></returns>
		int SetBoxOutBoundStatus(string boxNo, bool isOutBounded);


		/// <summary>
		/// 统计当日出库总数量
		/// </summary>
		/// <param name="departureId"></param>
		/// <returns></returns>
		int GetCurOutBoundCount(int departureId);

		/// <summary>
		/// 统计当日出库到当前目的地的数量
		/// </summary>
		/// <param name="departureId"></param>
		/// <param name="arrivalid"></param>
		/// <returns></returns>
		int GetCurDisOutBoundCount(int departureId, int arrivalid);
		/// <summary>
		/// 统计出库到当前目的地的数量和批次号（未截单）
		/// </summary>
		/// <returns></returns>
		ViewGetCountAndBatchNo GetCountAndBatchNo(int departureId, int arrivalid);


	}
}
