namespace Vancl.TMS.Tools.VCMRecords.MainForm.Forms
{
    partial class AddOrModifyVCMRecord
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddOrModifyVCMRecord));
            this.operationTime_DT = new System.Windows.Forms.DateTimePicker();
            this.promoteTime_DT = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.operationer_TB = new System.Windows.Forms.TextBox();
            this.promoter_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sql_TB = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.theme_TB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.desc_TB = new System.Windows.Forms.TextBox();
            this.OK_Btn = new System.Windows.Forms.Button();
            this.cancel_Btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.deliverOrderID_TB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.executeStatus_CB = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.uploadFile_toolStripabel = new System.Windows.Forms.ToolStripLabel();
            this.download_toolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.deleteFile_toolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.files_DG = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.files_DG)).BeginInit();
            this.SuspendLayout();
            // 
            // operationTime_DT
            // 
            this.operationTime_DT.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.operationTime_DT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.operationTime_DT.Location = new System.Drawing.Point(470, 53);
            this.operationTime_DT.Name = "operationTime_DT";
            this.operationTime_DT.Size = new System.Drawing.Size(154, 21);
            this.operationTime_DT.TabIndex = 5;
            this.operationTime_DT.Value = new System.DateTime(2011, 12, 28, 0, 0, 0, 0);
            // 
            // promoteTime_DT
            // 
            this.promoteTime_DT.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.promoteTime_DT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.promoteTime_DT.Location = new System.Drawing.Point(470, 23);
            this.promoteTime_DT.Name = "promoteTime_DT";
            this.promoteTime_DT.Size = new System.Drawing.Size(154, 21);
            this.promoteTime_DT.TabIndex = 2;
            this.promoteTime_DT.Value = new System.DateTime(2011, 12, 28, 0, 0, 0, 0);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(412, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "操作日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(412, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "提出日期：";
            // 
            // operationer_TB
            // 
            this.operationer_TB.Location = new System.Drawing.Point(278, 53);
            this.operationer_TB.Name = "operationer_TB";
            this.operationer_TB.Size = new System.Drawing.Size(129, 21);
            this.operationer_TB.TabIndex = 4;
            this.operationer_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // promoter_TB
            // 
            this.promoter_TB.Location = new System.Drawing.Point(278, 23);
            this.promoter_TB.Name = "promoter_TB";
            this.promoter_TB.Size = new System.Drawing.Size(129, 21);
            this.promoter_TB.TabIndex = 1;
            this.promoter_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(230, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "操作人：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "提出人：";
            // 
            // sql_TB
            // 
            this.sql_TB.Location = new System.Drawing.Point(61, 84);
            this.sql_TB.Multiline = true;
            this.sql_TB.Name = "sql_TB";
            this.sql_TB.Size = new System.Drawing.Size(346, 66);
            this.sql_TB.TabIndex = 6;
            this.sql_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(29, 84);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 12);
            this.label14.TabIndex = 11;
            this.label14.Text = "SQL：";
            // 
            // theme_TB
            // 
            this.theme_TB.Location = new System.Drawing.Point(62, 165);
            this.theme_TB.MaxLength = 128;
            this.theme_TB.Multiline = true;
            this.theme_TB.Name = "theme_TB";
            this.theme_TB.Size = new System.Drawing.Size(346, 66);
            this.theme_TB.TabIndex = 7;
            this.theme_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 165);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 13;
            this.label12.Text = "主题：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "描述：";
            // 
            // desc_TB
            // 
            this.desc_TB.Location = new System.Drawing.Point(62, 248);
            this.desc_TB.MaxLength = 256;
            this.desc_TB.Multiline = true;
            this.desc_TB.Name = "desc_TB";
            this.desc_TB.Size = new System.Drawing.Size(346, 66);
            this.desc_TB.TabIndex = 8;
            this.desc_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // OK_Btn
            // 
            this.OK_Btn.Location = new System.Drawing.Point(116, 336);
            this.OK_Btn.Name = "OK_Btn";
            this.OK_Btn.Size = new System.Drawing.Size(75, 23);
            this.OK_Btn.TabIndex = 9;
            this.OK_Btn.Text = "确定";
            this.OK_Btn.UseVisualStyleBackColor = true;
            this.OK_Btn.Click += new System.EventHandler(this.OK_Btn_Click);
            // 
            // cancel_Btn
            // 
            this.cancel_Btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Btn.Location = new System.Drawing.Point(481, 336);
            this.cancel_Btn.Name = "cancel_Btn";
            this.cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.cancel_Btn.TabIndex = 10;
            this.cancel_Btn.Text = "取消";
            this.cancel_Btn.UseVisualStyleBackColor = true;
            this.cancel_Btn.Click += new System.EventHandler(this.cancel_Btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "运单号：";
            // 
            // deliverOrderID_TB
            // 
            this.deliverOrderID_TB.Location = new System.Drawing.Point(61, 22);
            this.deliverOrderID_TB.Name = "deliverOrderID_TB";
            this.deliverOrderID_TB.Size = new System.Drawing.Size(154, 21);
            this.deliverOrderID_TB.TabIndex = 0;
            this.deliverOrderID_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-1, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "执行状态：";
            // 
            // executeStatus_CB
            // 
            this.executeStatus_CB.FormattingEnabled = true;
            this.executeStatus_CB.Items.AddRange(new object[] {
            "未执行",
            "已执行",
            "执行失败"});
            this.executeStatus_CB.Location = new System.Drawing.Point(61, 56);
            this.executeStatus_CB.Name = "executeStatus_CB";
            this.executeStatus_CB.Size = new System.Drawing.Size(154, 20);
            this.executeStatus_CB.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Controls.Add(this.files_DG);
            this.groupBox1.Location = new System.Drawing.Point(414, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 230);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "附件列表";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadFile_toolStripabel,
            this.download_toolStripLabel,
            this.deleteFile_toolStripLabel});
            this.toolStrip1.Location = new System.Drawing.Point(3, 17);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(391, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // uploadFile_toolStripabel
            // 
            this.uploadFile_toolStripabel.Image = ((System.Drawing.Image)(resources.GetObject("uploadFile_toolStripabel.Image")));
            this.uploadFile_toolStripabel.Name = "uploadFile_toolStripabel";
            this.uploadFile_toolStripabel.Size = new System.Drawing.Size(48, 22);
            this.uploadFile_toolStripabel.Text = "上传";
            this.uploadFile_toolStripabel.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // download_toolStripLabel
            // 
            this.download_toolStripLabel.Image = ((System.Drawing.Image)(resources.GetObject("download_toolStripLabel.Image")));
            this.download_toolStripLabel.Name = "download_toolStripLabel";
            this.download_toolStripLabel.Size = new System.Drawing.Size(48, 22);
            this.download_toolStripLabel.Text = "下载";
            this.download_toolStripLabel.Click += new System.EventHandler(this.download_toolStripLabel_Click);
            // 
            // deleteFile_toolStripLabel
            // 
            this.deleteFile_toolStripLabel.Image = ((System.Drawing.Image)(resources.GetObject("deleteFile_toolStripLabel.Image")));
            this.deleteFile_toolStripLabel.Name = "deleteFile_toolStripLabel";
            this.deleteFile_toolStripLabel.Size = new System.Drawing.Size(48, 22);
            this.deleteFile_toolStripLabel.Text = "删除";
            this.deleteFile_toolStripLabel.Click += new System.EventHandler(this.deleteFile_toolStripLabel_Click);
            // 
            // files_DG
            // 
            this.files_DG.AllowDrop = true;
            this.files_DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.files_DG.Location = new System.Drawing.Point(7, 42);
            this.files_DG.Name = "files_DG";
            this.files_DG.ReadOnly = true;
            this.files_DG.RowTemplate.Height = 23;
            this.files_DG.Size = new System.Drawing.Size(387, 182);
            this.files_DG.TabIndex = 0;
            this.files_DG.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.files_DG_CellMouseDoubleClick);
            this.files_DG.DragDrop += new System.Windows.Forms.DragEventHandler(this.files_DG_DragDrop);
            this.files_DG.DragEnter += new System.Windows.Forms.DragEventHandler(this.files_DG_DragEnter);
            // 
            // AddOrModifyVCMRecord
            // 
            this.AcceptButton = this.OK_Btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Btn;
            this.ClientSize = new System.Drawing.Size(823, 371);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.executeStatus_CB);
            this.Controls.Add(this.cancel_Btn);
            this.Controls.Add(this.OK_Btn);
            this.Controls.Add(this.desc_TB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.theme_TB);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.sql_TB);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.operationTime_DT);
            this.Controls.Add(this.promoteTime_DT);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deliverOrderID_TB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.operationer_TB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.promoter_TB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AddOrModifyVCMRecord";
            this.Text = "添加VCM日志";
            this.Load += new System.EventHandler(this.AddOrModifyVCMRecord_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.files_DG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker operationTime_DT;
        private System.Windows.Forms.DateTimePicker promoteTime_DT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox operationer_TB;
        private System.Windows.Forms.TextBox promoter_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sql_TB;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox theme_TB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox desc_TB;
        private System.Windows.Forms.Button OK_Btn;
        private System.Windows.Forms.Button cancel_Btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox deliverOrderID_TB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox executeStatus_CB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel uploadFile_toolStripabel;
        private System.Windows.Forms.ToolStripLabel download_toolStripLabel;
        private System.Windows.Forms.ToolStripLabel deleteFile_toolStripLabel;
        private System.Windows.Forms.DataGridView files_DG;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}