using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.BLL.PmsService;
using System.Configuration;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Common
{
    public class PermissionBLL : IPermissionBLL
    {
        #region IPermissionBLL 成员

        public PmsNotice GetNotice()
        {
            using (PermissionOpenServiceClient client = new PermissionOpenServiceClient())
            {
                var sysId = Convert.ToInt32(ConfigurationManager.AppSettings["SystemId"]);
                var notice = client.GetSysNotice(sysId, "");

                if (notice == null) return null;
                return new PmsNotice
                {
                    CreateBy = notice.CreateBy,
                    CreateTime = notice.CreateTime,
                    DistributionCode = notice.DistributionCode,
                    IsDelete = notice.IsDelete,
                    NoitceContent = notice.NoitceContent,
                    Signature = notice.Signature,
                    SystemId = notice.SystemId,
                    Title = notice.Title,
                    UpdateBy = notice.UpdateBy,
                    UpdateTime = notice.UpdateTime,
                };
            }
        }

        #endregion
    }
}
