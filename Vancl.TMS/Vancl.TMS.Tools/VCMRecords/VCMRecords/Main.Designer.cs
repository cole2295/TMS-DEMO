namespace Vancl.TMS.Tools.VCMRecords.MainForm
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.records_groupBox = new System.Windows.Forms.GroupBox();
            this.records_toolStrip = new System.Windows.Forms.ToolStrip();
            this.addRecord_Menu = new System.Windows.Forms.ToolStripLabel();
            this.modifyRecord_Menu = new System.Windows.Forms.ToolStripLabel();
            this.delrecord_Menu = new System.Windows.Forms.ToolStripLabel();
            this.queryRecords_groupBox = new System.Windows.Forms.GroupBox();
            this.executeStatus_CB = new System.Windows.Forms.ComboBox();
            this.queryRecord_Btn = new System.Windows.Forms.Button();
            this.operationTimeEnd_DT = new System.Windows.Forms.DateTimePicker();
            this.promoteTimeEnd_DT = new System.Windows.Forms.DateTimePicker();
            this.operationTimeStart_DT = new System.Windows.Forms.DateTimePicker();
            this.promoteTimeStart_DT = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.theme_TB = new System.Windows.Forms.TextBox();
            this.sql_TB = new System.Windows.Forms.TextBox();
            this.operationer_TB = new System.Windows.Forms.TextBox();
            this.deliverOrderID_TB = new System.Windows.Forms.TextBox();
            this.promoter_TB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ClearInputs = new System.Windows.Forms.Button();
            this.vcmRecordsDataGrid = new Vancl.TMS.Tools.VCMRecords.UserControls.DataGrid();
            this.records_groupBox.SuspendLayout();
            this.records_toolStrip.SuspendLayout();
            this.queryRecords_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // records_groupBox
            // 
            this.records_groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.records_groupBox.Controls.Add(this.records_toolStrip);
            this.records_groupBox.Controls.Add(this.vcmRecordsDataGrid);
            this.records_groupBox.Location = new System.Drawing.Point(2, 178);
            this.records_groupBox.Name = "records_groupBox";
            this.records_groupBox.Size = new System.Drawing.Size(979, 354);
            this.records_groupBox.TabIndex = 2;
            this.records_groupBox.TabStop = false;
            this.records_groupBox.Text = "VCM日志列表";
            // 
            // records_toolStrip
            // 
            this.records_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRecord_Menu,
            this.modifyRecord_Menu,
            this.delrecord_Menu});
            this.records_toolStrip.Location = new System.Drawing.Point(3, 17);
            this.records_toolStrip.Name = "records_toolStrip";
            this.records_toolStrip.Size = new System.Drawing.Size(973, 25);
            this.records_toolStrip.TabIndex = 1;
            this.records_toolStrip.Text = "toolStrip2";
            // 
            // addRecord_Menu
            // 
            this.addRecord_Menu.Image = ((System.Drawing.Image)(resources.GetObject("addRecord_Menu.Image")));
            this.addRecord_Menu.Name = "addRecord_Menu";
            this.addRecord_Menu.Size = new System.Drawing.Size(48, 22);
            this.addRecord_Menu.Text = "新增";
            this.addRecord_Menu.Click += new System.EventHandler(this.addRecord_Menu_Click);
            // 
            // modifyRecord_Menu
            // 
            this.modifyRecord_Menu.Image = ((System.Drawing.Image)(resources.GetObject("modifyRecord_Menu.Image")));
            this.modifyRecord_Menu.Name = "modifyRecord_Menu";
            this.modifyRecord_Menu.Size = new System.Drawing.Size(48, 22);
            this.modifyRecord_Menu.Text = "修改";
            this.modifyRecord_Menu.Click += new System.EventHandler(this.modifyRecord_Menu_Click);
            // 
            // delrecord_Menu
            // 
            this.delrecord_Menu.Image = ((System.Drawing.Image)(resources.GetObject("delrecord_Menu.Image")));
            this.delrecord_Menu.Name = "delrecord_Menu";
            this.delrecord_Menu.Size = new System.Drawing.Size(48, 22);
            this.delrecord_Menu.Text = "删除";
            this.delrecord_Menu.Click += new System.EventHandler(this.delrecord_Menu_Click);
            // 
            // queryRecords_groupBox
            // 
            this.queryRecords_groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryRecords_groupBox.Controls.Add(this.btn_ClearInputs);
            this.queryRecords_groupBox.Controls.Add(this.executeStatus_CB);
            this.queryRecords_groupBox.Controls.Add(this.queryRecord_Btn);
            this.queryRecords_groupBox.Controls.Add(this.operationTimeEnd_DT);
            this.queryRecords_groupBox.Controls.Add(this.promoteTimeEnd_DT);
            this.queryRecords_groupBox.Controls.Add(this.operationTimeStart_DT);
            this.queryRecords_groupBox.Controls.Add(this.promoteTimeStart_DT);
            this.queryRecords_groupBox.Controls.Add(this.label6);
            this.queryRecords_groupBox.Controls.Add(this.label2);
            this.queryRecords_groupBox.Controls.Add(this.theme_TB);
            this.queryRecords_groupBox.Controls.Add(this.sql_TB);
            this.queryRecords_groupBox.Controls.Add(this.operationer_TB);
            this.queryRecords_groupBox.Controls.Add(this.deliverOrderID_TB);
            this.queryRecords_groupBox.Controls.Add(this.promoter_TB);
            this.queryRecords_groupBox.Controls.Add(this.label5);
            this.queryRecords_groupBox.Controls.Add(this.label8);
            this.queryRecords_groupBox.Controls.Add(this.label3);
            this.queryRecords_groupBox.Controls.Add(this.label7);
            this.queryRecords_groupBox.Controls.Add(this.label4);
            this.queryRecords_groupBox.Controls.Add(this.label10);
            this.queryRecords_groupBox.Controls.Add(this.label9);
            this.queryRecords_groupBox.Controls.Add(this.label1);
            this.queryRecords_groupBox.Location = new System.Drawing.Point(2, 11);
            this.queryRecords_groupBox.Name = "queryRecords_groupBox";
            this.queryRecords_groupBox.Size = new System.Drawing.Size(976, 165);
            this.queryRecords_groupBox.TabIndex = 0;
            this.queryRecords_groupBox.TabStop = false;
            this.queryRecords_groupBox.Text = "查询条件";
            // 
            // executeStatus_CB
            // 
            this.executeStatus_CB.FormattingEnabled = true;
            this.executeStatus_CB.Items.AddRange(new object[] {
            "未执行",
            "已执行",
            "执行失败"});
            this.executeStatus_CB.Location = new System.Drawing.Point(64, 52);
            this.executeStatus_CB.Name = "executeStatus_CB";
            this.executeStatus_CB.Size = new System.Drawing.Size(162, 20);
            this.executeStatus_CB.TabIndex = 4;
            // 
            // queryRecord_Btn
            // 
            this.queryRecord_Btn.Font = new System.Drawing.Font("宋体", 20F);
            this.queryRecord_Btn.Location = new System.Drawing.Point(803, 91);
            this.queryRecord_Btn.Name = "queryRecord_Btn";
            this.queryRecord_Btn.Size = new System.Drawing.Size(169, 50);
            this.queryRecord_Btn.TabIndex = 10;
            this.queryRecord_Btn.Text = "查询";
            this.queryRecord_Btn.UseVisualStyleBackColor = true;
            this.queryRecord_Btn.Click += new System.EventHandler(this.queryRecord_Btn_Click);
            // 
            // operationTimeEnd_DT
            // 
            this.operationTimeEnd_DT.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.operationTimeEnd_DT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.operationTimeEnd_DT.Location = new System.Drawing.Point(631, 50);
            this.operationTimeEnd_DT.Name = "operationTimeEnd_DT";
            this.operationTimeEnd_DT.Size = new System.Drawing.Size(152, 21);
            this.operationTimeEnd_DT.TabIndex = 7;
            // 
            // promoteTimeEnd_DT
            // 
            this.promoteTimeEnd_DT.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.promoteTimeEnd_DT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.promoteTimeEnd_DT.Location = new System.Drawing.Point(631, 20);
            this.promoteTimeEnd_DT.Name = "promoteTimeEnd_DT";
            this.promoteTimeEnd_DT.Size = new System.Drawing.Size(152, 21);
            this.promoteTimeEnd_DT.TabIndex = 3;
            // 
            // operationTimeStart_DT
            // 
            this.operationTimeStart_DT.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.operationTimeStart_DT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.operationTimeStart_DT.Location = new System.Drawing.Point(458, 50);
            this.operationTimeStart_DT.Name = "operationTimeStart_DT";
            this.operationTimeStart_DT.Size = new System.Drawing.Size(154, 21);
            this.operationTimeStart_DT.TabIndex = 6;
            this.operationTimeStart_DT.Value = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            // 
            // promoteTimeStart_DT
            // 
            this.promoteTimeStart_DT.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.promoteTimeStart_DT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.promoteTimeStart_DT.Location = new System.Drawing.Point(458, 20);
            this.promoteTimeStart_DT.Name = "promoteTimeStart_DT";
            this.promoteTimeStart_DT.Size = new System.Drawing.Size(154, 21);
            this.promoteTimeStart_DT.TabIndex = 2;
            this.promoteTimeStart_DT.Value = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(398, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "操作日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(398, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "提出日期：";
            // 
            // theme_TB
            // 
            this.theme_TB.Location = new System.Drawing.Point(457, 75);
            this.theme_TB.MaxLength = 128;
            this.theme_TB.Multiline = true;
            this.theme_TB.Name = "theme_TB";
            this.theme_TB.Size = new System.Drawing.Size(329, 66);
            this.theme_TB.TabIndex = 9;
            this.theme_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // sql_TB
            // 
            this.sql_TB.Location = new System.Drawing.Point(64, 75);
            this.sql_TB.Multiline = true;
            this.sql_TB.Name = "sql_TB";
            this.sql_TB.Size = new System.Drawing.Size(329, 66);
            this.sql_TB.TabIndex = 8;
            this.sql_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // operationer_TB
            // 
            this.operationer_TB.Location = new System.Drawing.Point(292, 54);
            this.operationer_TB.MaxLength = 5;
            this.operationer_TB.Name = "operationer_TB";
            this.operationer_TB.Size = new System.Drawing.Size(100, 21);
            this.operationer_TB.TabIndex = 5;
            this.operationer_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // deliverOrderID_TB
            // 
            this.deliverOrderID_TB.Location = new System.Drawing.Point(63, 24);
            this.deliverOrderID_TB.MaxLength = 32;
            this.deliverOrderID_TB.Name = "deliverOrderID_TB";
            this.deliverOrderID_TB.Size = new System.Drawing.Size(163, 21);
            this.deliverOrderID_TB.TabIndex = 0;
            this.deliverOrderID_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // promoter_TB
            // 
            this.promoter_TB.Location = new System.Drawing.Point(292, 24);
            this.promoter_TB.MaxLength = 5;
            this.promoter_TB.Name = "promoter_TB";
            this.promoter_TB.Size = new System.Drawing.Size(100, 21);
            this.promoter_TB.TabIndex = 1;
            this.promoter_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCtrlAKeyDown_EventHandler);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(616, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(422, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "主题：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(616, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "SQL：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "操作人：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "执行状态：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "运单号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "提出人：";
            // 
            // btn_ClearInputs
            // 
            this.btn_ClearInputs.Font = new System.Drawing.Font("宋体", 20F);
            this.btn_ClearInputs.Location = new System.Drawing.Point(803, 20);
            this.btn_ClearInputs.Name = "btn_ClearInputs";
            this.btn_ClearInputs.Size = new System.Drawing.Size(169, 50);
            this.btn_ClearInputs.TabIndex = 11;
            this.btn_ClearInputs.Text = "清除";
            this.btn_ClearInputs.UseVisualStyleBackColor = true;
            this.btn_ClearInputs.Click += new System.EventHandler(this.btn_ClearInputs_Click);
            // 
            // vcmRecordsDataGrid
            // 
            this.vcmRecordsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vcmRecordsDataGrid.Location = new System.Drawing.Point(0, 45);
            this.vcmRecordsDataGrid.Name = "vcmRecordsDataGrid";
            this.vcmRecordsDataGrid.PageModel = null;
            this.vcmRecordsDataGrid.Size = new System.Drawing.Size(979, 309);
            this.vcmRecordsDataGrid.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 533);
            this.Controls.Add(this.queryRecords_groupBox);
            this.Controls.Add(this.records_groupBox);
            this.Name = "Main";
            this.Text = "VCM管理工具";
            this.Load += new System.EventHandler(this.Main_Load);
            this.records_groupBox.ResumeLayout(false);
            this.records_groupBox.PerformLayout();
            this.records_toolStrip.ResumeLayout(false);
            this.records_toolStrip.PerformLayout();
            this.queryRecords_groupBox.ResumeLayout(false);
            this.queryRecords_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.DataGrid vcmRecordsDataGrid;
        private System.Windows.Forms.GroupBox records_groupBox;
        private System.Windows.Forms.ToolStrip records_toolStrip;
        private System.Windows.Forms.ToolStripLabel addRecord_Menu;
        private System.Windows.Forms.ToolStripLabel modifyRecord_Menu;
        private System.Windows.Forms.GroupBox queryRecords_groupBox;
        private System.Windows.Forms.DateTimePicker operationTimeEnd_DT;
        private System.Windows.Forms.DateTimePicker promoteTimeEnd_DT;
        private System.Windows.Forms.DateTimePicker operationTimeStart_DT;
        private System.Windows.Forms.DateTimePicker promoteTimeStart_DT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox operationer_TB;
        private System.Windows.Forms.TextBox promoter_TB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox theme_TB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripLabel delrecord_Menu;
        private System.Windows.Forms.Button queryRecord_Btn;
        private System.Windows.Forms.TextBox deliverOrderID_TB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox executeStatus_CB;
        private System.Windows.Forms.TextBox sql_TB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_ClearInputs;

    }
}

