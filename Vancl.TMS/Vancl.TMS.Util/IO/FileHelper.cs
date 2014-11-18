using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vancl.TMS.Util.IO
{
    public static class FileHelper
    {
        public static bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool Create(string filePath, bool canCover = false)
        {
            if (canCover)
            {
                try
                {
                    File.Create(filePath);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                if (!File.Exists(filePath))
                {
                    try
                    {
                        File.Create(filePath);
                        return true;
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static bool CreateDirectory(string path, bool canCover = false)
        {
            if (canCover)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                        return true;
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsDirExists(string path)
        {
            return Directory.Exists(path);
        }

        public static List<FileInfo> GetFileList(string dir, bool isAll, string searchPattern = "")
        {
            if (IsDirExists(dir))
            {
                List<FileInfo> fileList = new List<FileInfo>();
                string[] files;
                if (string.IsNullOrWhiteSpace(searchPattern))
                    files = Directory.GetFiles(dir);
                else
                    files = Directory.GetFiles(dir, searchPattern);
                string[] dirs = Directory.GetDirectories(dir);

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(Path.Combine(dir, file));
                        fileList.Add(fi);
                    }
                }

                if (isAll)
                {
                    if (dirs.Length > 0)
                    {
                        foreach (string subdir in dirs)
                        {
                            List<FileInfo> subFileList = GetFileList(Path.Combine(dir, subdir), true);
                            if (subFileList != null)
                                fileList.AddRange(subFileList);

                        }
                    }
                }

                return fileList;
            }
            else
            {
                return null;
            }
        }

        public static List<DirectoryInfo> GetDirList(string dir, string searchPattern = "")
        {
            if (IsDirExists(dir))
            {
                List<DirectoryInfo> dirList = new List<DirectoryInfo>();
                string[] dirs;
                if (string.IsNullOrWhiteSpace(searchPattern))
                    dirs = Directory.GetDirectories(dir);
                else
                    dirs = Directory.GetDirectories(dir, searchPattern);
                if (dirs.Length > 0)
                {
                    foreach (var subdir in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(Path.Combine(dir, subdir));
                        dirList.Add(di);
                    }
                }

                return dirList;
            }
            else
            {
                return null;
            }
        }
    }
}
