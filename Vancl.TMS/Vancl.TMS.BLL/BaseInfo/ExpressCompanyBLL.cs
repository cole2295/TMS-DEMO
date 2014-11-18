using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.Core.Security;
using System.Threading;
using Vancl.TMS.Model.Sorting.RangeDefined;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class ExpressCompanyBLL : BaseBLL, IExpressCompanyBLL
    {
        private IExpressCompanyDAL dal = ServiceFactory.GetService<IExpressCompanyDAL>();

        #region IExpressCompanyBLL 成员

        public IList<ExpressCompanyModel> GetChildExpressCompany(int parentId)
        {
            return dal.GetChildExpressCompany(parentId);
        }

        public ExpressCompanyModel Get(int expressCompanyId)
        {
            return dal.Get(expressCompanyId);
        }
        public ExpressCompanyModel GetModel(int expressCompanyId)
        {
            return dal.GetModel(expressCompanyId);
        }
        public IList<string> GetAllCompanyNames()
        {
            return dal.GetAllCompanyNames();
        }

        /// <summary>
        /// 根据ID列表检索对象
        /// </summary>
        /// <param name="listExpressCompanyID"></param>
        /// <returns></returns>
        public IList<ExpressCompanyModel> Search(List<int> listExpressCompanyID)
        {
            return dal.Search(listExpressCompanyID);
        }

        /// <summary>
        /// 通过站点名称查询站点ID
        /// </summary>
        /// <param name="expressCompanyName">站点名称</param>
        /// <returns></returns>
        public int GetExpressCompanyIDByName(string expressCompanyName)
        {
            return dal.GetExpressCompanyIDByName(expressCompanyName);
        }

        /// <summary>
        /// 根据配送商编码取得分拣中心列表不包含登录人所在分拣中心
        /// </summary>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetSortingCentersByDistributionCodeWithoutSelf()
        {
            return dal.GetSortingCentersByDistributionCodeWithoutSelf();
        }

        /// <summary>
        /// 根据配送商编码取得分拣中心列表
        /// </summary>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetSortingCentersByDistributionCode()
        {
            return dal.GetSortingCentersByDistributionCode();
        }

        /// <summary>
        /// 获得所有配送商
        /// </summary>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetAllDistributors()
        {
            return dal.GetAllDistributors();
        }

		public ExpressCompanyModel GetDistributor(string code)
		{
			return dal.GetDistributor(code);
		}

        /// <summary>
        /// 取得配送商关系配送商列表
        /// </summary>
        /// <param name="distributionCode">配送商Code</param>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetRelatedDistributor(string distributionCode)
        {
            if (String.IsNullOrWhiteSpace(distributionCode)) throw new ArgumentNullException("distributionCode is null or empty.");
            List<ExpressCompanyModel> listDistributor = new List<ExpressCompanyModel>();
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                var arrExpress = client.GetSubCooperationDistributions(distributionCode);
                if (arrExpress != null)
                {
                    foreach (var item in arrExpress)
                    {
                        listDistributor.Add(new ExpressCompanyModel()
                        {
                            ID = item.ExpressCompanyID.ToString(),
                            Name = item.CompanyName,
                            CompanyFlag = (Enums.CompanyFlag)item.CompanyFlag,
                            DistributionCode = item.DistributionCode
                        });
                    }
                }
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
                return listDistributor.OrderBy(p => p.Name).ToList();
            }
        }

	    public List<ExpressCompanyModel> GetDistributionCooprelation(string distributionCode)
	    {
		    return dal.GetDistributionCooprelation(distributionCode);
	    }



	    /// <summary>
        /// 取得有权限操作的城市
        /// </summary>
        /// <param name="addDefaultOption">是否添加默认"--请选择--"</param>
        /// <returns></returns>
        public List<IDAndNameModel> GetCitiesHasAuthority(bool addDefaultOption = false)
        {
            RoleCity[] cities = null;
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                //TODO:暂时用LMS的权限，后期需调整
                cities = client.GetCitiesOfUserHasAuthority(UserContext.CurrentUser.ID, (int)Enums.SpecificSystem.LMS, (int)Enums.UserAuthorityType.Operate);
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
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            list = (from l in list
                    orderby l.Name
                    select l).ToList();
            if (addDefaultOption)
            {
                list.Insert(0, new IDAndNameModel() { ID = "-1", Name = "--请选择--" });
            }
            return list;
        }

        /// <summary>
        /// 取得有权限操作的城市下的站点
        /// </summary>
        /// <param name="cityID">城市ID</param>
        /// <param name="addDefaultOption">是否添加默认"--请选择--"</param>
        /// <returns></returns>
        public List<IDAndNameModel> GetStationsHasAuthorityByCityID(string cityID, bool addDefaultOption = false)
        {
            Station[] stations = null;
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                //TODO:暂时用LMS的权限，后期需调整
                stations = client.GetStationsOfUserHasAuthorityByCityID(UserContext.CurrentUser.ID, (int)Enums.SpecificSystem.LMS, (int)Enums.UserAuthorityType.Operate, cityID);
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
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            list = (from l in list
                    orderby l.Name
                    select l).ToList();
            if (addDefaultOption)
            {
                list.Insert(0, new IDAndNameModel() { ID = "-1", Name = "--请选择--" });
            }
            return list;
        }

        /// <summary>
        /// 取得所有省
        /// </summary>
        /// <param name="addDefaultOption">是否添加默认"--请选择--"</param>
        /// <returns></returns>
        public List<IDAndNameModel> GetAllProvince(bool addDefaultOption = false)
        {
            Province[] provinces = null;
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                provinces = client.GetAllProvince();
            }
            if (provinces == null || provinces.Length == 0)
            {
                return null;
            }
            List<IDAndNameModel> list = new List<IDAndNameModel>();
            foreach (var province in provinces)
            {
                list.Add(new IDAndNameModel() { ID = province.ProvinceID, Name = province.ProvinceName });
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

        /// <summary>
        /// 根据省份取得城市
        /// </summary>
        /// <param name="provinceID">省ID</param>
        /// <param name="addDefaultOption">是否添加默认"--请选择--"</param>
        /// <returns></returns>
        public List<IDAndNameModel> GetCitiesByProviceID(string provinceID, bool addDefaultOption = false)
        {
            City[] cities = null;
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                cities = client.GetCityList(new City() { ProvinceID = provinceID });
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
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            list = (from l in list
                    orderby l.Name
                    select l).ToList();
            if (addDefaultOption)
            {
                list.Insert(0, new IDAndNameModel() { ID = "-1", Name = "--请选择--" });
            }
            return list;
        }
        #endregion

        #region IExpressCompanyBLL 成员

        /// <summary>
        /// 取得分拣中心下属站点
        /// </summary>
        /// <param name="SortCenterID">分拣中心ID</param>
        /// <returns></returns>
        public List<int> GetSortCenterStationList(int SortCenterID)
        {
            return dal.GetSortCenterStationList(SortCenterID);
        }

        /// <summary>
        /// 取得联系电话
        /// </summary>
        /// <param name="ExpresscompanyID"></param>
        /// <returns></returns>
        public string GetContacterPhone(int ExpresscompanyID)
        {
            return dal.GetContacterPhone(ExpresscompanyID);
        }

        #endregion

        /// <summary>
        /// 查询某个分拣中心对应的分拣范围
        /// </summary>
        /// <param name="searchaModel"></param>
        /// <returns></returns>
        public PagedList<ViewRangeDefinedSearchListModel> GetRangeDefineInfo(RangeDefinedSearchModel searchaModel)
        {
            return dal.GetRangeDefineInfoList(searchaModel);
        }


        public ViewRangeDefinedSearchListModel GetExistInfoByStationId(int companyFlag, int rangedExpressCompanyId)
        {
            return dal.GetExistInfoByStationId(companyFlag, rangedExpressCompanyId);
        }

        public bool IsExistsOfRangeDefined(RangeDefinedModel model)
        {
            return dal.IsExistsOfRangeDefined(model);
        }

        public int AddRangeDefined(RangeDefinedModel model)
        {
            return dal.AddRangeDefined(model);
        }

        public ResultModel DeleteRangeDefined(List<int> rangeDefinedIds)
        {
            if (rangeDefinedIds == null || rangeDefinedIds.Count == 0)
            {
                throw new CodeNotValidException();
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = dal.DeleteRangeDefined(rangeDefinedIds);
                //todo:增加日志
                scope.Complete();
            }
            if (i <= 0)
            {
                return ErrorResult("删除分拣范围定义失败！");
            }
            else
            {
                return InfoResult("删除分拣范围定义成功！");
            }
        }

        public IList<RangeDefinedPrintExportModel> GetRangeDefinedPrintExportModel(IList<string> rangeIdList, int chooseType)
        {
            if (rangeIdList == null) throw new ArgumentNullException();
            if (rangeIdList.Count == 0) throw new ArgumentException("未传入有批次数据。");
            return dal.GetRangeDefinedPrintExportModel(rangeIdList, chooseType);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public List<int> GetSortingCenterByStation(int station)
        {
            List<int> scenterList = new List<int>();
            var scenter = dal.GetSortingCenterByStation(station);

            if (scenter == null)
            {
                return scenterList;
            }

            scenterList.Add(scenter.BaseExpressCompanyId);

            //递归
            scenterList.AddRange(GetSortingCenterByStation(scenter.BaseExpressCompanyId));
            return scenterList;
        }
    }
}
