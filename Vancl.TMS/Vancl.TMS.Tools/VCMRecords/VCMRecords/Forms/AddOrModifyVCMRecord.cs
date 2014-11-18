using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;
using Vancl.TMS.Tools.VCMRecords.Tool;
using Vancl.TMS.Tools.VCMRecords.Entity;
using Vancl.TMS.Tools.VCMRecords.Tool.File;
using Vancl.TMS.Tools.VCMRecords.BLL;

namespace Vancl.TMS.Tools.VCMRecords.MainForm.Forms
{
    public partial class AddOrModifyVCMRecord : Form
    {
        #region 成员

        /// <summary>
        /// VCM记录(增加/修改)
        /// </summary>
        private VCMRecord record;

        /// <summary>
        /// 待增加的附件
        /// </summary>
        private List<VCMFile> addFiles=new List<VCMFile>();

        /// <summary>
        /// 待删除的附件
        /// </summary>
        private List<VCMFile> delFiles = new List<VCMFile>();

        private List<VCMFile> sourceFiles = new List<VCMFile>();

        /// <summary>
        /// 操作日志 业务处理
        /// </summary>
        private VCMRecordBLL recordBLL = new VCMRecordBLL();

        /// <summary>
        /// 附件 业务处理
        /// </summary>
        private VCMFileBLL fileBLL = new VCMFileBLL();

        #endregion

        #region 页面事件

        /// <summary>
        /// 若record的ID为空则代表添加，否则代表修改
        /// </summary>
        /// <param name="record"></param>
        public AddOrModifyVCMRecord(VCMRecord record)
        {
            InitializeComponent();
            this.record = record;
        }      

        /// <summary>
        /// 页面载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrModifyVCMRecord_Load(object sender, EventArgs e)
        {
            SetFilesDataGridViewColumnStyle();
            SetControlsTips();
            if (record.ID == -1)
            {
                InitAddVCMRecord();
            }
            else
            {
                InitModifyVCMRecord();
            }
        }
       
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定按钮点击事件（添加/修改 VCM日志）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Btn_Click(object sender, EventArgs e)
        {
            InputsToRecord();
            bool checkresult = CheckRecord();
            if (checkresult == false)
            {
                return;
            }
            ReturnResult result;
            if (record.ID == -1)//添加VCM日志
            {
                result = recordBLL.Create(record);
                foreach (VCMFile vcmFile in addFiles)
                {
                    vcmFile.RecordID = record.ID;
                }
            }
            else//修改
            {
                result = recordBLL.Update(record);
            }
            if (result.Result == false)
            {
                CustomMessageBox.Tips(result.Fault.Obj.ToString(), result.Fault.Detail);
                return;
            }
            DealFiles();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 处理一些输入框Ctrl+A全选的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCtrlAKeyDown_EventHandler(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            } 
        }

        #endregion

        #region 文件列表操作

        /// <summary>
        /// 上传文件 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            String[] filenames = CustomFileDialog.selectFile();
            UploadFiles(filenames);
        }

