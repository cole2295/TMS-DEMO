using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.IO;

namespace Vancl.WuliuSys.ClientService.Update
{
    class Program
    {
        static void Main(string[] args)
        {
            //      args = @"'WuliuSysService' 'd:\temp\WuliuSysService\Download\20120928\update' 'C:\Program Files (x86)\Vancl\云配送平台客户端服务' 'd:\temp\WuliuSysService\Backup\'".Split(' ');
            
            if (args.Length != 4) return;

            string serviceName = args[0];
            string sourceDir = args[1];
            string destDir = args[2];
            string backDir = args[3];

            StopService(serviceName);
            try
            {
                DeleteFolderFiles(destDir);
                CopyFolder(sourceDir, destDir);
                StartService(serviceName);
            }
            catch (Exception)
            {
                StopService(serviceName);
                CopyFolder(backDir, destDir);
                StartService(serviceName);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName"></param>
        private static void StartService(string serviceName)
        {
            var controller = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName);
            if (controller != null && controller.Status != ServiceControllerStatus.Running)
            {
                try
                {
                    controller.Start();
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName"></param>
        private static void StopService(string serviceName)
        {
            var controller = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName);
            if (controller != null && controller.CanStop && controller.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    controller.Stop();
                }
                catch (Exception) { }
            }
            controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 20));
        }


        /// <summary>
        /// Copy文件夹
        /// </summary>
        /// <param name="sPath">源文件夹路径</param>
        /// <param name="dPath">目的文件夹路径</param>
        /// <returns>完成状态：success-完成；其他-报错</returns>
        private static bool CopyFolder(string sPath, string dPath)
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
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }


        /// <summary>
        /// 用递归方法删除文件夹目录及文件
        /// </summary>
        /// <param name="dir">带文件夹名的路径</param> 
        private static void DeleteFolder(string dir)
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

        private static void DeleteFolderFiles(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                    File.Delete(d); //直接删除其中的文件 
                else DeleteFolder(d); //递归删除子文件夹 
            }
        }

    }
}
