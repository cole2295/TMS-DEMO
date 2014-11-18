using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Common;

namespace Vancl.TMS.BLL.Common
{
    public class TransPointBLL : ITransPointBLL
    {
        #region ITransPointBLL 成员

        public List<Model.BaseInfo.ExpressCompanyModel> GetDepartures()
        {
            TransPointService.TmsDataServiceClient client = new TransPointService.TmsDataServiceClient();
            try
            {
                List<Model.BaseInfo.ExpressCompanyModel> list = new List<Model.BaseInfo.ExpressCompanyModel>();
                Dictionary<int, string> dic = client.FromDeliverCenter();
                if (dic != null && dic.Count > 0)
                {
                    foreach (KeyValuePair<int,string> item in dic)
                    {
                        Model.BaseInfo.ExpressCompanyModel ec = new Model.BaseInfo.ExpressCompanyModel();
                        ec.CompanyAllName = item.Value;
                        ec.CompanyCode = item.Key.ToString();

                        list.Add(ec);
                    }

                    return list;
                }
                else
                    return null;
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

        public List<Model.BaseInfo.ExpressCompanyModel> GetArrivals(int departureId)
        {
            TransPointService.TmsDataServiceClient client = new TransPointService.TmsDataServiceClient();
            try
            {
                List<Model.BaseInfo.ExpressCompanyModel> list = new List<Model.BaseInfo.ExpressCompanyModel>();

                Dictionary<int, string> dic = client.ToDeliverCenter(departureId);
                if (dic != null && dic.Count > 0)
                {
                    foreach (KeyValuePair<int, string> item in dic)
                    {
                        Model.BaseInfo.ExpressCompanyModel ec = new Model.BaseInfo.ExpressCompanyModel();
                        ec.CompanyAllName = item.Value;
                        ec.CompanyCode = item.Key.ToString();

                        list.Add(ec);
                    }

                    return list;
                }
                else
                    return null;
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

        #endregion
    }
}
