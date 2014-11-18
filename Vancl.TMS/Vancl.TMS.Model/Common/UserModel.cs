using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Vancl.TMS.Model.Common
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// 标识(原EmployeeID)
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色ID(用户菜单权限控制)
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 所属部门(原ExpressCompanyID,用于数据权限控制)
        /// </summary>
        public int DeptID { get; set; }

        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [NonSerialized]
        public string UserPwd;// { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [NonSerialized]
        public DateTime? LoginTime;// { get; set; }

        /// <summary>
        /// 用户拥有数据权限的部门
        /// </summary>
        [NonSerialized]
        public List<int> DeptDataPermissions;// { get; set; }

        /// <summary>
        /// 配送商编码
        /// </summary>
        public string DistributionCode { get; set; }
    }
}
