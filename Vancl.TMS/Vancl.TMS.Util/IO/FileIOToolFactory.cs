using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.IO
{
    public static class FileIOToolFactory
    {
        public static IFileTransfer GetFileIOTool(FtpAction action)
        {
            switch (action)
            { 
                case FtpAction.Delete:
                    return new DeleteFileTool();
                case FtpAction.DownLoad:
                    return new DownLoadFileTool();
                case FtpAction.UpLoad:
                    return new UpLoadFileTool();
                default:
                    return null;
            }
        }
    }
}
