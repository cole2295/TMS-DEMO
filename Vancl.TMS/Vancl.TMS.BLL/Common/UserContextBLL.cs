using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.Model.BaseInfo.Carrier;
using System.ServiceModel;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Security;
using System.Data;
using Vancl.TMS.IDAL.AdministrationRegion;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.BLL.PmsService;

namespace Vancl.TMS.BLL.Common
{
    public class UserContextBLL : IUserContextBLL
    {
        IAdministrationDAL _adminDAL = ServiceFactory.GetService<IAdministrationDAL>("AdministrationDAL");
        #region IUserContextBLL 成员

        public bool UserLogon(string userName, string userPwd)
        {
            return true;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(userPwd))
                return false;
            PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient();
            try
            {
                return client.EmployeeLogin(userName, userPwd);
            }
            catch (FaultException e)
            {
                throw e;
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Close();
            }
        }

        public bool AddCarrier(CarrierModel carrier)
        {
            if (carrier == null)
                return false;
            PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient();
            try
            {
                PmsService.ExpressCompany ec = new PmsService.ExpressCompany();
                ec.Address = carrier.Address;
                ec.CompanyAllName = carrier.CarrierAllName;
                ec.CompanyName = carrier.CarrierName;
                ec.ContacterPhone = carrier.Phone;
                ec.Email = carrier.Email;
                ec.ExpressCompanyCode = carrier.CarrierNo;
                ec.MainContacter = carrier.Contacter;
                ec.DistributionCode = carrier.DistributionCode;

                if (client.AddExpressCompany(ec) <= 0)
                    throw new Exception("PMS接口异常.");
                return true;
            }
            catch (FaultException e)
            {
                throw e;
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Close();
            }
        }

        public UserModel GetUserInfo(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;
            PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient();
            try
            {
                UserModel u = new UserModel();
                var em = client.GetEmployee(userName);
                if (em == null)
                    return null;
                u.DeptID = em.StationID ?? 0;
                u.UserName = em.EmployeeName;
                u.ID = em.EmployeeID;
                u.UserCode = em.EmployeeCode;
                u.UserPwd = em.PassWord;
                u.DeptName = _adminDAL.GetCompanyNameByCompanyID(u.DeptID);
                
                u.DeptDataPermissions = GetDeptsByUserName(userName);
                u.DistributionCode = em.DistributionCode;
                return u;
            }
            catch (FaultException e)
            {
                throw e;
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Close();
            }
        }

        public UserModel GetUserInfo(int userID)
        {
            if (userID <= 0)
                return null;
            PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient();
            try
            {
                UserModel u = new UserModel();
                PmsService.Employee em = client.GetEmployeeByID(userID);
                if (em == null)
                    return null;
                u.DeptID = em.StationID ?? 0;
                u.UserName = em.EmployeeName;
                u.ID = em.EmployeeID;
                u.UserCode = em.EmployeeCode;
                u.UserPwd = em.PassWord;
                u.DeptName = _adminDAL.GetCompanyNameByCompanyID(u.DeptID);
                u.DeptDataPermissions = GetDeptsByUserName(em.EmployeeName);
                u.DistributionCode = em.DistributionCode;
                return u;
            }
            catch (FaultException e)
            {
                throw e;
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Close();
            }
        }

        public List<MenuModel> GetUserMenuByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;
            PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient();
            try
            {
                DataTable dt = client.GetEmployeeMenuData(userName, Consts.TMS_SYS_KEY);

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<MenuModel> menuList = new List<MenuModel>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MenuModel menu = new MenuModel();
                        menu.ID = i + 1;
                        menu.Name = dt.Rows[i]["MenuName"].ToString();
                        menu.Url = dt.Rows[i]["Url"].ToString();
                        menu.GrounpID = dt.Rows[i]["MenuGroup"] is DBNull ? -1 : int.Parse(dt.Rows[i]["MenuGroup"].ToString());
                        menu.MainMenuSeqNo = dt.Rows[i]["MainSortBy"] is DBNull ? -1 : int.Parse(dt.Rows[i]["MainSortBy"].ToString());
                        menu.SubMenuSeqNo = dt.Rows[i]["Sorting"] is DBNull ? -1 : int.Parse(dt.Rows[i]["Sorting"].ToString());
                        menu.MenuLevel = dt.Rows[i]["MenuLevel"] is DBNull ? -1 : int.Parse(dt.Rows[i]["MenuLevel"].ToString());

                        menuList.Add(menu);

                    }

                    List<MenuModel> MainMenuList = new List<MenuModel>();
                    List<MenuModel> SubMenuList = new List<MenuModel>();

                    for (int i = 0; i < menuList.Count; i++)
                    {
                        if (menuList[i].MenuLevel == 0)
                        {
                            MainMenuList.Add(menuList[i]);
                        }
                        else
                        {
                            SubMenuList.Add(menuList[i]);
                        }
                    }

                    menuList.Clear();

                    for (int i = 0; i < MainMenuList.Count; i++)
                    {
                        for (int j = 0; j < SubMenuList.Count; j++)
                        {
                            if (SubMenuList[j].GrounpID == MainMenuList[i].GrounpID)
                            {
                                SubMenuList[j].ParentID = MainMenuList[i].ID;
                            }
                        }
                    }

                    MainMenuList.AddRange(SubMenuList);

                    return MainMenuList;

                }
                else
                    return null;
            }
            catch (FaultException e)
            {
                throw e;
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Close();
            }
        }

        public List<string> GetUserRolesByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public List<int> GetDeptsByUserName(string userName)
        {
            //TODO:
            return null;
        }

        public bool IsSetOutsourcingRelation(int principalUserID, int agentUserID)
        {
            using (PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient())
            {
                return client.IsSetRelation(principalUserID, agentUserID);
            }
        }

        public List<OutSourcingModel> GetAgentRelations(int agentUserID)
        {
            AgentRelationModel[] relations = null;
            using (PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient())
            {
                relations = client.GetAgentRelation(agentUserID);
            }
            if (relations == null || relations.Length == 0)
            {
                return null;
            }
            List<OutSourcingModel> list = new List<OutSourcingModel>();
            foreach (var relation in relations)
            {
                list.Add(new OutSourcingModel() { DisplayName = relation.RelationName, PrincipalUserID = relation.PrincipalUserID });
            }
            return list;
        }
        #endregion
    }
}
