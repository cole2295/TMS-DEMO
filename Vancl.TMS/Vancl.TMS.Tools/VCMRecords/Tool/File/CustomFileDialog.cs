using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Vancl.TMS.Tools.VCMRecords.Tool.File
{
    /// <summary>
    /// 文件对话框相关处理函数（非窗体应用程序不能调用文件对话框）
    /// </summary>
    public static class CustomFileDialog
    {
        /// <summary>
        /// 得到文件保存路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static String getSavePath(string filename)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.DefaultExt = Path.GetExtension(filename);
            saveDlg.Title = "选择存放文件位置";
            saveDlg.FilterIndex = 1;
            saveDlg.AddExtension = true;
            saveDlg.FileName = filename;
            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return saveDlg.FileName;
            }
            return null;
        }
        

        /// <summary>
        /// 选择一组文件
        /// </summary>
        /// <returns>文件名的数组</returns>
        public static String[] selectFile()
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "选择要上传的文件";
            DialogResult dr = openFileDlg.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDlg.FileNames;
            }
            return null;
        }
    }
}
