using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.DeliveryAbnormal;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Transport.Dispatch
{
    public interface IPreDispatchBLL
    {
        /// <summary>
        /// 返回异常预调度批次信息
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        BoxModel GetAbnormalPreDispatchByBID(Int64 bid);

        /// <summary>
        /// 预调度服务
        /// </summary>
        /// <param name="arguments"></param>
        [Obsolete("采用新的预调度方式，此方法作废")]
        void PreDispatch(PreDispatchJobArgModel arguments);

        /// <summary>
        /// 预调度服务 通用
        /// </summary>
        /// <param name="boxList"></param>
        /// <returns></returns>
        [Obsolete("采用新的预调度方式，此方法作废")]
        List<ResultModel> CommonPreDispatch(List<BoxModel> boxList);

        /// <summary>
        /// 预调度服务
        /// </summary>
        /// <param name="arguments"></param>
        void PreDispatchV1(PreDispatchJobArgModel arguments);

        /// <summary>
        /// 预调度服务 通用
        /// </summary>
        /// <param name="boxList"></param>
        /// <returns></returns>
        List<ResultModel> CommonPreDispatchV1(List<BoxModel> boxList);

        /// <summary>
        /// 进行预调度
        /// </summary>
        /// <param name="count">执行数据条数</param>
        /// <returns></returns>
        [Obsolete("采用新的预调度方式，此方法作废")]
        int PreDispatch(int count);

        /// <summary>
        /// 运输调度根据线路ID取得预调度信息
        /// </summary>
        /// <param name="LPID">线路计划ID</param>
        /// <returns></returns>
        List<ViewDispatchBoxModel> GetPreDispatchBoxList(int LPID);

        /// <summary>
        /// 更新城际批次为待预调度状态
        /// </summary>
        /// <param name="box">城际批次对象</param>
        /// <returns></returns>
        int UpdateBoxToWaitforDispatch(BoxModel box);

        /// <summary>
        /// 批量更新城际批次为待预调度状态
        /// </summary>
        /// <param name="listbox">城际批次对象</param>
        /// <returns></returns>
        int BatchUpdateUpdateBoxToWaitforDispatch(List<BoxModel> listbox);

        /// <summary>
        /// 查询预调度异常
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<PreDispatchAbnormalModel> GetPreDispatchAbnormalList(PreDispatchAbnormalSearchModel searchModel);

        List<ViewDispatchBoxModel> GetPreDispatchBoxListV1(int LPID);
    }
}
