using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;

namespace Vancl.WuliuSys.ClientService.Tests
{
    /// <summary>
    /// ClearDir 的摘要说明
    /// </summary>
    [TestClass]
    public class ClearDir
    {
        public ClearDir()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShowProcess()
        {
            Process.GetProcesses().ToList().ForEach(
                x =>
                {
                    Console.WriteLine(x.ProcessName);
                }
                );
        }

        [TestMethod]
        public void DeleteDir()
        {
            Process.GetProcesses().ToList().ForEach(
                x =>
                {
                    Console.WriteLine(x.ProcessName);
                }
                );
            Process[] killprocess = Process.GetProcessesByName("ServiceUpdate");
            foreach (System.Diagnostics.Process p in killprocess)
            {
                p.Kill();
            }
            string dir = @"C:\Program Files (x86)\Vancl";
            DeleteFolder(dir);
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
