using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<int> Role { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 菜单URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 子菜单序号
        /// </summary>
        public int SubMenuSeqNo { get; set; }

        /// <summary>
        /// 父菜单序号
        /// </summary>
        public int MainMenuSeqNo { get; set; }

        /// <summary>
        /// 菜单级别
        /// </summary>
        public int MenuLevel { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        public int GrounpID { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public int ParentID { get; set; }
    }
}
