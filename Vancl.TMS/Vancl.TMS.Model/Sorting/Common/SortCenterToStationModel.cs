using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣目的地对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class SortCenterToStationModel
    {
        /// <summary>
        /// 目的地ExpressCompanyID
        /// </summary>
        [DataMember]
        public int ExpressCompanyID { get; set; }

        /// <summary>
        /// 站点类型
        /// </summary>
        [DataMember]
        public Enums.CompanyFlag CompanyFlag { get; set; }

        /// <summary>
        /// 分拣类型
        /// </summary>
        public Enums.SortCenterOperateType SortingType
        {
            get
            {
                if (CompanyFlag == Enums.CompanyFlag.SortingCenter)
                {
                    return Enums.SortCenterOperateType.SecondSorting;
                }
                if (CompanyFlag == Enums.CompanyFlag.DistributionStation)
                {
                    return Enums.SortCenterOperateType.SimpleSorting;
                }
                if (CompanyFlag == Enums.CompanyFlag.Distributor)
                {
                    return Enums.SortCenterOperateType.DistributionSorting;
                }
                throw new Exception("严重错误:分拣类型问题");
            }
        }

        /// <summary>
        /// 配送商DistributionCode
        /// </summary>
        [DataMember]
        public string DistributionCode { get; set; }

        /// <summary>
        /// 助记名称
        /// </summary>
        [DataMember]
        public String MnemonicName { get; set; }

		/// <summary>
		/// 公司名称
		/// </summary>
		[DataMember]
		public string CompanyName { get; set; }
    }
}
