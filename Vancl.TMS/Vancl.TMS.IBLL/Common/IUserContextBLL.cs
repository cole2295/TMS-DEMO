using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Common
{
    public interface IUserContextBLL
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        bool UserLogon(string userName, string userPwd);

        /// <summary>
        /// 新增承运商
        /// </summary>
        /// <param name="carrier"></param>
        /// <returns></returns>
        bool AddCarrier(CarrierModel carrier);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserModel GetUserInfo(string userName);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        UserModel GetUserInfo(int userID);

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<MenuModel> GetUserMenuByUserName(string userName);

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<string> GetUserRolesByUserName(string userName);

        /// <summary>
        /// 根据用户名获取用户有权查看的部门ID
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        List<int> GetDeptsByUserName(string userName);

        /// <summary>
        /// 是否设置了外包代理关系
        /// </summary>
        /// <param name="principalUserID">被代理人</param>
        /// <param name="agentUserID">代理人</param>
        /// <returns></returns>
        bool IsSetOutsourcingRelation(int principalUserID, int agentUserID);

        /// <summary>
        /// 取得外包代理列表
        /// </summary>
        /// <param name="agentUserID">代理人</param>
        /// <returns></returns>
        List<OutSourcingModel> GetAgentRelations(int agentUserID);
    }
}
