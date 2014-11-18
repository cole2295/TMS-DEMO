using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vancl.TMS.Tools.VCMRecords.Tool.DataGrid;
using Vancl.TMS.Tools.VCMRecords.BLL;

namespace Vancl.TMS.Tools.VCMRecords.UserControls
{
    public partial class DataGrid : UserControl
    {
        #region 属性

        private PageModel _pageModel;

        /// <summary>
        /// 页面对象
        /// </summary>
        public PageModel PageModel
        {
            get
            {
                return _pageModel;
            }
            set
            {
                _pageModel = value;
                SetColumnsStyle();
            }
        }

        /// <summary>
        /// DataGrid业务处理对象
        /// </summary>
        DatagridBLL bll = new DatagridBLL();

        public delegate void DataRowDoubleClickHandler(Object obj);

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        public event DataRowDoubleClickHandler DataRowDoubleClickEvent;

        public void init()
        {
            pagesize_CMB.SelectedIndex = 0;
            this.PageModel.pageSize = getPageSizeByCMB();
            RefreshData();
        }

        #endregion

        public DataGrid()
        {
            InitializeComponent();
        }

        #region 页面事件

        private void DataGrid_Load(object sender, EventArgs e)
        {
            dataGridView.ReadOnly = true;
            InitToolTips();
        }

        /// <summary>
        /// 第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void firstPage_Btn_Click(object sender, EventArgs e)
        {
            PageModel.currentPage = 0;
            RefreshData();
        }

        /// <summary>
        /// 前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prePage_Btn_Click(object sender, EventArgs e)
        {
            if (PageModel.currentPage > 0)
            {
                PageModel.currentPage--;
                RefreshData();
            }
        }

        /// <summary>
        /// 后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextPage_Btn_Click(object sender, EventArgs e)
        {
            if (PageModel.currentPage < PageModel.pageCount - 1)
            {
                PageModel.currentPage++;
                RefreshData();
            }
        }

        /// <summary>
        /// 最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lastPage_Btn_Click(object sender, EventArgs e)
        {
            PageModel.currentPage = PageModel.pageCount - 1;
            RefreshData();
        }

        /// <summary>
        /// 跳至指定页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void currentPage_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            try
            {
                int goToPage = Convert.ToInt32(currentPage_TB.Text)-1;
                if (goToPage == PageModel.currentPage)
                    return;
                if (goToPage <0)
                    goToPage =0;
                else if (goToPage > PageModel.pageCount - 1)
                    goToPage = PageModel.pageCount - 1;
                PageModel.currentPage = goToPage;
                RefreshData();
            }
            catch
            {
                currentPage_TB.Text = (PageModel.currentPage+1).ToString();
            }
        }

        /// <summary>
        /// 每页记录数改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pagesize_CMB_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage_TB.Focus();
            PageModel.pageSize = getPageSizeByCMB();
            PageModel.currentPage = 0;
            RefreshData();
        }

        /// <summary>
        /// 通过Comobox得到当前页面显示记录数
        /// </summary>
        /// <returns></returns>
        private int getPageSizeByCMB()
        {
            int selIndx = pagesize_CMB.SelectedIndex;
            if (selIndx < 0)
                return 50;
            return Convert.ToInt32(pagesize_CMB.SelectedItem.ToString());
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Btn_Click(object sender, EventArgs e)
        {
            PageModel.count = -1;
            RefreshData();
        }

        /// <summary>
        /// 一页被双击的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex>-1&&DataRowDoubleClickEvent != null && PageModel.result != null && PageModel.result.Count > 0)
            {
                DataRowDoubleClickEvent(PageModel.result[e.RowIndex]);
            }

        }

        #endregion

        #region 方法

        /// <summary>
        /// 设置控件的ToolTips
        /// </summary>
        private void InitToolTips()
        {
            toolTip1.SetToolTip(firstPage_Btn, "第一页");
            toolTip1.SetToolTip(prePage_Btn, "前一页");
            toolTip1.SetToolTip(nextPage_Btn, "下一页");
            toolTip1.SetToolTip(lastPage_Btn, "最后一页");
        }

        /// <summary>
        /// 设置Columns的显示样式
        /// </summary>
        public void SetColumnsStyle()
        {
            if (PageModel == null)
                return;
            for (int i = 0; i < PageModel.columns.Length; i++)
            {
                Column column = PageModel.columns[i];
                DataGridViewTextBoxColumn viewcolumn = new DataGridViewTextBoxColumn();
                viewcolumn.DataPropertyName = column.name;
                viewcolumn.Name = column.name;
                viewcolumn.HeaderText = column.aliasName;
                viewcolumn.DisplayIndex = i;
                viewcolumn.Width = column.width;
                if (column.dataType != null)
                {
                    if (column.dataType == typeof(DateTime))
                    {
                        viewcolumn.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss ";
                    }
                }
                viewcolumn.Visible = column.visible;
                dataGridView.Columns.Add(viewcolumn);
            }
            dataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// 刷新页面数据
        /// </summary>
        public void RefreshData()
        {
            bll.FindPageModel(PageModel);
            this.currentPage_TB.Text = (PageModel.currentPage + 1).ToString();
            int itemCount = PageModel.count < 0 ? 0 : PageModel.count;
            int fromItem = PageModel.currentPage * PageModel.pageSize + 1;
            int toItem = fromItem + PageModel.pageSize - 1;
            if (toItem > PageModel.count)
            {
                toItem = PageModel.count;
            }
            currentPageDes_Label.Text = String.Format("共{0}页,{1}条记录，显示第{2}到{3}条记录",
                PageModel.pageCount, PageModel.count, fromItem, toItem);
            dataGridView.DataSource = PageModel.result;
            dataGridView.Refresh();
        }

        /// <summary>
        /// 重新载入数据 和RefreshData不同，此方法重新刷新页面数，并跳至第一页
        /// </summary>
        public void reloadData()
        {
            PageModel.count = -1;
            PageModel.currentPage = 0;
            RefreshData();
        }

        /// <summary>
        /// 得到选中的元素
        /// </summary>
        /// <returns></returns>
        public T[] GetSelectedItems<T>()
        {
            List<T> items = new List<T>();
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                items.Add((T)PageModel.result[row.Index]);
            }
            return items.ToArray();
        }
        
        #endregion

    }
}
