namespace LineIDRepair
{
    partial class frmMain
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
            this.btnLinePalnBackup = new System.Windows.Forms.Button();
            this.btnRepair = new System.Windows.Forms.Button();
            this.btnLinePlanRestore = new System.Windows.Forms.Button();
            this.btnTransportPlanBackup = new System.Windows.Forms.Button();
            this.btnTransportPlanRestore = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLinePalnBackup
            // 
            this.btnLinePalnBackup.Location = new System.Drawing.Point(12, 12);
            this.btnLinePalnBackup.Name = "btnLinePalnBackup";
            this.btnLinePalnBackup.Size = new System.Drawing.Size(120, 23);
            this.btnLinePalnBackup.TabIndex = 0;
            this.btnLinePalnBackup.Text = "备份线路计划数据";
            this.btnLinePalnBackup.UseVisualStyleBackColor = true;
            this.btnLinePalnBackup.Click += new System.EventHandler(this.btnLinePalnBackup_Click);
            // 
            // btnRepair
            // 
            this.btnRepair.Location = new System.Drawing.Point(12, 117);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(120, 23);
            this.btnRepair.TabIndex = 4;
            this.btnRepair.Text = "修复线路编号";
            this.btnRepair.UseVisualStyleBackColor = true;
            this.btnRepair.Click += new System.EventHandler(this.btnRepair_Click);
            // 
            // btnLinePlanRestore
            // 
            this.btnLinePlanRestore.Location = new System.Drawing.Point(187, 12);
            this.btnLinePlanRestore.Name = "btnLinePlanRestore";
            this.btnLinePlanRestore.Size = new System.Drawing.Size(120, 23);
            this.btnLinePlanRestore.TabIndex = 1;
            this.btnLinePlanRestore.Text = "恢复线路计划数据";
            this.btnLinePlanRestore.UseVisualStyleBackColor = true;
            this.btnLinePlanRestore.Click += new System.EventHandler(this.btnLinePlanRestore_Click);
            // 
            // btnTransportPlanBackup
            // 
            this.btnTransportPlanBackup.Location = new System.Drawing.Point(12, 51);
            this.btnTransportPlanBackup.Name = "btnTransportPlanBackup";
            this.btnTransportPlanBackup.Size = new System.Drawing.Size(120, 23);
            this.btnTransportPlanBackup.TabIndex = 2;
            this.btnTransportPlanBackup.Text = "备份运输计划数据";
            this.btnTransportPlanBackup.UseVisualStyleBackColor = true;
            this.btnTransportPlanBackup.Click += new System.EventHandler(this.btnTransportPlanBackup_Click);
            // 
            // btnTransportPlanRestore
            // 
            this.btnTransportPlanRestore.Location = new System.Drawing.Point(187, 51);
            this.btnTransportPlanRestore.Name = "btnTransportPlanRestore";
            this.btnTransportPlanRestore.Size = new System.Drawing.Size(120, 23);
            this.btnTransportPlanRestore.TabIndex = 3;
            this.btnTransportPlanRestore.Text = "恢复运输计划数据";
            this.btnTransportPlanRestore.UseVisualStyleBackColor = true;
            this.btnTransportPlanRestore.Click += new System.EventHandler(this.btnTransportPlanRestore_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(187, 117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 153);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTransportPlanRestore);
            this.Controls.Add(this.btnTransportPlanBackup);
            this.Controls.Add(this.btnLinePlanRestore);
            this.Controls.Add(this.btnRepair);
            this.Controls.Add(this.btnLinePalnBackup);
            this.Name = "frmMain";
            this.Text = "线路编号修复工具";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLinePalnBackup;
        private System.Windows.Forms.Button btnRepair;
        private System.Windows.Forms.Button btnLinePlanRestore;
        private System.Windows.Forms.Button btnTransportPlanBackup;
        private System.Windows.Forms.Button btnTransportPlanRestore;
        private System.Windows.Forms.Button btnCancel;
    }
}

