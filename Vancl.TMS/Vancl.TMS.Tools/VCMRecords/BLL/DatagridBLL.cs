using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Tools.VCMRecords.DAL;
using Vancl.TMS.Tools.VCMRecords.Tool.DataGrid;

namespace Vancl.TMS.Tools.VCMRecords.BLL
{
    /// <summary>
    /// 处理DataGrid
    /// </summary>
    public class DatagridBLL
    {
        DatagridDAL dal = new DatagridDAL();
        public void FindPageModel(PageModel pageModel)
        {
            dal.FindPageModel(pageModel);
        }
    }


}
