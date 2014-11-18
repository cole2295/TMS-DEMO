
using Vancl.TMS.Model.Common;
namespace Vancl.TMS.Model.BaseInfo
{
    /// <summary>
    /// 部门类
    /// </summary>
    public class ExpressCompanyModel : IDAndNameModel
    {
        /// <summary>
        /// 部门代码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>
        public string SimpleSpell { get; set; }
        /// <summary>
        /// 部门类型
        /// </summary>
        public Enums.CompanyFlag CompanyFlag { get; set; }
        /// <summary>
        /// 上级编码
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 部门全称
        /// </summary>
        public string CompanyAllName { get; set; }
        /// <summary>
        /// 是否有下级
        /// </summary>
        public bool HasChild { get; set; }
        /// <summary>
        /// 所属配送商编号
        /// </summary>
        public string DistributionCode { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 助记编号
        /// </summary>
        public string MnemonicCode { get; set; }
        /// <summary>
        /// 助记名称
        /// </summary>
        public string MnemonicName { get; set; }

        /// <summary>
        /// 站点所属分拣中心，多值，号隔开
        /// </summary>
        public string ParentSortingCenterId { get; set; }

        public string SiteNo { get; set; }

		public string Expresscompanycode { get; set; }
    }
}
