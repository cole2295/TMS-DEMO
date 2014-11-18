using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;
using System.Net;
using System.IO;

namespace Vancl.TMS.Util.IO
{
    public class FtpHelper : IDisposable
    {
        private FtpWebRequest ftpRequest;
        private FtpAction? _currentAction;
        private string _defaultRemotePath = string.Empty;
        private string _userName = string.Empty;
        private string _userPwd = string.Empty;
        private string _remoteFilePath = string.Empty;
        private string _remoteRelativeFilePath = string.Empty;
        private string _remoteRelativeDirectory = string.Empty;

        /// <summary>
        /// 获取文件流
        /// </summary>
        public Stream FileStream
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取或设置需要上传的文件流
        /// </summary>
        public Stream LocalFileStream
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">执行的操作</param>
        /// <param name="relativeFilePath">文件相对路径(不包含文件名)</param>
        /// <param name="remoteFileName">远程文件名(仅文件名和扩展名)</param>
        /// <param name="defualtRemotePath">FTP默认根文件夹路径</param>
        /// <param name="userName">FTP用户名</param>
        /// <param name="userPwd">FTP密码</param>
        public FtpHelper(FtpAction action, string relativeFilePath, string remoteFileName, string defualtRemotePath = "", string userName = "", string userPwd = "")
        {
            this._currentAction = action;
            this._defaultRemotePath = string.IsNullOrWhiteSpace(defualtRemotePath) ? ConfigurationHelper.GetAppSetting("FileServerDefaultFtpAddress") : defualtRemotePath;
            this._userName = string.IsNullOrWhiteSpace(userName) ? ConfigurationHelper.GetAppSetting("FileServerFtpUserName") : userName;
            this._userPwd = string.IsNullOrWhiteSpace(userPwd) ? ConfigurationHelper.GetAppSetting("FileServerFtpUserPwd") : userPwd;
            this._remoteRelativeDirectory = relativeFilePath;
            this._remoteRelativeFilePath = Path.Combine(relativeFilePath, remoteFileName);
            this._remoteFilePath = Path.Combine(this._defaultRemotePath, this._remoteRelativeFilePath);
        }
        public FtpHelper(FtpAction action, string remoteFileName, Uri uri, string userName = "", string userPwd = "")
            : this(action, "", remoteFileName, uri.ToString(), userName, userPwd)
        {
        }

        public void Action()
        {
            if (this._currentAction.HasValue)
            {
                try
                {
                    switch (this._currentAction.Value)
                    {
                        case FtpAction.Delete:
                            DeleteAction();
                            break;
                        case FtpAction.DownLoad:
                            DownLoadAction();
                            break;
                        case FtpAction.UpLoad:
                            UpLoadAction();
                            break;
                        default:
                            throw new Exception("无法识别指定的操作类型");
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                throw new Exception("未指定操作内容");
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        private void UpLoadAction()
        {
            if (this.LocalFileStream != null)
            {
                Stream stream = this.LocalFileStream;
                if (!string.IsNullOrWhiteSpace(this._remoteRelativeDirectory))
                {
                    AutoCreateDirectory(this._remoteRelativeDirectory);
                }
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(this._remoteFilePath));
                ftpRequest.Credentials = new NetworkCredential(this._userName, this._userPwd);
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
            else
            {
                throw new Exception("无上传文件流");
            }
        }

        #region 上传文件相关私有方法

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
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(this._defaultRemotePath + "/" + tagDir));
                ftp.Credentials = new NetworkCredential(this._userName, this._userPwd);

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
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(this._defaultRemotePath + pDirName + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this._userName, this._userPwd);
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

        /// <summary>
        /// 下载文件
        /// </summary>
        private void DownLoadAction()
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(this._remoteFilePath));
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpRequest.UseBinary = true;
            ftpRequest.Credentials = new NetworkCredential(this._userName, this._userPwd);
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            MemoryStream ms = new MemoryStream();
            ftpStream.CopyTo(ms);
            ms.Position = 0;
            this.FileStream = ms;

            ftpStream.Close();
            response.Close();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        private void DeleteAction()
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(this._remoteFilePath));

            ftpRequest.Credentials = new NetworkCredential(this._userName, this._userPwd);
            ftpRequest.KeepAlive = false;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            Stream datastream = response.GetResponseStream();
            MemoryStream ms = new MemoryStream();
            datastream.CopyTo(ms);
            ms.Position = 0;
            this.FileStream = ms;
            datastream.Close();
            response.Close();
        }



        #region IDisposable 成员

        public void Dispose()
        {
            if (this.LocalFileStream != null)
                this.LocalFileStream.Dispose();
            if (this.FileStream != null)
                this.FileStream.Dispose();
            this.ftpRequest = null;
        }

        #endregion
    }
}
