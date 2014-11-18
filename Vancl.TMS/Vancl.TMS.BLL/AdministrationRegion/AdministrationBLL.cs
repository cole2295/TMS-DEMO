using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.AdministrationRegion;
using Vancl.TMS.IBLL.AdministrationRegion;

namespace Vancl.TMS.BLL.AdministrationRegion
{
    public class AdministrationBLL : BaseBLL, IAdministrationBLL
    {
        IAdministrationDAL _dal = ServiceFactory.GetService<IAdministrationDAL>("AdministrationDAL");

        #region IAdministrationBLL 成员

        public IList<Model.AdministrationRegion.District> GetDistrict()
        {
            return _dal.GetDistrict();
        }

        public Model.AdministrationRegion.District GetDistrict(string districtID)
        {
            return _dal.GetDistrict(districtID);
        }

        public IList<Model.AdministrationRegion.Province> GetProvince()
        {
            return _dal.GetProvince();
        }

        public IList<Model.AdministrationRegion.Province> GetProvinceByDistrictID(string districtID)
        {
            return _dal.GetProvinceByDistrictID(districtID);
        }

        public Model.AdministrationRegion.Province GetProvince(string provinceID)
        {
            return _dal.GetProvince(provinceID);
        }

        public IList<Model.AdministrationRegion.City> GetCity()
        {
            return _dal.GetCity();
        }

        public IList<Model.AdministrationRegion.City> GetCityByProvinceID(string provinceID)
        {
            return _dal.GetCityByProvinceID(provinceID);
        }

        public Model.AdministrationRegion.City GetCity(string cityID)
        {
            return _dal.GetCity(cityID);
        }

        #endregion
    }
}
