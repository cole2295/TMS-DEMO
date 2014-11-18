using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Vancl.TMS.Util.IO
{
    public class IOHelper
    {
        /// <summary>
        /// 获取文件的绝对路径
        /// </summary>
        /// <remarks>
        /// modify by zbd 201201291720
        /// 修改逻辑为向上级目录遍历查找，以适应单元测试项目
        /// </remarks>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(string dirName)
        {
            string OriginalPath = AppDomain.CurrentDomain.BaseDirectory;

            IEnumerable<string> directorys = OriginalPath.Split('\\').ToList();

            //是否为目录，根据文件名是否包含'.'判断
            bool isDirectory = !dirName.Split('\\').Last().Any(x => x == '.');
            while (directorys.Count() > 1)
            {
                directorys = directorys.Take(directorys.Count() - 1);
                string path = Path.Combine(string.Join("\\", directorys), dirName);
                if (isDirectory)
                {
                    if (Directory.Exists(path))
                    {
                        return path;
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            throw new Exception("未找到相关配置文件。");
        }

        /// <summary>
        /// 取得文件内容
        /// </summary>
        /// <param name="strFileFullName"></param>
        /// <returns></returns>
        public static string GetFileContent(string strFileFullName)
        {
            FileInfo info = new FileInfo(strFileFullName);
            using (FileStream stream = info.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    stream.Flush();
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 按照文件夹名称升序取得线程文件夹
        /// </summary>
        /// <param name="directPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SearchDirectory(string directPath, string searchPattern)
        {
            if (Directory.Exists(directPath))
            {
                DirectoryInfo direcInfo = new DirectoryInfo(directPath);
                DirectoryInfo[] subdirecInfos = direcInfo.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);
                if (subdirecInfos != null
                    && subdirecInfos.Length > 0)
                {
                    Dictionary<string, string> listdirectName = new Dictionary<string, string>(subdirecInfos.Length);
                    IOrderedEnumerable<DirectoryInfo> orderdirecInfos = subdirecInfos.OrderBy(p => p.Name);
                    foreach (var item in orderdirecInfos)
                    {
                        listdirectName.Add(item.Name, item.FullName);
                    }
                    return listdirectName;
                }
            }
            return null;
        }

        /// <summary>
        /// 搜索每个文件夹排名靠前的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="TopNum"></param>
        /// <returns></returns>
        public static string[] SearchFile(string filePath, string searchPattern, int TopNum)
        {
            if (Directory.Exists(filePath))
            {
                DirectoryInfo direcInfo = new DirectoryInfo(filePath);
                FileInfo[] fileInfos = direcInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                if (fileInfos != null
                    && fileInfos.Length > 0)
                {
                    IEnumerable<FileInfo> topTenFile = fileInfos.OrderBy(p => p.LastAccessTime).Take(TopNum);
                    List<string> listFileName = new List<string>(topTenFile.Count());
                    foreach (var item in topTenFile)
                    {
                        listFileName.Add(item.Name);
                    }
                    return listFileName.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// 搜索最早的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string SearchFile(string filePath, string searchPattern)
        {
            if (Directory.Exists(filePath))
            {
                DirectoryInfo direcInfo = new DirectoryInfo(filePath);
                FileInfo[] fileInfos = direcInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                if (fileInfos != null
                    && fileInfos.Length > 0)
                {
                    fileInfos.OrderBy(p => p.LastAccessTime);
                    return fileInfos.First().FullName;
                }
            }
            return "";
        }

        public static string GetFileFullPath(string FileFullName)
        {
            FileInfo info = new FileInfo(FileFullName);
            return info.DirectoryName;
        }

        public static string GetFileShortName(string FileFullName)
        {
            FileInfo info = new FileInfo(FileFullName);
            return info.Name;
        }

        public static void Remove(string From, string To)
        {
            if (string.IsNullOrEmpty(From)
                || string.IsNullOrEmpty(To))
            {
                throw new IOException(string.Format("从文件{0}移到{1}时,源或者目的文件名为空", From, To));
            }
            try
            {
                string directoryName = GetFileFullPath(To);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                File.Move(From, To);
            }
            catch (IOException ex)
            {
                throw new IOException(string.Format("从文件{0}移到{1}时,出现异常", From, To), ex);
            }
        }

        /// <summary>
        ///序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void EntityToFile<T>(T data, string path) where T : new()
        {
            if (null == data) throw new ArgumentNullException("data");
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string tmpPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, data);
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T FileToEntity<T>(string path) where T : new()
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            CreateFileDirectory(path);
            T data = new T();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                data = (T)serializer.Deserialize(fs);
            }
            return data;
        }


        /// <summary>
        /// 序列化对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void EntitiesToFile<T>(List<T> data, string path)
        {
            if (data.Count > 0)
            {
                CreateFileDirectory(path);
                XmlSerializer serializer = new XmlSerializer(data.GetType());

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, data);
                }
            }
        }

        /// <summary>
        /// 反序列化列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<T> FileToEntities<T>(string path)
        {
            List<T> list = new List<T>();

            if (path != null && path != string.Empty)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    list = (List<T>)serializer.Deserialize(fs);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据文件路径创建目录
        /// </summary>
        /// <param name="fileFullPath"></param>
        public static void CreateFileDirectory(string fileFullPath)
        {
            string dir = new FileInfo(fileFullPath).DirectoryName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

    }
}
