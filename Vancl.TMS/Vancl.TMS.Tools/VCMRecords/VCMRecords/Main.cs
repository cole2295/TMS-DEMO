using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Vancl.TMS.Tools.VCMRecords.MainForm.Forms;
using Vancl.TMS.Tools.VCMRecords.Entity;
using Vancl.TMS.Tools.VCMRecords.Tool;
using Vancl.TMS.Tools.VCMRecords.BLL;
using Vancl.TMS.Tools.VCMRecords.Tool.DB;
using Vancl.TMS.Tools.VCMRecords.Tool.DataGrid;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;


namespace Vancl.TMS.Tools.VCMRecords.MainForm
{
    public partial class Main : Form
    {
        #region 属性

        /// <summary>
        /// VCM日志操作业务逻辑处理层
        /// </summary>
        VCMRecordBLL vcmRecordBll = new VCMRecordBLL();

        /// <summary>
        /// 默认开始日期
        /// </summary>
        private DateTime DefaultStartTime
        {
            get
            {
                return new DateTime(2011,1,1,0,0,0);
            }
        }

        /// <summary>
        /// 默认结束时间
        /// </summary>
        private DateTime DefaultEndTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        #endregion
      
        public Main()
        {
            InitializeComponent();
        }

        #region 页面事件

        private void Main_Load(object sender, EventArgs e)
        {
            InitControlsEvent();
            InitControlsData();
            InitVCMRecordsDataGrid();
        }

