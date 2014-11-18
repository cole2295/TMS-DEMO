using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vancl.TMS.ConfigManagerTool
{
    public partial class GenericConfig : Form
    {
        public string GenericString = string.Empty;

        private List<GenericConfigModel> list = new List<GenericConfigModel>();

        public GenericConfig()
        {
            InitializeComponent();
        }

        private class GenericConfigModel
        {
            public string ClassName { get; set; }

            public string AssName { get; set; }
        }

        private string ValueChecking()
        {
            if (this.textBox1.Text.Trim() != string.Empty)
            {
                if (this.textBox2.Text.Trim() != string.Empty)
                {
                    if (this.textBox1.Text.Trim().LastIndexOf(".") == this.textBox1.Text.Trim().Length)
                    {
                        return "类名错误.";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                    return "程序集名不能为空.";
            }
            else
                return "类名不能为空.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddGeneric();
        }

        private void AddGeneric()
        {
            string error = ValueChecking();
            if (error == string.Empty)
            {
                list.Add(new GenericConfigModel()
                {
                    ClassName = this.textBox1.Text.Trim(),
                    AssName = this.textBox2.Text.Trim()
                });
                this.textBox1.Text = "";
                this.textBox2.Text = "";
            }
            else
                MessageBox.Show(error);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddGeneric();
            if (list != null)
            {
                if (list.Count > 0)
                {
                    string s = string.Empty;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (s != string.Empty)
                            s += ";";
                        s += list[i].ClassName + "," + list[i].AssName;
                    }

                    GenericString = s;
                }
            }

            this.textBox1.Text = "";
            this.textBox2.Text = "";

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
