using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.AdministrationRegion;

namespace Vancl.TMS.IBLL.AdministrationRegion
{
    public interface IAdministrationBLL
    {
        IList<District> GetDistrict();
        District GetDistrict(string districtID);

        IList<Province> GetProvince();
        IList<Province> GetProvinceByDistrictID(string districtID);
        Province GetProvince(string provinceID);

        IList<City> GetCity();
        IList<City> GetCityByProvinceID(string provinceID);
        City GetCity(string cityID);
    }
}
