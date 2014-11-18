using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public abstract class BillReturnBoxPrintStrategy:Tms2LmsStrategy
    {
        #region 私有属性

        private Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL _wl_WaybillReturnBoxInfo_sql = null;
        private Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL _wl_WaybillReturnBoxInfo_oracle = null;
        private Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL _sc_BillReturnBoxInfo = null;

        #endregion

        #region 数据层
        /// <summary>
        /// TMS退货分拣称重箱号数据层
        /// </summary>
        public Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL SC_BillReturnBoxInfo
        {
            get
            {
                if (_sc_BillReturnBoxInfo == null)
                {
                    _sc_BillReturnBoxInfo = ServiceFactory.GetService<Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL>();
                }
                return _sc_BillReturnBoxInfo;
            }
        }

        /// <summary>
        /// LMS物流主库WaybillReturnBoxInfo【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL WL_WaybillReturnBoxInfo_OracleDAL
        {
            get
            {
                if (_wl_WaybillReturnBoxInfo_oracle == null)
                {
                    _wl_WaybillReturnBoxInfo_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL>("LMSWayBillReturnBoxDAL_Oracle");
                }
                return _wl_WaybillReturnBoxInfo_oracle;
            }
        }
        /// <summary>
        /// LMS物流主库WaybillBoxInfo【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL WL_WaybillReturnBoxInfo_SQLDAL
        {
            get
            {
                if (_wl_WaybillReturnBoxInfo_sql == null)
                {
                    _wl_WaybillReturnBoxInfo_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL>("LMSWayBillReturnBoxDAL_SQL");
                }
                return _wl_WaybillReturnBoxInfo_sql;
            }
        }
        #endregion

        #region 构建实体对象
        /// <summary>
        /// 创建LMS物流主库退货分拣称重箱号实体对象
        /// </summary>
        /// <param name="waybillmodel"></param>
        /// <param name="scBillReturnDetailInfoModel"></param>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        protected virtual Vancl.TMS.Model.LMS.WaybillReturnBoxInfoEntityModel CreateLMSWaybillReturnBoxEntityModel(Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel scBillReturnBoxInfoModel)
        {
            return new Model.LMS.WaybillReturnBoxInfoEntityModel()
            {
                CreateBy = scBillReturnBoxInfoModel.CreateBy,
                IsPrintBackForm = scBillReturnBoxInfoModel.IsPrintBackForm,
                IsPrintBackPacking = scBillReturnBoxInfoModel.IsPrintBackPacking,
                ReturnMerchant = scBillReturnBoxInfoModel.ReturnMerchant,
                CreateTime = scBillReturnBoxInfoModel.CreateTime,
                BoxNo = scBillReturnBoxInfoModel.BoxNo,
                CreateDep = scBillReturnBoxInfoModel.CreateDept,
                ReturnTo = scBillReturnBoxInfoModel.ReturnTo,
                Weight = scBillReturnBoxInfoModel.Weight
            };
        }
        #endregion

    }
}
