using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Vancl.TMS.IBLL.History;
using Vancl.TMS.Util.IO;
using System.IO;

namespace Vancl.TMS.BLL.History
{
    public class FileProcessingBLL : IFileProcessingBLL
    {
        #region IFileProcessingBLL 成员

        public int DeleteFile(string dir, DateTime deadLine, string searchPattern = "")
        {
            List<FileInfo> fileList = new List<FileInfo>();
            if (string.IsNullOrWhiteSpace(searchPattern))
                fileList = FileHelper.GetFileList(dir, true);
            else
                fileList = FileHelper.GetFileList(dir, true, searchPattern);
            if (fileList != null && fileList.Count > 0)
            {
                int count = 0;
                foreach (FileInfo item in fileList)
                {
                    if (item.CreationTime <= deadLine)
                    {
                        try
                        {
                            item.Delete();
                            count++;
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }

                return count;
            }

            return 0;
        }

        public string GetHelpFile(string sysname, string menuname)
        {
            using (PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient())
            {
                var sysnamedecode = HttpUtility.UrlEncode(sysname);
                var menunamedecode = HttpUtility.UrlEncode(menuname);
                var opreationGuid = client.GetAllOperationGuide(sysname, menuname);
                if (opreationGuid != null && opreationGuid.Length > 0)
                {
                    return opreationGuid[0].FilePath;
                }
                else
                {
                    return "";

                }
            }
        }

        #endregion
    }
}