        /// <summary>
        /// 按输入条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryRecord_Btn_Click(object sender, EventArgs e)
        {
            setQueryCondition();
            vcmRecordsDataGrid.reloadData();
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRecord_Menu_Click(object sender, EventArgs e)
        {
            AddOrModifyVCMRecord addRecordFom = new AddOrModifyVCMRecord(new VCMRecord());
            if (addRecordFom.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vcmRecordsDataGrid.RefreshData();
            }
        }

        /// <summary>
        /// 点击清除查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ClearInputs_Click(object sender, EventArgs e)
        {
            ClearInputs(this);
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modifyRecord_Menu_Click(object sender, EventArgs e)
        {
            VCMRecord[] delRecords = vcmRecordsDataGrid.GetSelectedItems<VCMRecord>();
            if (delRecords == null || delRecords.Length == 0)
            {
                CustomMessageBox.Tips("你没有选择要修改的记录");
                return;
            }
            if (delRecords.Length > 1)
            {
                CustomMessageBox.Tips("只能修改一条记录");
                return;
            }
            VCMRecord record =  vcmRecordsDataGrid.GetSelectedItems<VCMRecord>()[0];
            AddOrModifyVCMRecord modifyRecordFom = new AddOrModifyVCMRecord(record);
            if (modifyRecordFom.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vcmRecordsDataGrid.RefreshData();
            }
        }

        /// <summary>
        /// 删除一条VCM记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delrecord_Menu_Click(object sender, EventArgs e)
        {
            VCMRecord[] delRecords = vcmRecordsDataGrid.GetSelectedItems<VCMRecord>();
            if (delRecords==null||delRecords.Length==0)
            {
                CustomMessageBox.Tips("你没有选择要删除的记录");
                return;
            }
            if (
                CustomMessageBox.Alert(String.Format("确认删除{0}条记录？", delRecords.Length),"警告", MessageBoxButtons.YesNo)
                == System.Windows.Forms.DialogResult.Yes)
            {
                 ReturnResult result= vcmRecordBll.Remove(delRecords);
                 if (result.Result)
                 {
                     vcmRecordsDataGrid.RefreshData();
                 }
                 else
                 {
                     CustomMessageBox.Tips(result.Fault.Detail);
                 }
            }
        }

        /// <summary>
        /// 处理回车事件（回车默认是搜索）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnterClick_EventHandler(object sender, KeyEventArgs e)
        {
            setQueryCondition();
            vcmRecordsDataGrid.reloadData();
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

        #region 方法

        /// <summary>
        /// 得到查询条件
        /// </summary>
        /// <returns></returns>
        private void setQueryCondition()
        {
            string promoter = promoter_TB.Text.Trim();
            string operationer = operationer_TB.Text.Trim();
            string sql = sql_TB.Text.Trim();
            string theme = theme_TB.Text.Trim();
            List<DBCondition> conditions = new List<DBCondition>();
            if (deliverOrderID_TB.Text.Trim() != "")
            {
                try
                {
                    int orderid = Convert.ToInt32(deliverOrderID_TB.Text);
                    conditions.Add(new DBCondition("DeliverOrderID = @DeliverOrderID", "@DeliverOrderID", orderid));
                }
                catch 
                {
                    CustomMessageBox.Tips("订单号就输入整数");
                    deliverOrderID_TB.Text = "";
                    deliverOrderID_TB.Focus();
                }
            }
            if (executeStatus_CB.SelectedIndex != -1)
            {
                conditions.Add(new DBCondition("ExecuteStatus = @ExecuteStatus", "@ExecuteStatus", executeStatus_CB.SelectedIndex));
            }
            if (promoter != "")
            {
                conditions.Add(new DBCondition("PromoteEmployee like @PromoteEmployee", "@PromoteEmployee", String.Format("%{0}%",promoter)));
            }
            if (operationer != "")
            {
                conditions.Add(new DBCondition("OperationEmployee like @OperationEmployee","@OperationEmployee",  String.Format("%{0}%",operationer)));
            }
            if (sql != "")
            {
                conditions.Add(new DBCondition("SQL like @SQL", "@SQL", String.Format("%{0}%", sql)));
            }
            if (theme != "")
            {
                conditions.Add(new DBCondition("Theme like @Theme", "@Theme", String.Format("%{0}%",theme)));
            }

            conditions.Add(new DBCondition("PromoteTime > @promoteTimeStart ", "@promoteTimeStart", promoteTimeStart_DT.Value));
            conditions.Add(new DBCondition("PromoteTime < @promoteTimeEnd ", "@promoteTimeEnd", promoteTimeEnd_DT.Value));
            conditions.Add(new DBCondition("OperationTime > @operationTimeStart","@operationTimeStart", operationTimeStart_DT.Value));
            conditions.Add(new DBCondition("OperationTime > @operationTimeEnd","@operationTimeEnd", operationTimeStart_DT.Value));

            SqlParameter[] parameters;
            String whereSql = DBFunc.GetSqlAndParametersByConditions(conditions.ToArray(), out parameters);
            vcmRecordsDataGrid.PageModel.whereClause = whereSql;
            vcmRecordsDataGrid.PageModel.sqlParameters = parameters;
            vcmRecordsDataGrid.PageModel.count = -1;
        }

        /// <summary>
        /// 初使化控件相关的事件
        /// </summary>
        private void InitControlsEvent()
        {
            queryRecords_groupBox.KeyDown += OnEnterClick_EventHandler;
        }

        /// <summary>
        /// 初使化控件的数据
        /// </summary>
        private void InitControlsData()
        {
            operationTimeStart_DT.Value = DefaultStartTime;
            promoteTimeStart_DT.Value = DefaultStartTime;

            operationTimeEnd_DT.Value = DefaultEndTime;
            promoteTimeEnd_DT.Value = DefaultEndTime;
        }

        /// <summary>
        /// 清除查询条件
        /// </summary>
        private void ClearInputs(Control container)
        {
            InitControlsData();            
            foreach (Control control in container.Controls)
            {
                Type controlType = control.GetType();
                if (controlType == typeof(GroupBox) || controlType == typeof(Panel))
                {
                    ClearInputs(control);
                }
                else if (controlType == typeof(TextBox))
                {
                    control.Text = "";
                }
                else if (controlType == typeof(ComboBox))
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
            }
        }

        #endregion

        #region datagrid

        /// <summary>
        /// 初使化显示的DataGrid
        /// </summary>
        private void InitVCMRecordsDataGrid()
        {
            PageModel pageModel = new PageModel()
            {
                entityType = typeof(VCMRecord),
                tableName = "VCM_Records",
                orderFields = new String[] { "ID" },
                ordertype = OrderType.desc,
            };
            Column[] column = new Column[] { 
                new Column(){ name="id",aliasName="编号",visible=false},
                new Column(){name="DeliverOrderID",aliasName="运单号",visible=true},
                new Column(){ name="PromoteEmployee",aliasName="提出人",visible=true},
                new Column(){ name="PromoteTime",aliasName="提出时间",visible=true,dataType=typeof(DateTime),width=127},
                new Column(){ name="OperationEmployee",aliasName="操作人",visible=true},
                new Column(){ name="OperationTime",aliasName="操作时间",visible=true,dataType=typeof(DateTime),width=127},
                new Column(){ name="ExecuteStatus",aliasName="执行状态",visible = false},
                new Column(){ name="SQL",aliasName="变更SQL",visible=true},
                new Column(){ name="Theme",aliasName="主题",visible=true},
                new Column(){ name="Description",aliasName="操作描述",visible=true}
            };
            pageModel.columns = column;         
            vcmRecordsDataGrid.PageModel = pageModel;
            vcmRecordsDataGrid.init();
            vcmRecordsDataGrid.DataRowDoubleClickEvent += vcmRecordsDataGrid_DataRowDoubleClickHandler;
        }

        /// <summary>
        /// 双击记录
        /// </summary>
        /// <param name="row"></param>
        private void vcmRecordsDataGrid_DataRowDoubleClickHandler(Object record)
        {
            AddOrModifyVCMRecord modifyRecordFom = new AddOrModifyVCMRecord(record as VCMRecord);
            if (modifyRecordFom.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vcmRecordsDataGrid.RefreshData();
            }
        }

#endregion

       

      






    }
}
