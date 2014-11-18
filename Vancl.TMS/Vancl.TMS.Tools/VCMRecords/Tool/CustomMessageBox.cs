using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vancl.TMS.Tools.VCMRecords.Tool
{
    /// <summary>
    /// 自定义弹出框
    /// </summary>
    public class CustomMessageBox
    {
        /// <summary>
        /// 弹出警告类提示框
        /// </summary>
        /// <param name="content"></param>
        public static DialogResult Alert(String content,String title ="警告" ,
            MessageBoxButtons btn = MessageBoxButtons.OK,MessageBoxIcon  icon = MessageBoxIcon.Warning)
        {
            return MessageBox.Show(content, title, btn, icon);
        }

        /// <summary>
        /// 弹出错误类提示框
        /// </summary>
        /// <param name="content"></param>
        public static DialogResult Error(String content, String title = "错误", 
            MessageBoxButtons btn = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            return  MessageBox.Show(content, title, btn, icon);
        }

        /// <summary>
        /// 普通的信息提示框
        /// </summary>
        /// <param name="content"></param>
        public static DialogResult Tips(String content, String title = "提示",
            MessageBoxButtons btn = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return MessageBox.Show(content, title, btn, icon);
        }

    }
}
