using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Vancl.TMS.Util.IO
{
    public sealed class DeleteFileTool : IFileTransfer
    {
        private FtpWebRequest ftpRequest;

        #region IFileTransfer 成员

        public event EventHandler BeForeTransfer;

        public event EventHandler AfterTransfer;

        public FtpTransferLocalContext LocalContext
        {
            get;
            set;
        }

        public FtpTransferServerContext ServerContext
        {
            get;
            set;
        }

        public Stream ActionFileStream
        {
            get;
            set;
        }

        public void DoAction()
        {
            if (BeForeTransfer != null)
                BeForeTransfer(this, new EventArgs());

            if (LocalContext != null && ServerContext != null)
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ServerContext.FilePath));

                    ftpRequest.Credentials = new NetworkCredential(ServerContext.UserName, ServerContext.PassWord);
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    Stream datastream = response.GetResponseStream();
                    MemoryStream ms = new MemoryStream();
                    datastream.CopyTo(ms);
                    ms.Position = 0;
                    this.ActionFileStream = ms;
                    datastream.Close();
                    response.Close();
                }
                catch
                {
                    throw;
                }
            }

            if (AfterTransfer != null)
                AfterTransfer(this, new EventArgs());
        }

        #endregion
    }
}
