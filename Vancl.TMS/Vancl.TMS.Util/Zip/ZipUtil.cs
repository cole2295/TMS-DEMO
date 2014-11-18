using System;
using System.Collections.Generic;
using System.Text;
using SevenZip;
using System.Configuration;
using System.Web;
using log4net;
using System.IO;

namespace Vancl.TMS.Util.Zip
{
    /*
 * (C)Copyright 2011-2012 TR类库
 * 
 * 模块名称：ZIP工具
 * 说明：使用7z压缩,解压
 * 作者：任钰
 * 创建日期：2011-09-21 14:34:00
 * 修改人：
 * 修改时间：
 * 修改记录：
 */
    public class ZipUtil
    {
        private string _senvenZipPath;
        private readonly ILog _ilogger = LogManager.GetLogger(typeof(ZipUtil));
        private readonly SevenZipCompressor _zipCompressor;

        /// <summary>
        /// SenvenPath Real Path
        /// </summary>
        public string SenvenZipPath
        {
            get
            {
                return _senvenZipPath;
            }
        }

        public ZipUtil(string zipConfigName)
        {
            _senvenZipPath = zipConfigName;
            SetZipSoftPath();
            _zipCompressor = new SevenZipCompressor();
        }

        private void SetZipSoftPath()
        {
            SevenZipCompressor.SetLibraryPath(_senvenZipPath);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="strOrigFileName">原始文件</param>
        /// <param name="strZipFileName">压缩文件</param>
        /// <param name="strMsg">错误消息</param>
        /// <returns></returns>
        public bool CompressFiles(string strOrigFileName, string strZipFileName, out string strMsg)
        {
            try
            {
                SetZipFormat(strZipFileName);
                _zipCompressor.CompressFiles(strZipFileName, strOrigFileName);
            }
            catch (Exception e)
            {
                strMsg = e.Message;
                _ilogger.ErrorFormat("压缩文件出错:原始文件名\t{0},压缩文件名\t{1}\t 原异常数据:{2}"
                    , strOrigFileName
                    , strZipFileName
                    , e.Message);
                return false;
            }
            strMsg = "";
            return true;
        }

        /// <summary>
        /// 设置压缩格式
        /// </summary>
        /// <param name="strZipFileName"></param>
        private void SetZipFormat(string strZipFileName)
        {
            string fileExt = Path.GetExtension(strZipFileName).ToLower();
            _zipCompressor.ArchiveFormat = fileExt.Equals(".zip") ? OutArchiveFormat.Zip : OutArchiveFormat.SevenZip;
        }


        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="strOrigDirecPath">原始文件夹路径</param>
        /// <param name="strZipFileName">压缩文件名</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool CompressDirectory(string strOrigDirecPath, string strZipFileName, out string strMsg)
        {
            try
            {
                SetZipFormat(strZipFileName);
                _zipCompressor.CompressDirectory(strOrigDirecPath, strZipFileName);
            }
            catch (Exception e)
            {
                strMsg = e.Message;
                _ilogger.ErrorFormat("压缩文件夹出错:原始文件夹\t{0},压缩文件名\t{1}\t 原异常数据:{2}"
                    , strOrigDirecPath
                    , strZipFileName
                    , e.Message);
                return false;
            }
            strMsg = "";
            return true;
        }

        /// <summary>
        /// 解压file到当前目录
        /// </summary>
        /// <param name="strZipFileName"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool ExtractFiles(string strZipFileName, out string strMsg)
        {
            try
            {
                SevenZipExtractor zipExtractor = new SevenZipExtractor(strZipFileName);
                zipExtractor.ExtractArchive(Path.GetFullPath(strZipFileName));
            }
            catch (Exception e)
            {
                strMsg = e.Message;
                _ilogger.ErrorFormat("解压文件夹出错:原始压缩文件\t{0},解压后文件夹名\t{1}\t 原异常数据:{2}"
                    , strZipFileName
                    , Path.GetFullPath(strZipFileName)
                    , e.Message);
                return false;
            }
            strMsg = "";
            return true;
        }

        /// <summary>
        /// 解压到指定目录
        /// </summary>
        /// <param name="strZipFileName"></param>
        /// <param name="strExtractFileName">指定的解压路径</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool ExtractFiles(string strZipFileName, string strExtractFileName, out string strMsg)
        {
            if (string.IsNullOrEmpty(strExtractFileName))
            {
                return ExtractFiles(strZipFileName, out strMsg);
            }
            try
            {
                SevenZipExtractor zipExtractor = new SevenZipExtractor(strZipFileName);
                zipExtractor.ExtractArchive(strExtractFileName);
            }
            catch (Exception e)
            {
                strMsg = e.Message;
                _ilogger.ErrorFormat("解压文件夹出错:原始压缩文件\t{0},解压后文件夹名\t{1}\t 原异常数据:{2}"
                    , strZipFileName
                    , strExtractFileName
                    , e.Message);
                return false;
            }
            strMsg = "";
            return true;
        }

    }
}
