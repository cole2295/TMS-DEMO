namespace Vancl.TMS.ConfigManagerTool
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbServiceConfigPath = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbAssName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioMsSql = new System.Windows.Forms.RadioButton();
            this.radioOracle = new System.Windows.Forms.RadioButton();
            this.radioBll = new System.Windows.Forms.RadioButton();
            this.cbIsGeneric = new System.Windows.Forms.CheckBox();
            this.tbInterfaceName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbClassFullName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbServiceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbfFormulaConfigPath = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbfDesc = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tbfAssName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbfInterfaceName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbfClassName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbfFormulaName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbfIsGeneric = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(458, 305);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbServiceConfigPath);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.tbAssName);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.tbDesc);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.radioMsSql);
            this.tabPage1.Controls.Add(this.radioOracle);
            this.tabPage1.Controls.Add(this.radioBll);
            this.tabPage1.Controls.Add(this.cbIsGeneric);
            this.tabPage1.Controls.Add(this.tbInterfaceName);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.tbClassFullName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbServiceName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(450, 279);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "服务配置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbServiceConfigPath
            // 
            this.tbServiceConfigPath.Enabled = false;
            this.tbServiceConfigPath.Location = new System.Drawing.Point(69, 249);
            this.tbServiceConfigPath.Name = "tbServiceConfigPath";
            this.tbServiceConfigPath.Size = new System.Drawing.Size(357, 21);
            this.tbServiceConfigPath.TabIndex = 18;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 252);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 12);
            this.label15.TabIndex = 17;
            this.label15.Text = "路  径:";
            // 
            // tbAssName
            // 
            this.tbAssName.Enabled = false;
            this.tbAssName.Location = new System.Drawing.Point(69, 123);
            this.tbAssName.Name = "tbAssName";
            this.tbAssName.Size = new System.Drawing.Size(357, 21);
            this.tbAssName.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "程序集:";
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(69, 153);
            this.tbDesc.Multiline = true;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(357, 89);
            this.tbDesc.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "描  述:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(432, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(432, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(432, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "*";
            // 
            // radioMsSql
            // 
            this.radioMsSql.AutoSize = true;
            this.radioMsSql.Location = new System.Drawing.Point(209, 10);
            this.radioMsSql.Name = "radioMsSql";
            this.radioMsSql.Size = new System.Drawing.Size(101, 16);
            this.radioMsSql.TabIndex = 9;
            this.radioMsSql.Text = "MSSql数据服务";
            this.radioMsSql.UseVisualStyleBackColor = true;
            this.radioMsSql.CheckedChanged += new System.EventHandler(this.radioMsSql_CheckedChanged);
            // 
            // radioOracle
            // 
            this.radioOracle.AutoSize = true;
            this.radioOracle.Location = new System.Drawing.Point(96, 9);
            this.radioOracle.Name = "radioOracle";
            this.radioOracle.Size = new System.Drawing.Size(107, 16);
            this.radioOracle.TabIndex = 8;
            this.radioOracle.Text = "Oracle数据服务";
            this.radioOracle.UseVisualStyleBackColor = true;
            this.radioOracle.CheckedChanged += new System.EventHandler(this.radioOracle_CheckedChanged);
            // 
            // radioBll
            // 
            this.radioBll.AutoSize = true;
            this.radioBll.Location = new System.Drawing.Point(19, 9);
            this.radioBll.Name = "radioBll";
            this.radioBll.Size = new System.Drawing.Size(71, 16);
            this.radioBll.TabIndex = 7;
            this.radioBll.Text = "业务服务";
            this.radioBll.UseVisualStyleBackColor = true;
            this.radioBll.CheckedChanged += new System.EventHandler(this.radioBll_CheckedChanged);
            // 
            // cbIsGeneric
            // 
            this.cbIsGeneric.AutoSize = true;
            this.cbIsGeneric.Location = new System.Drawing.Point(338, 10);
            this.cbIsGeneric.Name = "cbIsGeneric";
            this.cbIsGeneric.Size = new System.Drawing.Size(72, 16);
            this.cbIsGeneric.TabIndex = 6;
            this.cbIsGeneric.Text = "泛型服务";
            this.cbIsGeneric.UseVisualStyleBackColor = true;
            this.cbIsGeneric.CheckedChanged += new System.EventHandler(this.cbIsGeneric_CheckedChanged);
            // 
            // tbInterfaceName
            // 
            this.tbInterfaceName.Location = new System.Drawing.Point(69, 93);
            this.tbInterfaceName.Name = "tbInterfaceName";
            this.tbInterfaceName.Size = new System.Drawing.Size(357, 21);
            this.tbInterfaceName.TabIndex = 5;
            this.tbInterfaceName.TextChanged += new System.EventHandler(this.tbInterfaceName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "接口名:";
            // 
            // tbClassFullName
            // 
            this.tbClassFullName.Location = new System.Drawing.Point(69, 66);
            this.tbClassFullName.Name = "tbClassFullName";
            this.tbClassFullName.Size = new System.Drawing.Size(357, 21);
            this.tbClassFullName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "类  名:";
            // 
            // tbServiceName
            // 
            this.tbServiceName.Location = new System.Drawing.Point(69, 35);
            this.tbServiceName.Name = "tbServiceName";
            this.tbServiceName.Size = new System.Drawing.Size(357, 21);
            this.tbServiceName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务名:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbfFormulaConfigPath);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.tbfDesc);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.tbfAssName);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.tbfInterfaceName);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.tbfClassName);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.tbfFormulaName);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.cbfIsGeneric);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(450, 279);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "公式配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbfFormulaConfigPath
            // 
            this.tbfFormulaConfigPath.Enabled = false;
            this.tbfFormulaConfigPath.Location = new System.Drawing.Point(69, 249);
            this.tbfFormulaConfigPath.Name = "tbfFormulaConfigPath";
            this.tbfFormulaConfigPath.Size = new System.Drawing.Size(357, 21);
            this.tbfFormulaConfigPath.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 252);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 32;
            this.label11.Text = "路  径:";
            // 
            // tbfDesc
            // 
            this.tbfDesc.Location = new System.Drawing.Point(69, 153);
            this.tbfDesc.Multiline = true;
            this.tbfDesc.Name = "tbfDesc";
            this.tbfDesc.Size = new System.Drawing.Size(357, 89);
            this.tbfDesc.TabIndex = 31;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 157);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 12);
            this.label16.TabIndex = 30;
            this.label16.Text = "描  述:";
            // 
            // tbfAssName
            // 
            this.tbfAssName.Enabled = false;
            this.tbfAssName.Location = new System.Drawing.Point(69, 123);
            this.tbfAssName.Name = "tbfAssName";
            this.tbfAssName.Size = new System.Drawing.Size(357, 21);
            this.tbfAssName.TabIndex = 29;
            this.tbfAssName.Text = "Vancl.TMS.BLL.dll";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 129);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "程序集:";
            // 
            // tbfInterfaceName
            // 
            this.tbfInterfaceName.Location = new System.Drawing.Point(69, 93);
            this.tbfInterfaceName.Name = "tbfInterfaceName";
            this.tbfInterfaceName.Size = new System.Drawing.Size(357, 21);
            this.tbfInterfaceName.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "接口名:";
            // 
            // tbfClassName
            // 
            this.tbfClassName.Location = new System.Drawing.Point(69, 66);
            this.tbfClassName.Name = "tbfClassName";
            this.tbfClassName.Size = new System.Drawing.Size(357, 21);
            this.tbfClassName.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 19;
            this.label13.Text = "类  名:";
            // 
            // tbfFormulaName
            // 
            this.tbfFormulaName.Location = new System.Drawing.Point(69, 35);
            this.tbfFormulaName.Name = "tbfFormulaName";
            this.tbfFormulaName.Size = new System.Drawing.Size(357, 21);
            this.tbfFormulaName.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 41);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 17;
            this.label14.Text = "服务名:";
            // 
            // cbfIsGeneric
            // 
            this.cbfIsGeneric.AutoSize = true;
            this.cbfIsGeneric.Location = new System.Drawing.Point(19, 12);
            this.cbfIsGeneric.Name = "cbfIsGeneric";
            this.cbfIsGeneric.Size = new System.Drawing.Size(72, 16);
            this.cbfIsGeneric.TabIndex = 7;
            this.cbfIsGeneric.Text = "泛型接口";
            this.cbfIsGeneric.UseVisualStyleBackColor = true;
            this.cbfIsGeneric.CheckedChanged += new System.EventHandler(this.cbfIsGeneric_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(305, 324);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(392, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "退出(&E)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(12, 329);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(266, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "没有修改功能,只能添加,错了只能手动修改哈";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(483, 355);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "城际运输系统配置文件管理工具(阉割版)";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton radioMsSql;
        private System.Windows.Forms.RadioButton radioOracle;
        private System.Windows.Forms.RadioButton radioBll;
        private System.Windows.Forms.CheckBox cbIsGeneric;
        private System.Windows.Forms.TextBox tbInterfaceName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbClassFullName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbServiceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.TextBox tbAssName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbfIsGeneric;
        private System.Windows.Forms.TextBox tbfAssName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbfInterfaceName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbfClassName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbfFormulaName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbServiceConfigPath;
        private System.Windows.Forms.TextBox tbfFormulaConfigPath;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbfDesc;
        private System.Windows.Forms.Label label16;

    }
}

