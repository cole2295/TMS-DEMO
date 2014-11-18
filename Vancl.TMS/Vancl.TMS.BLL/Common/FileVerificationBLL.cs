using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.Util.IO;
using System.IO;
using System.Net;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.BLL.Common
{
    public class FileVerificationBLL : IVerificationCode
    {
        #region IVerificationCode 成员

        public string SetVerificationCode(string code)
        {
            try
            {
                IFileTransfer tool = FileIOToolFactory.GetFileIOTool(FtpAction.UpLoad);
                string fileName = Guid.NewGuid().ToString();
                tool.LocalContext = new FtpTransferLocalContext()
                {
                    DataStream = new MemoryStream(Encoding.Default.GetBytes(code))
                };
                tool.ServerContext = new FtpTransferServerContext();
                tool.ServerContext.FileName = Path.Combine(ConfigurationHelper.GetAppSetting("FileServerVerificationCodesPath"), fileName);

                tool.DoAction();
                tool.LocalContext.DataStream.Close();

                return fileName;
            }
            //catch (WebException)
            //{
            //    return string.Empty;
            //}
            catch
            {
                throw;
            }
        }

        public bool VerifyCode(string id, string code, bool clearCode)
        {
            try
            {
                IFileTransfer tool = FileIOToolFactory.GetFileIOTool(FtpAction.DownLoad);
                tool.LocalContext = new FtpTransferLocalContext();
                tool.ServerContext = new FtpTransferServerContext();
                tool.ServerContext.FileName = Path.Combine(ConfigurationHelper.GetAppSetting("FileServerVerificationCodesPath"), id);
                tool.DoAction();
                StreamReader sr = new StreamReader(tool.ActionFileStream);
                string s = sr.ReadToEnd();
                tool.ActionFileStream.Close();

                if (clearCode)
                {
                    IFileTransfer deletetool = FileIOToolFactory.GetFileIOTool(FtpAction.Delete);
                    deletetool.LocalContext = new FtpTransferLocalContext();
                    deletetool.ServerContext = tool.ServerContext;
                    deletetool.DoAction();
                }

                return string.Compare(code, s, true) == 0;
            }
            //catch (WebException)
            //{
            //    return false;
            //}
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