        /// <summary>
        /// 把文件上传到数据库中
        /// </summary>
        /// <param name="filenames"></param>
        private void UploadFiles(String[] filenames)
        {
            if (filenames == null)
                return;
            List<VCMFile> vcmFiles = new List<VCMFile>();
            try
            {
                foreach (String filename in filenames)
                {
                    FileInfo fileInfo = new FileInfo(filename);
                    VCMFile vcmFile = new VCMFile();
                    vcmFile.FileContent = CustomFile.serializeFileToString(filename);
                    vcmFile.Size = (int)fileInfo.Length;
                    vcmFile.RecordID = record.ID;
                    vcmFile.SizeLabel = CustomFile.getFileSizelabel(fileInfo.Length);
                    vcmFile.FileName = fileInfo.Name;
                    vcmFile.UploadTime = DateTime.Now;
                    vcmFiles.Add(vcmFile);

                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Tips(ex.Message);
                return;
            }
            addFiles.AddRange(vcmFiles);
            RefreshFilesDB();
        }
      

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteFile_toolStripLabel_Click(object sender, EventArgs e)
        {
            if (files_DG.SelectedRows.Count == 0)
            {
                CustomMessageBox.Tips("你没有选择要删除的记录");
                return;
            }
            List<VCMFile> displayFiles = files_DG.DataSource as  List<VCMFile> ;
            List<VCMFile> delfiles = new List<VCMFile>();
            foreach (DataGridViewRow row in files_DG.SelectedRows)
            {
                VCMFile vcmFile = displayFiles[row.Index];
                if (addFiles.Contains(vcmFile))
                {
                    addFiles.Remove(vcmFile);
                }
                else
                {
                    sourceFiles.Remove(vcmFile);
                    delFiles.Add(vcmFile);
                }
            }
            RefreshFilesDB();            
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void download_toolStripLabel_Click(object sender, EventArgs e)
        {
            if (files_DG.DataSource == null)
                return;
            if (files_DG.SelectedRows.Count == 0)
            {
                CustomMessageBox.Tips("你没有选择要下载的文件");
                return;
            }
            FolderBrowserDialog saveDlg = new FolderBrowserDialog();
            saveDlg.Description="选择文件存放位置";
            List<VCMFile> displayFiles = files_DG.DataSource as List<VCMFile>;
            if(saveDlg.ShowDialog()!= System.Windows.Forms.DialogResult.OK)
                return;
            String path = saveDlg.SelectedPath;
            foreach (DataGridViewRow row in files_DG.SelectedRows)
            {
                VCMFile vcmFile = displayFiles[row.Index];
                if (String.IsNullOrEmpty(vcmFile.FileContent))
                {
                    vcmFile.FileContent = fileBLL.FindFileContent(vcmFile);
                }
                string filename = String.Format("{0}\\{1}", path, vcmFile.FileName);
                CustomFile.serializeStringToFile(vcmFile.FileContent, filename);   
            }           
        }


        /// <summary>
        /// 文件拖进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void files_DG_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        /// <summary>
        /// 文件拖进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void files_DG_DragDrop(object sender, DragEventArgs e)
        {
            String[] files = e.Data.GetData(DataFormats.FileDrop, false) as String[];
            UploadFiles(files);
        }

        /// <summary>
        /// 双击某一列，代表下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void files_DG_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            files_DG.ClearSelection();
            files_DG.Rows[e.RowIndex].Selected = true;
            download_toolStripLabel_Click(sender, e);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 设置控件的ToolTips
        /// </summary>
        private void SetControlsTips()
        {
            toolTip1.SetToolTip(theme_TB, "最多输入128字");
            toolTip1.SetToolTip(desc_TB, "最多输入256字");
        }

        /// <summary>
        /// 初使化添加VCM日志记录
        /// </summary>
        private void InitAddVCMRecord()
        {
            this.Text = "增加VCM日志";
            this.executeStatus_CB.SelectedIndex = 0;
            promoteTime_DT.Value = DateTime.Now;
            operationTime_DT.Value = DateTime.Now;

        }

        /// <summary>
        /// 初使化修改VCM记录
        /// </summary>
        private void InitModifyVCMRecord()
        {
            this.Text = "修改VCM日志";
            //把record里的信息显示到控件中
            this.promoter_TB.Text = record.PromoteEmployee;
            this.promoteTime_DT.Value = record.PromoteTime;
            this.operationer_TB.Text = record.OperationEmployee;
            this.operationTime_DT.Value = record.OperationTime;
            this.sql_TB.Text = record.SQL;
            this.desc_TB.Text = record.Description;
            this.deliverOrderID_TB.Text = record.DeliverOrderID.ToString();
            this.executeStatus_CB.SelectedIndex = (int)record.ExecuteStatus;
            this.theme_TB.Text = record.Theme;
            //显示日志列表
            sourceFiles = fileBLL.Find(record);
            files_DG.DataSource = sourceFiles;
        }

        /// <summary>
        /// 把控件的输入项更新到当前VCM日志(更新/添加)中
        /// </summary>
        private void InputsToRecord()
        {
            record.PromoteEmployee = this.promoter_TB.Text.Trim();
            record.PromoteTime = this.promoteTime_DT.Value;
            record.SQL = this.sql_TB.Text.Trim();
            record.OperationEmployee = this.operationer_TB.Text.Trim();
            record.OperationTime = this.operationTime_DT.Value;
            record.Description = this.desc_TB.Text.Trim();
            record.ExecuteStatus = (ExecuteStatusType)this.executeStatus_CB.SelectedIndex;
            record.Theme = this.theme_TB.Text.Trim();
        }

        /// <summary>
        /// 检查输入数据是否合理
        /// </summary>
        /// <returns></returns>
        private bool CheckRecord()
        {
            if (record.PromoteEmployee == "")
            {
                CustomMessageBox.Tips("请输入提出人");
                promoter_TB.Focus();
                return false;
            }
            if (record.OperationEmployee == "")
            {
                CustomMessageBox.Tips("请输入操作人");
                operationer_TB.Focus();
                return false;
            }
            if (deliverOrderID_TB.Text.Trim() == "")
            {
                CustomMessageBox.Tips("请输入运单号");
                deliverOrderID_TB.Focus();
                return false;
            }
            try
            {
                record.DeliverOrderID= Convert.ToInt64(deliverOrderID_TB.Text);
            }
            catch
            {
                CustomMessageBox.Tips("运单号必须为整形");
                deliverOrderID_TB.Text = "";
                deliverOrderID_TB.Focus();
                return false;
            }
            if (record.SQL == "")
            {
                CustomMessageBox.Tips("请输入SQL");
                sql_TB.Focus();
                return false;
            }
            if (record.Theme == "")
            {
                CustomMessageBox.Tips("请输入主题");
                theme_TB.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置日志列表的显示样式
        /// </summary>
        private void SetFilesDataGridViewColumnStyle()
        {
            DataGridViewTextBoxColumn fileNameColumn = new DataGridViewTextBoxColumn();
            fileNameColumn.DataPropertyName = "FileName";
            fileNameColumn.Name = "FileName";
            fileNameColumn.HeaderText = "文件名";
            DataGridViewTextBoxColumn sizeLabelColumn = new DataGridViewTextBoxColumn();
            sizeLabelColumn.DataPropertyName = "SizeLabel";
            sizeLabelColumn.Name = "SizeLabel";
            sizeLabelColumn.HeaderText = "文件大小";
            DataGridViewTextBoxColumn uploadTimeLabelColumn = new DataGridViewTextBoxColumn();
            uploadTimeLabelColumn.DataPropertyName = "UploadTime";
            uploadTimeLabelColumn.Name = "UploadTime";
            uploadTimeLabelColumn.HeaderText = "上传日期";
            uploadTimeLabelColumn.Width = 127;
            uploadTimeLabelColumn.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss ";

            files_DG.Columns.Add(fileNameColumn);
            files_DG.Columns.Add(sizeLabelColumn);
            files_DG.Columns.Add(uploadTimeLabelColumn);

            files_DG.AutoGenerateColumns = false;
        }

        /// <summary>
        /// 刷新日志列表
        /// </summary>
        private void RefreshFilesDB()
        {
            List<VCMFile> files = new List<VCMFile>();
            files.AddRange(sourceFiles);
            files.AddRange(addFiles);
            files_DG.DataSource = files;            
        }

        /// <summary>
        /// 确定后处理记录的附件
        /// </summary>
        private void DealFiles()
        {
            //处理增加的文件
            fileBLL.Create(addFiles.ToArray());
            //处理删除的文件
            fileBLL.Remove(delFiles.ToArray());
        }
        #endregion


        

       

 

    }
}
