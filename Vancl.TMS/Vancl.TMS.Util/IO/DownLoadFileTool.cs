using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Vancl.TMS.Util.IO
{
    public sealed class DownLoadFileTool : IFileTransfer
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
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.UseBinary = true;
                    ftpRequest.Credentials = new NetworkCredential(ServerContext.UserName, ServerContext.PassWord);
                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    Stream ftpStream = response.GetResponseStream();
                    MemoryStream ms = new MemoryStream();
                    ftpStream.CopyTo(ms);
                    ms.Position = 0;
                    this.ActionFileStream = ms;

                    if (!String.IsNullOrWhiteSpace(LocalContext.LocalFilePath))
                    {
                        FileStream outputStream = new FileStream(LocalContext.LocalFilePath, FileMode.Create);
                        int bufferSize = 8192;
                        int readCount;
                        byte[] buffer = new byte[bufferSize];
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                        while (readCount > 0)
                        {
                            outputStream.Write(buffer, 0, readCount);
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                        }
                        outputStream.Close();
                    }

                    ftpStream.Close();
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
