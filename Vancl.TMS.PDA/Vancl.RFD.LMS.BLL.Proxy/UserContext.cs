using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.PDA.Core.Model;
using Vancl.TMS.BLL.Proxy.PMSService;
using System.Web;
using System.Data;
using Vancl.TMS.PDA.Core;

namespace Vancl.TMS.BLL.Proxy
{
    public class UserContext
    {

        public static PDAUserModel Login(string userName, string Pwd)
        {
            using (PMSService.PermissionOpenServiceClient client = new Vancl.TMS.BLL.Proxy.PMSService.PermissionOpenServiceClient())
            {
                PDAUserModel u = null;
                try
                {
                    if (client.EmployeeLogin(userName, Pwd))
                    {
                        Vancl.TMS.BLL.Proxy.PMSService.Employee employee = client.GetEmployee(userName);
                        if (employee != null)
                        {
                            u = new PDAUserModel();
                            u.UserID = employee.EmployeeID;
                            u.UserLoginTime = DateTime.Now;
                            u.UserName = employee.EmployeeCode;
                            u.UserDisplayName = employee.EmployeeName;
                            u.UserDeptID = employee.StationID;
                            u.UserDistributeCode = employee.DistributionCode;

                            SetUserFromCookies(u);
                        }
                    }
                }
                catch
                {
                    throw;
                }

                return u;
            }
        }

        private static void SetUserFromCookies(PDAUserModel user)
        {
            CookieUtil.AddCookie("Vancl.Vancl.TMS.PDA.UserID", user.UserID.ToString(), 18);
            CookieUtil.AddCookie("Vancl.Vancl.TMS.PDA.UserName", user.UserName, 18);
            CookieUtil.AddCookie("Vancl.Vancl.TMS.PDA.UserDisplayName", user.UserDisplayName, 18);
            CookieUtil.AddCookie("Vancl.Vancl.TMS.PDA.UserDeptID", user.UserDeptID.ToString(), 18);
            CookieUtil.AddCookie("Vancl.Vancl.TMS.PDA.UserDistributionCode", user.UserDistributeCode, 18);
        }

        public static PDAUserModel GetCurrentUserFromCookies()
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PDAUserModel user = null;
            if (CookieUtil.ExistCookie("Vancl.Vancl.TMS.PDA.UserID"))
            {
                user = new PDAUserModel();
                user.UserID = CookieUtil.ExistCookie("Vancl.Vancl.TMS.PDA.UserID") ? int.Parse(CookieUtil.GetCookie("Vancl.Vancl.TMS.PDA.UserID")) : -1;
                user.UserDeptID = CookieUtil.ExistCookie("Vancl.Vancl.TMS.PDA.UserDeptID") ? int.Parse(CookieUtil.GetCookie("Vancl.Vancl.TMS.PDA.UserDeptID")) : -1;
                user.UserDisplayName = CookieUtil.ExistCookie("Vancl.Vancl.TMS.PDA.UserDisplayName") ? CookieUtil.GetCookie("Vancl.Vancl.TMS.PDA.UserDisplayName") : string.Empty;
                user.UserDistributeCode = CookieUtil.ExistCookie("Vancl.Vancl.TMS.PDA.UserDistributionCode") ? CookieUtil.GetCookie("Vancl.Vancl.TMS.PDA.UserDistributionCode") : string.Empty;
                user.UserName = CookieUtil.ExistCookie("Vancl.Vancl.TMS.PDA.UserName") ? CookieUtil.GetCookie("Vancl.Vancl.TMS.PDA.UserName") : string.Empty;
            }

            return user;
        }

        public static void Logout()
        {
            CookieUtil.ClearCookie("Vancl.Vancl.TMS.PDA.UserID");
            CookieUtil.ClearCookie("Vancl.Vancl.TMS.PDA.UserDeptID");
            CookieUtil.ClearCookie("Vancl.Vancl.TMS.PDA.UserDisplayName");
            CookieUtil.ClearCookie("Vancl.Vancl.TMS.PDA.UserDistributionCode");
            CookieUtil.ClearCookie("Vancl.Vancl.TMS.PDA.UserName");
        }

        public static List<IDAndNameModel> GetCityBySortingCenter(int userID, bool addDefaultOption = false)
        {
            RoleCity[] cities = null;
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                //TODO:暂时用LMS的权限，后期需调整
                cities = client.GetCitiesOfUserHasAuthority(userID, 1, 1);
            }
            if (cities == null || cities.Length == 0)
            {
                return null;
            }
            List<IDAndNameModel> list = new List<IDAndNameModel>();
            foreach (var city in cities)
            {
                list.Add(new IDAndNameModel() { ID = city.CityID, Name = city.CityName });
            }

            list = (from l in list
                    orderby l.Name
                    select l).ToList();
            if (addDefaultOption)
            {
                list.Insert(0, new IDAndNameModel() { ID = "-1", Name = "--请选择--" });
            }
            return list;
        }

        public static List<IDAndNameModel> GetStationsByCityId(string cityID, int userID, bool addDefaultOption = false)
        {
            Station[] stations = null;
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                //TODO:暂时用LMS的权限，后期需调整
                stations = client.GetStationsOfUserHasAuthorityByCityID(userID, 1, 1, cityID);
            }
            if (stations == null || stations.Length == 0)
            {
                return null;
            }
            List<IDAndNameModel> list = new List<IDAndNameModel>();
            foreach (var station in stations)
            {
                list.Add(new IDAndNameModel() { ID = station.StationID, Name = station.StationName });
            }
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            list = (from l in list
                    orderby l.Name
                    select l).ToList();
            if (addDefaultOption)
            {
                list.Insert(0, new IDAndNameModel() { ID = "-1", Name = "--请选择--" });
            }
            return list;
        }

        public static List<ExpressCompany> GetSortCenters(string distributionCode, int deptID)
        {
            if (string.IsNullOrWhiteSpace(distributionCode) || deptID == -1) throw new ArgumentNullException();
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                ExpressCompany[] companys = client.GetSortingCentersByDistributionCodeWithoutSelf(distributionCode, deptID);
                return companys.ToList();
            }
        }

        public static List<ExpressCompany> GetDistributions(string distributionCode)
        {
            if (String.IsNullOrWhiteSpace(distributionCode)) throw new ArgumentNullException("distributionCode is null or empty.");
            List<ExpressCompany> listDistributor = new List<ExpressCompany>();
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                var arrExpress = client.GetSubCooperationDistributions(distributionCode);
                if (arrExpress != null)
                {
                    foreach (var item in arrExpress)
                    {
                        listDistributor.Add(new ExpressCompany()
                        {
                            ExpressCompanyID = item.ExpressCompanyID,
                            CompanyName = item.CompanyName,
                            CompanyFlag = item.CompanyFlag,
                            DistributionCode = item.DistributionCode
                        });
                    }
                }
                //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
                return listDistributor.OrderBy(p => p.CompanyName).ToList();
            }
        }

    }
}
