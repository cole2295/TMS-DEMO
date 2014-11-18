using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Export;
using Vancl.TMS.IBLL.Transport.Plan;
using Vancl.TMS.Model.Transport.Plan;

namespace LineIDRepair
{
    public partial class frmMain : Form
    {
        private ILinePlanBLL _linePlanBLL = ServiceFactory.GetService<ILinePlanBLL>();
        ITransportPlanBLL _transportPlanBLL = ServiceFactory.GetService<ITransportPlanBLL>();
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLinePalnBackup_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "csv";
            sfd.Filter = "CSV Files | *.csv";
            sfd.FileName = "线路计划线路编号备份.csv";
            string ErrorMessage = "";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<LinePlanLineIDRepairModel> lstModel = _linePlanBLL.GetAllValidLinePlan();
                if (CSVExport.Export<LinePlanLineIDRepairModel>(lstModel, sfd.FileName, out ErrorMessage))
                {
                    MessageBox.Show("备份成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(ErrorMessage, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            List<LinePlanLineIDRepairModel> lstModel = _linePlanBLL.GetAllValidLinePlan();
            ResultModel rm = _linePlanBLL.RepairLineID(lstModel);
            if (rm.IsSuccess)
            {
                MessageBox.Show(rm.Message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(rm.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnCancel.Enabled = true;
        }

        private void btnLinePlanRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "csv";
            ofd.Filter = "CSV Files | *.csv";
            string ErrorMessage = "";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<LinePlanLineIDRepairModel> lstModel = CSVExport.Import<LinePlanLineIDRepairModel>(ofd.FileName, out ErrorMessage);
                if (lstModel == null)
                {
                    MessageBox.Show(ErrorMessage, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lstModel.Count == 0)
                {
                    MessageBox.Show("没有可恢复的数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ResultModel rm = _linePlanBLL.RestoreLineID(lstModel);
                if (rm.IsSuccess)
                {
                    MessageBox.Show(rm.Message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(rm.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTransportPlanBackup_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "csv";
            sfd.Filter = "CSV Files | *.csv";
            sfd.FileName = "运输计划线路编号备份.csv";
            string ErrorMessage = "";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<TransportPlanDetailLineIDRepairModel> lstTPDModel = _transportPlanBLL.GetValidTransportPlanDetail();
                if (lstTPDModel == null || lstTPDModel.Count == 0)
                {
                    MessageBox.Show("没有可备份的数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                };
                if (CSVExport.Export<TransportPlanDetailLineIDRepairModel>(lstTPDModel, sfd.FileName, out ErrorMessage))
                {
                    MessageBox.Show("备份成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(ErrorMessage, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTransportPlanRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "csv";
            ofd.Filter = "CSV Files | *.csv";
            string ErrorMessage = "";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<TransportPlanDetailLineIDRepairModel> lstModel = CSVExport.Import<TransportPlanDetailLineIDRepairModel>(ofd.FileName, out ErrorMessage);
                if (lstModel == null)
                {
                    MessageBox.Show(ErrorMessage, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lstModel.Count == 0)
                {
                    MessageBox.Show("没有可恢复的数据！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lstModel.ForEach(m => m.NewLineID = m.LineID);
                ResultModel rm = _transportPlanBLL.RestoreLineID(lstModel);
                if (rm.IsSuccess)
                {
                    MessageBox.Show(rm.Message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(rm.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
