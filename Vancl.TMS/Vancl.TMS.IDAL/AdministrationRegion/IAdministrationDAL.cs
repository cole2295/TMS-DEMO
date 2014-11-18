using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.AdministrationRegion;

namespace Vancl.TMS.IDAL.AdministrationRegion
{
    public interface IAdministrationDAL
    {
        IList<District> GetDistrict();
        District GetDistrict(string districtID);

        IList<Province> GetProvince();
        IList<Province> GetProvinceByDistrictID(string districtID);
        Province GetProvince(string provinceID);

        IList<City> GetCity();
        IList<City> GetCityByProvinceID(string provinceID);
        City GetCity(string cityID);

        /// <summary>
        /// 根据站点id取得站点名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetCompanyNameByCompanyID(int id);
    }
}
