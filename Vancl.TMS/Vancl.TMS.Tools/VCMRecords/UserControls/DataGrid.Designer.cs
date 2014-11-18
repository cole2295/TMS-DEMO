namespace Vancl.TMS.Tools.VCMRecords.UserControls
{
    partial class DataGrid
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataGrid));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.pagesize_CMB = new System.Windows.Forms.ComboBox();
            this.firstPage_Btn = new System.Windows.Forms.Button();
            this.prePage_Btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.currentPage_TB = new System.Windows.Forms.TextBox();
            this.nextPage_Btn = new System.Windows.Forms.Button();
            this.lastPage_Btn = new System.Windows.Forms.Button();
            this.currentPageDes_Label = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.refresh_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(627, 341);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            // 
            // pagesize_CMB
            // 
            this.pagesize_CMB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pagesize_CMB.FormattingEnabled = true;
            this.pagesize_CMB.Items.AddRange(new object[] {
            "10",
            "20",
            "50",
            "100"});
            this.pagesize_CMB.Location = new System.Drawing.Point(4, 353);
            this.pagesize_CMB.Name = "pagesize_CMB";
            this.pagesize_CMB.Size = new System.Drawing.Size(70, 20);
            this.pagesize_CMB.TabIndex = 1;
            this.pagesize_CMB.SelectedIndexChanged += new System.EventHandler(this.pagesize_CMB_SelectedIndexChanged);
            // 
            // firstPage_Btn
            // 
            this.firstPage_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.firstPage_Btn.Image = ((System.Drawing.Image)(resources.GetObject("firstPage_Btn.Image")));
            this.firstPage_Btn.Location = new System.Drawing.Point(83, 351);
            this.firstPage_Btn.Name = "firstPage_Btn";
            this.firstPage_Btn.Size = new System.Drawing.Size(23, 23);
            this.firstPage_Btn.TabIndex = 2;
            this.firstPage_Btn.UseVisualStyleBackColor = true;
            this.firstPage_Btn.Click += new System.EventHandler(this.firstPage_Btn_Click);
            // 
            // prePage_Btn
            // 
            this.prePage_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prePage_Btn.Image = ((System.Drawing.Image)(resources.GetObject("prePage_Btn.Image")));
            this.prePage_Btn.Location = new System.Drawing.Point(113, 351);
            this.prePage_Btn.Name = "prePage_Btn";
            this.prePage_Btn.Size = new System.Drawing.Size(23, 23);
            this.prePage_Btn.TabIndex = 3;
            this.prePage_Btn.UseVisualStyleBackColor = true;
            this.prePage_Btn.Click += new System.EventHandler(this.prePage_Btn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "第";
            // 
            // currentPage_TB
            // 
            this.currentPage_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentPage_TB.Location = new System.Drawing.Point(171, 353);
            this.currentPage_TB.Name = "currentPage_TB";
            this.currentPage_TB.Size = new System.Drawing.Size(58, 21);
            this.currentPage_TB.TabIndex = 6;
            this.currentPage_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.currentPage_TB_KeyDown);
            // 
            // nextPage_Btn
            // 
            this.nextPage_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextPage_Btn.Image = ((System.Drawing.Image)(resources.GetObject("nextPage_Btn.Image")));
            this.nextPage_Btn.Location = new System.Drawing.Point(255, 351);
            this.nextPage_Btn.Name = "nextPage_Btn";
            this.nextPage_Btn.Size = new System.Drawing.Size(23, 23);
            this.nextPage_Btn.TabIndex = 7;
            this.nextPage_Btn.UseVisualStyleBackColor = true;
            this.nextPage_Btn.Click += new System.EventHandler(this.nextPage_Btn_Click);
            // 
            // lastPage_Btn
            // 
            this.lastPage_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastPage_Btn.Image = ((System.Drawing.Image)(resources.GetObject("lastPage_Btn.Image")));
            this.lastPage_Btn.Location = new System.Drawing.Point(284, 351);
            this.lastPage_Btn.Name = "lastPage_Btn";
            this.lastPage_Btn.Size = new System.Drawing.Size(23, 23);
            this.lastPage_Btn.TabIndex = 7;
            this.lastPage_Btn.UseVisualStyleBackColor = true;
            this.lastPage_Btn.Click += new System.EventHandler(this.lastPage_Btn_Click);
            // 
            // currentPageDes_Label
            // 
            this.currentPageDes_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentPageDes_Label.AutoSize = true;
            this.currentPageDes_Label.Location = new System.Drawing.Point(358, 356);
            this.currentPageDes_Label.Name = "currentPageDes_Label";
            this.currentPageDes_Label.Size = new System.Drawing.Size(143, 12);
            this.currentPageDes_Label.TabIndex = 8;
            this.currentPageDes_Label.Text = "共0页，显示第0至0条记录";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "页";
            // 
            // refresh_Btn
            // 
            this.refresh_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refresh_Btn.Image = ((System.Drawing.Image)(resources.GetObject("refresh_Btn.Image")));
            this.refresh_Btn.Location = new System.Drawing.Point(324, 351);
            this.refresh_Btn.Name = "refresh_Btn";
            this.refresh_Btn.Size = new System.Drawing.Size(23, 23);
            this.refresh_Btn.TabIndex = 7;
            this.refresh_Btn.UseVisualStyleBackColor = true;
            this.refresh_Btn.Click += new System.EventHandler(this.refresh_Btn_Click);
            // 
            // DataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.currentPageDes_Label);
            this.Controls.Add(this.refresh_Btn);
            this.Controls.Add(this.lastPage_Btn);
            this.Controls.Add(this.nextPage_Btn);
            this.Controls.Add(this.currentPage_TB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.prePage_Btn);
            this.Controls.Add(this.firstPage_Btn);
            this.Controls.Add(this.pagesize_CMB);
            this.Controls.Add(this.dataGridView);
            this.Name = "DataGrid";
            this.Size = new System.Drawing.Size(632, 376);
            this.Load += new System.EventHandler(this.DataGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ComboBox pagesize_CMB;
        private System.Windows.Forms.Button firstPage_Btn;
        private System.Windows.Forms.Button prePage_Btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox currentPage_TB;
        private System.Windows.Forms.Button nextPage_Btn;
        private System.Windows.Forms.Button lastPage_Btn;
        private System.Windows.Forms.Label currentPageDes_Label;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button refresh_Btn;
    }
}
