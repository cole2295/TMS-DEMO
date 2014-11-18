using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Reflection;
using System.IO;
using Ionic.Zip;
using Vancl.WuliuSys.ClientLib.Properties;
using System.Diagnostics;
using System.ServiceProcess;

namespace Vancl.WuliuSys.ClientLib.AutoUpdate
{
    public class ServiceUpdate
    {
        static ServiceUpdate workerObject;
        static Thread updateThread;
        static ServiceUpdate()
        {
            workerObject = new ServiceUpdate();
            updateThread = new Thread(workerObject.DoUpdate);
        }
        public static void StartCheckUpdate()
        {
            updateThread.Start();
        }
        public static void StopCheckUpdate()
        {
            if (updateThread.IsAlive)
            {
                updateThread.Abort();
            }
        }

        private string WorkDir;
        private string BackupDir;
        private string DownLoadDir;
        private string ServiceName = "WuliuSysService";

        public ServiceUpdate()
        {
              WorkDir = Path.GetTempPath() + "\\" + ServiceName + "\\";
           // WorkDir = "d:\\temp\\" + ServiceName + "\\";
            BackupDir = WorkDir + AutoUpdateSettings.Default.BackupDir + "\\";
            DownLoadDir = WorkDir + AutoUpdateSettings.Default.DownLoadDir + "\\";
        }


        public void DoUpdate()
        {
            while (true)
            {
                try
                {
                    if (Directory.Exists(WorkDir))
                    {
                        DeleteFolder(WorkDir);
                    }

                    string fileName = CheckAndDownloadUpdate();
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        string dir = UnzipUpdateFile(fileName);
                        string exeFilePath = ExtractExeFile();
                        BackupServiceFiles();

                        //   StopService(ServiceName);
                        string arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", ServiceName, dir, this.GetAssemblyPath(), BackupDir);
                     //   TestOut(exeFilePath + "|" + arguments);
                        Process.Start("\"" + exeFilePath + "\"", arguments);
                    }
                }
                catch (Exception)
                {
                }

                Thread.Sleep(AutoUpdateSettings.Default.Interval);
            }
        }

        private void TestOut(string arguments)
        {
            FileStream aFile = new FileStream("d:\\temp\\out.txt ", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine(arguments);
            sw.Close();
            aFile.Close();
        }

        /// <summary>
        /// 检查更新并下载
        /// </summary>
        /// <returns></returns>
        private string CheckAndDownloadUpdate()
        {
            using (WebClient wc = new WebClient())
            {
                var strCfg = wc.DownloadString(AutoUpdateSettings.Default.Url);

                if (string.IsNullOrWhiteSpace(strCfg)) return null;
                var cfg = strCfg.Split('|');
                string version = cfg[0];
                string downloadUrl = cfg[1];

                if (AutoUpdateSettings.Default.CurrentVersion == version) return null;


                string dir = DownLoadDir + version + "\\";
                if (Directory.Exists(dir)) return null;
                else Directory.CreateDirectory(dir);

                string fileName = dir + "update.zip";
                wc.DownloadFile(downloadUrl, fileName);

                return fileName;
            }
        }

        /// <summary>
        /// 解压压缩包
        /// </summary>
        /// <param name="fileName"></param>
        private string UnzipUpdateFile(string fileName)
        {
            if (!File.Exists(fileName)) return "";

            using (ZipFile zip = ZipFile.Read(fileName))
            {
                var dir = fileName.TrimEnd(".zip".ToArray());
                //  zip.Password = "123456";//密码解压
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract(dir);
                }
                return dir;
            }
        }

        /// <summary>
        /// 提取升级文件
        /// </summary>
        private string ExtractExeFile()
        {
            string filePath = WorkDir + "\\ServiceUpdate.exe";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                var fileBytes = ClientLibResource.ServiceUpdate;
                fs.Write(fileBytes, 0, fileBytes.Length);
                fs.Close();
            }
            return filePath;
        }

        /// <summary>
        /// 备份服务文件
        /// </summary>
        private void BackupServiceFiles()
        {
            this.CopyFolder(GetAssemblyPath(), BackupDir);
        }

        /// <summary>
        /// 获取执行程序路径
        /// </summary>
        /// <returns></returns>
        private string GetAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        }

        /// <summary>
        /// Copy文件夹
        /// </summary>
        /// <param name="sPath">源文件夹路径</param>
        /// <param name="dPath">目的文件夹路径</param>
        /// <returns>完成状态：success-完成；其他-报错</returns>
        public bool CopyFolder(string sPath, string dPath)
        {
            //源文件夹不存在
            if (!Directory.Exists(sPath)) return true;

            bool flag = true;
            try
            {
                // 创建目的文件夹
                if (!Directory.Exists(dPath))
                {
                    Directory.CreateDirectory(dPath);
                }

                // 拷贝文件
                DirectoryInfo sDir = new DirectoryInfo(sPath);
                FileInfo[] fileArray = sDir.GetFiles();
                foreach (FileInfo file in fileArray)
                {
                    file.CopyTo(dPath + "\\" + file.Name, true);
                }

                // 循环子文件夹
                DirectoryInfo dDir = new DirectoryInfo(dPath);
                DirectoryInfo[] subDirArray = sDir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirArray)
                {
                    CopyFolder(subDir.FullName, dPath + "//" + subDir.Name);
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        private void StopService(string serviceName)
        {
            var controller = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName);
            if (controller != null && controller.CanStop)
            {
                controller.Stop();
                controller.Dispose();
            }
            controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 20));
        }

        /// <summary>
        /// 用递归方法删除文件夹目录及文件
        /// </summary>
        /// <param name="dir">带文件夹名的路径</param> 
        private void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件 
                    else DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir, true); //删除已空文件夹 
            }
        }

    }
}
