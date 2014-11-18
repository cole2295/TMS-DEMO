using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.AdministrationRegion;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.AdministrationRegion;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.AdministrationRegion
{
    public class AdministrationDAL : BaseDAL, IAdministrationDAL
    {
        #region IAdministrationDAL 成员

        public IList<District> GetDistrict()
        {
            string sql = @"
                SELECT districtid, districtname, isdeleted, distributioncode 
                FROM district
                WHERE isdeleted = 0";

            return ExecuteSql_ByReaderReflect<District>(TMSReadOnlyConnection, sql);
        }

        public District GetDistrict(string districtID)
        {
            string sql = @"
                SELECT districtid, districtname, isdeleted, distributioncode 
                FROM district
                WHERE isdeleted = 0 and districtid=:DistrictID";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DistrictID", DbType = DbType.String, Value = districtID } 
            };

            var list = ExecuteSql_ByReaderReflect<District>(TMSReadOnlyConnection, sql, arguments);
            return list.Count > 0 ? list[0] : null;
        }

        public IList<Province> GetProvince()
        {
            string sql = @"
                SELECT provinceid, provincename, districtid, isdeleted, distributioncode
                FROM province
                WHERE isdeleted = 0";

            return ExecuteSql_ByReaderReflect<Province>(TMSReadOnlyConnection, sql);
        }

        public IList<Province> GetProvinceByDistrictID(string districtID)
        {
            string sql = @"
                SELECT provinceid, provincename, districtid, isdeleted, distributioncode
                FROM province
                WHERE isdeleted = 0 and districtid=:DistrictID";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DistrictID", DbType = DbType.String, Value = districtID } 
            };

            return ExecuteSql_ByReaderReflect<Province>(TMSReadOnlyConnection, sql, arguments);
        }

        public Province GetProvince(string provinceID)
        {
            string sql = @"
                SELECT provinceid, provincename, districtid, isdeleted, distributioncode
                FROM province
                WHERE isdeleted = 0 and provinceid=:ProvinceID";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "ProvinceID", DbType = DbType.String, Value = provinceID } 
            };

            var list = ExecuteSql_ByReaderReflect<Province>(TMSReadOnlyConnection, sql, arguments);
            return list.Count > 0 ? list[0] : null;
        }

        public IList<City> GetCity()
        {
            string sql = @"
                SELECT cityid, cityname, provinceid, expresscompanyid, isdeleted, distributioncode 
                FROM city
                WHERE isdeleted = 0";

            return ExecuteSql_ByReaderReflect<City>(TMSReadOnlyConnection, sql);
        }

        public IList<City> GetCityByProvinceID(string provinceID)
        {
            string sql = @"
                SELECT cityid, cityname, provinceid, expresscompanyid, isdeleted, distributioncode 
                FROM city
                WHERE isdeleted = 0 and provinceid=:ProvinceID";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "ProvinceID", DbType = DbType.String, Value = provinceID } 
            };

            return ExecuteSql_ByReaderReflect<City>(TMSReadOnlyConnection, sql, arguments);
        }

        public City GetCity(string cityID)
        {
            string sql = @"
                SELECT cityid, cityname, provinceid, expresscompanyid, isdeleted, distributioncode 
                FROM city
                WHERE isdeleted = 0 and cityid=:CityID";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "CityID", DbType = DbType.String, Value = cityID } 
            };

            var list = ExecuteSql_ByReaderReflect<City>(TMSReadOnlyConnection, sql, arguments);
            return list.Count > 0 ? list[0] : null;
        }

        public string GetCompanyNameByCompanyID(int id)
        {
            if (id <= 0)
            {
                return string.Empty;
            }
            string strSql = @"
                SELECT CompanyName
                FROM ExpressCompany
                WHERE ExpressCompanyID=:ExpressCompanyID";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "ExpressCompanyID", DbType = DbType.Int32, Value = id } 
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToString(o);
            }
            return string.Empty;
        }

        #endregion
    }
}
