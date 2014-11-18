using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Util.IO
{
    public sealed class UpLoadFileTool : IFileTransfer
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
                    Stream stream = String.IsNullOrWhiteSpace(LocalContext.LocalFilePath) ? LocalContext.DataStream : File.Open(LocalContext.LocalFilePath, FileMode.Open);
                    if (!string.IsNullOrWhiteSpace(ServerContext.ServerPath))
                    {
                        AutoCreateDirectory(ServerContext.ServerPath);
                    }
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ServerContext.FilePath));
                    ftpRequest.Credentials = new NetworkCredential(ServerContext.UserName, ServerContext.PassWord);
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    ftpRequest.UseBinary = true;
                    ftpRequest.ContentLength = stream.Length;
                    byte[] buff = new byte[stream.Length];
                    int contentLen;
                    Stream strm = ftpRequest.GetRequestStream();
                    contentLen = stream.Read(buff, 0, int.Parse(stream.Length.ToString()));
                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);
                        contentLen = stream.Read(buff, 0, int.Parse(stream.Length.ToString()));
                    }
                    strm.Close();
                    stream.Close();
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

        #region 私有方法

        /// <summary>
        /// 判断当前目录下指定的子目录是否存在,不存在自动创建目录
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        private void AutoCreateDirectory(string RemoteDirectoryName)
        {
            RemoteDirectoryName = RemoteDirectoryName.Replace("\\", "/");
            if (RemoteDirectoryName.IndexOf('/') != 0)
                RemoteDirectoryName = "/" + RemoteDirectoryName;
            if (RemoteDirectoryName.LastIndexOf('/') == RemoteDirectoryName.Length - 1)
                RemoteDirectoryName = RemoteDirectoryName.Substring(0, RemoteDirectoryName.Length - 2);
            string[] dirs = RemoteDirectoryName.Split('/');
            if (dirs.Length == 2)
            {
                if (!DrectoryExist(dirs[0], dirs[1]))
                    MakeDir(dirs[0], dirs[1]);

            }
            string currentDir = string.Empty;
            for (int i = 0; i < dirs.Length - 1; i++)
            {
                if (dirs[i] != "")
                    currentDir += "/" + dirs[i];
                if (DrectoryExist(currentDir, dirs[i + 1]))
                    continue;
                else
                    MakeDir(currentDir, dirs[i + 1]);
            }
        }

        /// <summary>
        /// 判断当前文件夹下是否存在目标文件夹
        /// </summary>
        /// <param name="currentDir">当前目录名</param>
        /// <param name="nextDir">目标目录名</param>
        /// <returns></returns>
        private bool DrectoryExist(string currentDir, string nextDir)
        {
            string[] dirList = GetDirectoryList(currentDir);
            foreach (string str in dirList)
            {
                if (GetDirName(str.Trim()).Trim() == nextDir.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定目录下明细(包含文件和文件夹)
        /// </summary>
        /// <param name="tagDir">目录名</param>
        /// <returns></returns>
        private string[] GetFilesDetailList(string tagDir)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ServerContext.DefaultPath + "/" + tagDir));
                ftp.Credentials = new NetworkCredential(ServerContext.UserName, ServerContext.PassWord);

                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");

                    line = reader.ReadLine();
                }
                if (result.ToString() != "")
                    result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定目录下所有的文件夹列表(仅文件夹)
        /// </summary>
        /// <param name="tagDir">目录名</param>
        /// <returns></returns>
        private string[] GetDirectoryList(string tagDir)
        {
            string[] drectory = GetFilesDetailList(tagDir);
            string m = string.Empty;
            if (drectory != null)
            {
                foreach (string str in drectory)
                {
                    if (str != "")
                    {
                        if (str.Trim().Substring(0, 1).ToUpper() == "D")
                        {
                            m += str.Trim() + "\n";
                        }
                    }
                }
            }
            char[] n = new char[] { '\n' };
            return m.Split(n);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="pDirName">要创建新文件夹的目录</param>
        /// <param name="dirName">文件夹名称</param>
        private void MakeDir(string pDirName, string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                if (pDirName != string.Empty && pDirName != null)
                    pDirName = pDirName + "/";
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ServerContext.DefaultPath + pDirName + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ServerContext.UserName, ServerContext.PassWord);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解析ftp文件名
        /// </summary>
        /// <param name="originalName">原始文件名</param>
        /// <returns></returns>
        private string GetDirName(string originalName)
        {
            string[] s = originalName.Replace(" ", "$").Split('$');
            return s[s.Length - 1];
        }

        #endregion
    }
}
