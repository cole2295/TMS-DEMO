using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Configuration;

namespace Vancl.TMS.ConfigManagerTool
{
    public partial class Form1 : Form
    {
        private string configString = "<service name=\"{0}\" className=\"{1}\"  desc=\"{2}\"  loadFrom=\"{3}\" serviceName=\"{4}\"/>";
        private string formulaConfigString = "<formula name=\"{0}\" className=\"{1}\"  desc=\"{2}\"  loadFrom=\"{3}\" serviceName=\"{4}\"/>";
        private static readonly string serviceConfigPath = ConfigurationManager.AppSettings["ServiceConfigPath"].ToString();
        private static readonly string formulaConfigPath = ConfigurationManager.AppSettings["FormulaConfigPath"].ToString();
        private bool hasService = false;
        private string searchContent = string.Empty;
        private GenericConfig gc = new GenericConfig();
        private DialogResult serviceRes;
        private DialogResult formulaRes;

        public Form1()
        {
            InitializeComponent();
            this.tbServiceConfigPath.Text = serviceConfigPath;
            this.tbfFormulaConfigPath.Text = formulaConfigPath;
        }

        private void radioBll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBll.Checked)
            {
                this.tbAssName.Text = "Vancl.TMS.BLL.dll";
                this.tbInterfaceName.Text = "Vancl.TMS.IBLL.";
                this.tbInterfaceName.ForeColor = Color.Red;
                searchContent = "<!--业务层服务END-->";
                hasService = true;
            }
        }

        private void radioOracle_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioOracle.Checked)
            {
                this.tbAssName.Text = "Vancl.TMS.DAL.Oracle.dll";
                this.tbInterfaceName.Text = "Vancl.TMS.IDAL.";
                this.tbInterfaceName.ForeColor = Color.Red;
                searchContent = "<!--ORACLE数据层服务END-->";
                hasService = true;
            }
        }

        private void radioMsSql_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioMsSql.Checked)
            {
                this.tbAssName.Text = "Vancl.TMS.DAL.Sql2008.dll";
                this.tbInterfaceName.Text = "Vancl.TMS.IDAL.";
                this.tbInterfaceName.ForeColor = Color.Red;
                searchContent = "<!--SQL SERVER数据层服务END-->";
                hasService = true;
            }
        }

        private void tbInterfaceName_TextChanged(object sender, EventArgs e)
        {
            this.tbInterfaceName.ForeColor = Color.Black;
        }

        private bool HasFormula()
        {
            string serviceName = this.tbfFormulaName.Text.Trim();
            string interfaceName = this.tbfInterfaceName.Text.Trim();
            string className = this.tbfClassName.Text.Trim();
            string assName = this.tbfAssName.Text.Trim();

            if (serviceName == string.Empty || interfaceName == string.Empty || className == string.Empty || assName == string.Empty)
                return false;
            else
                return true;
        }

        private string FormulaValueChecking()
        {
            string serviceName = this.tbfFormulaName.Text.Trim();
            string interfaceName = this.tbfInterfaceName.Text.Trim();
            string className = this.tbfClassName.Text.Trim();
            string assName = this.tbfAssName.Text.Trim();

            if (serviceName == string.Empty)
                return "公式名不能为空.";
            else if (interfaceName == string.Empty)
                return "公式接口名不能为空.";
            else if (className == string.Empty)
                return "公式类名不能为空.";
            else if (assName == string.Empty)
                return "公式程序集不能为空.";

            else if (className.LastIndexOf(".") == (className.Length - 1))
                return "公式类名错误.";
            else
                return string.Empty;
        }

        private string ServiceValueChecking()
        {
            string serviceName = this.tbServiceName.Text.Trim();
            string interfaceName = this.tbInterfaceName.Text.Trim();
            string className = this.tbClassFullName.Text.Trim();
            string assName = this.tbAssName.Text.Trim();

            if (serviceName == string.Empty)
                return "服务名不能为空.";
            else if (interfaceName == string.Empty)
                return "服务接口名不能为空.";
            else if (className == string.Empty)
                return "服务类名不能为空.";
            else if (assName == string.Empty)
                return "服务程序集不能为空.";

            else if (className.LastIndexOf(".") == (className.Length - 1))
                return "服务类名错误.";
            else
                return string.Empty;
        }

        private void cbIsGeneric_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsGeneric.Checked)
            {
                serviceRes = gc.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (hasService)
            {
                string error = ServiceValueChecking();
                if (error != string.Empty)
                {
                    MessageBox.Show(error);
                    return;
                }
                SaveServiceConfig();
            }
            if (HasFormula())
            {
                string error = FormulaValueChecking();
                if (error != string.Empty)
                {
                    MessageBox.Show(error);
                    return;
                }
                SaveFormulaConfig();
            }
            ClearAllContent(new Control[] { this.tabPage1, this.tabPage2, gc });

            MessageBox.Show("保存成功");
        }

        private void SaveFormulaConfig()
        {
            string serviceName = this.tbfFormulaName.Text.Trim();
            string interfaceName = this.tbfInterfaceName.Text.Trim();
            string className = this.tbfClassName.Text.Trim();
            string assName = this.tbfAssName.Text.Trim();
            string desc = this.tbfDesc.Text.Trim().Replace("\r\n", "");

            if (this.cbfIsGeneric.Checked)
            {
                if (formulaRes == DialogResult.OK)
                {
                    if (gc.GenericString != string.Empty)
                    {
                        interfaceName += ";" + gc.GenericString;
                    }
                }
            }

            string s = String.Format(formulaConfigString, serviceName, className, desc, assName, interfaceName);

            XMLDoc doc = new XMLDoc(formulaConfigPath);

            doc.AppendConfigSection("</Formulas>", s);
            doc.Save();
        }

        public static void ClearAllContent(Control[] forms)
        {
            if (forms != null)
            {
                if (forms.Length > 0)
                {
                    foreach (Control form in forms)
                    {
                        foreach (Control item in form.Controls)
                        {
                            if (item is TextBox)
                            {
                                ((TextBox)item).Clear();
                            }
                            if (item is CheckBox)
                            {
                                ((CheckBox)item).Checked = false;
                            }
                            if (item is RadioButton)
                            {
                                ((RadioButton)item).Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void SaveServiceConfig()
        {
            string serviceName = this.tbServiceName.Text.Trim();
            string interfaceName = this.tbInterfaceName.Text.Trim();
            string className = this.tbClassFullName.Text.Trim();
            string assName = this.tbAssName.Text.Trim();
            string desc = this.tbDesc.Text.Trim().Replace("\r\n", "");

            if (this.cbIsGeneric.Checked)
            {
                if (serviceRes == DialogResult.OK)
                {
                    if (gc.GenericString != string.Empty)
                    {
                        interfaceName += ";" + gc.GenericString;
                    }
                }
            }

            string s = String.Format(configString, serviceName, className, desc, assName, interfaceName);

            XMLDoc doc = new XMLDoc(serviceConfigPath);

            doc.AppendConfigSection(searchContent, s);
            doc.Save();
        }

        public class XMLDoc : IDisposable
        {
            private XmlDocument doc = new XmlDocument();

            private string filePath = string.Empty;

            public XMLDoc(string fileName)
            {
                this.filePath = fileName;
                Loader();
            }

            public void Save()
            {
                doc.Save(filePath);
            }

            public string AppendConfigSection(string searchContent, string content)
            {
                string s = XMLToString();
                int index = s.LastIndexOf(searchContent);
                doc.InnerXml = AppendContentAt(index, Environment.NewLine + content);

                return doc.InnerXml;
            }

            private string AppendContentAt(int index, string content)
            {
                string s = XMLToString();
                StringBuilder sb = new StringBuilder();
                string befString = s.Substring(0, index);
                string afterString = s.Substring(index, s.Length - index);
                sb.Append(befString);
                sb.Append(content);
                sb.Append(afterString);

                return sb.ToString();
            }

            public XmlNodeList GetElementsByTagName(string tagName)
            {
                XmlElement root = GetRootElement();
                return root.GetElementsByTagName(tagName);
            }

            public XmlElement GetRootElement()
            {
                if (doc != null)
                {
                    return doc.DocumentElement;
                }
                else
                    return null;
            }

            private XmlDocument Loader()
            {
                if (this.filePath != string.Empty)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder("");
                        using (StreamReader sr = new StreamReader(this.filePath, System.Text.Encoding.GetEncoding("GB2312")))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                sb.Append(line);
                            }
                        }

                        doc.LoadXml(sb.ToString());

                        return doc;
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                    return null;
            }

            public string XMLToString()
            {
                if (doc != null)
                {
                    return doc.InnerXml;
                }
                else
                    return string.Empty;
            }

            #region IDisposable 成员

            public void Dispose()
            {
                if (doc != null)
                {
                    this.Save();
                    doc = null;
                }
            }

            #endregion
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbfIsGeneric_CheckedChanged(object sender, EventArgs e)
        {
            if (cbfIsGeneric.Checked)
            {
                formulaRes = gc.ShowDialog();
            }
        }
    }
}
